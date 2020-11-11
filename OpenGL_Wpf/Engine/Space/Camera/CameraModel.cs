using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Events;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Geometry.SystemModel.Clips;
using Simple_Engine.Engine.Geometry.ThreeDModels;
using Simple_Engine.Engine.Render.ShaderSystem;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Engine.Water.Render;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Point = System.Drawing.Point;

namespace Simple_Engine.Engine.Space.Camera
{
    public partial class CameraModel : IRenderable
    {
        private Line3D cameraLine;

        //use setGlobal from clip instance
        public static bool EnableClipPlans;

        //Global
        public static ClipPlan ClipPlanX;

        public static ClipPlan ClipPlanY;
        public static ClipPlan ClipPlanZ;

        public ReadBufferMode ColorChannel = ReadBufferMode.ColorAttachment0;

        public enum CameraType
        {
            Perspective,
            Plan,
            Section,
            None,
        }

        public CameraType ViewType { get; set; }

        public static CameraModel PerspectiveCamera { get; set; }
        public static CameraModel PlanCamera { get; internal set; }

        public CameraModel(SceneModel activeScene, CameraType plan)
        {
            this.scene = activeScene;
            ViewType = plan;
            switch (plan)
            {
                case CameraType.Perspective:
                    ActivatePrespective();
                    break;

                case CameraType.Plan:
                    Activate_Ortho();

                    break;

                case CameraType.Section:
                    Activate_Ortho();

                    break;

                case CameraType.None:
                    break;

                default:
                    break;
            }
        }

        public void Add_Clips()
        {
            ClipPlans = new List<ClipPlan>();
            scene.RemoveModels(ClipPlanX);
            scene.RemoveModels(ClipPlanY);
            scene.RemoveModels(ClipPlanZ);

            ClipPlanX = ClipPlan.Generate_ClipPlan(Vector3.UnitX, OpenTK.Graphics.OpenGL.EnableCap.ClipDistance0);
            ClipPlanY = ClipPlan.Generate_ClipPlan(Vector3.UnitY, OpenTK.Graphics.OpenGL.EnableCap.ClipDistance1);
            ClipPlanZ = ClipPlan.Generate_ClipPlan(Vector3.UnitZ, OpenTK.Graphics.OpenGL.EnableCap.ClipDistance2);

            ClipPlanX.IsActive = false;
            ClipPlanY.IsActive = false;
            ClipPlanZ.IsActive = false;

            ClipPlans.Add(ClipPlanX);
            ClipPlans.Add(ClipPlanY);
            ClipPlans.Add(ClipPlanZ);

            scene.UpLoadModels(ClipPlanX);
            scene.UpLoadModels(ClipPlanY);
            scene.UpLoadModels(ClipPlanZ);
        }

        public void Set_IsDirectionVisible(bool value)
        {
            if (!value)
            {
                cameraLine.IsActive = false;
            }
            else
            {
                if (cameraLine == null)
                {
                    cameraLine = new Line3D(Position, Target);
                    cameraLine.IsSystemModel = true;
                    cameraLine.DefaultColor = new Vector4(1, 0, 0, 1);
                }
                cameraLine.IsActive = true;

                SceneModel.ActiveScene.UpLoadModels(cameraLine);
            }
            IsDirectionVisible = value;
        }

        public void Activate_Ortho()
        {
            ProjectionTransform = Matrix4.CreateOrthographic(Width, height, NearDistance, FarDistance);
            IsPerspective = false;
        }

        public void ActivatePrespective()
        {
            ProjectionTransform = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), DisplayManager.DisplayRatio, NearDistance, FarDistance);
            ViewType = CameraType.Perspective;
            IsPerspective = true;
        }

        public void AttachTargetTo(Base_Geo model)
        {
            if (hoockedModel != null)
            {
                model.MoveEvent -= modelMove;
                hoockedModel = model;
            }
            model.MoveEvent += modelMove;
        }

        public void BuildModel()
        {
            throw new NotImplementedException();
        }

        public void Circulate(float time)
        {
            var camx = (float)Math.Sin(time);
            var camy = (float)Math.Cos(time);
            Position = new Vector3(camx, 0, camy);
            UP = new Vector3(0, 1, 0);
            UpdateCamera();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Evaluate_RightVector()
        {
            var tempUP = new Vector3(0, 1, 0);
            Right = Vector3.Cross(tempUP, Direction).Normalized();
        }

        public void Evaluate_UPVector()
        {
            Evaluate_DirectionVector();
            Evaluate_RightVector();
            UP = Vector3.Cross(Direction, Right);
        }

        //RayCast Explanation: https://antongerdelan.net/opengl/raycasting.html
        //https://www.youtube.com/watch?v=DLKN0jExRIM
        public Vector3 Extract_RayFromScreen(Point mousePosition)
        {
            Vector3 ray_NDCEnd = Get_NormalizedDeviceCoordinates(mousePosition);

            //02- Moving To Clip Space Coordinates
            //Since OpenGL Style is -Z direction Pointing to inside the Screen and in Perspective Mode, then we need Vec4 with -Z
            var ray_clipEnd = new Vector4(ray_NDCEnd.X, ray_NDCEnd.Y, -1, 1);

            //Now ray_Eye is a point and not a vector since we meaningfully transformed point ray-clip to inverse projection. hence, setting z =-1 and w =0 ;
            //This is at the near plan
            Vector4 ray_EyeEnd = Get_RayInEyeSpace(ray_clipEnd);

            //04- Move to 4D World Coordinates
            //Back more Step by multiplying by inverse of View Transform, then Normalize the resulted Ray since we need a Vector Ray and not Point
            var ray_WCOEnd = Get_RayInWorldSpace(ray_EyeEnd);

            return ray_WCOEnd;
        }

        public void Live_Update(Base_Shader ShaderModel)
        {
            ShaderModel.SetMatrix4(ShaderModel.Location_ViewTransform, ViewTransform);
            ShaderModel.SetMatrix4(ShaderModel.Location_ProjectionTransform, ProjectionTransform);
        }

        public IRenderable Load(string path)
        {
            return null;
        }

        public ISelectable PickObject(Point mousePosition)
        {
            FBO_MTargets targetfbo = Game.Instance.mTargets_FBO;

            ISelectable selectedModel = null;
            bool found = false;
            for (int i = 0; i < SceneModel.ActiveScene.geoModels.Count; i++)
            {
                var model = SceneModel.ActiveScene.geoModels.ElementAt(i) as ISelectable;
                if (model is null) continue;
                if (model.GetShaderModel().EnableInstancing) continue;

                if (!found)
                {
                    var baseGeo = model as Base_Geo;

                    Vector4 pickedColor = targetfbo.GetPixelColorFromFrameBufferObject(ref mousePosition, ReadBufferMode.ColorAttachment1, 2);

                    int VertexId = targetfbo.GetIntegerFromFrameBufferObject(ref mousePosition, ReadBufferMode.ColorAttachment2);
                    var model3d = ((Base_Geo3D)model);

                    Base_Geo3D.SelectedVertex = model3d.Positions.ElementAtOrDefault(VertexId);

                    if (model3d.GetShaderModel() is Geo_Shader)
                    {
                        model3d.GetShaderModel().RunOnUIThread.Push(() =>
                        {
                            model3d.GeoPointShader.SetInt(model3d.GeoPointShader.Location_SelectedVertex, VertexId);
                        });
                    }

                    if (model.DefaultColor == pickedColor)
                    {
                        found = true;
                        selectedModel = model;

                        var worldRay = Extract_RayFromScreen(mousePosition);
                        if (selectedModel is Base_Geo3D)
                        {
                            var p = ((Base_Geo3D)selectedModel).Intersect(worldRay.Normalized(), Position);
                            if (p.Distance != 0 && p.NormalPlan.Length != 0)
                            {
                            }
                        }
                        continue;
                    }
                }
            }

            return selectedModel;
        }

        internal static CameraModel Create(SceneModel scene, CameraType viewType)
        {
            var cam = new CameraModel(scene, viewType);
            cam.Target = new Vector3(0, 0, 0);

            switch (viewType)
            {
                case CameraType.Perspective:
                    {
                        cam.ActivatePrespective();
                        cam.Position = new Vector3(-2, 0, -2);

                        cam.Add_Clips();
                    }
                    break;

                case CameraType.Plan:
                    {
                        cam.SetHeight(100);
                        cam.Position = new Vector3(0, 10, 0);
                        cam.Activate_Ortho();
                    }
                    break;

                case CameraType.Section:
                    break;

                case CameraType.None:
                    break;

                default:
                    break;
            }

            cam.UpdateCamera();

            return cam;
        }

        public void PostRender(Base_Shader ShaderModel)
        {
            throw new NotImplementedException();
        }

        public void PrepareForRender(Base_Shader shaderModel)
        {
            throw new NotImplementedException();
        }

        public void UploadVAO()
        {
        }

        public string Save()
        {
            string path = Path.GetTempPath() + "Camera.ssd";
            Engine.Core.Serialize.IO.Save(this, path);

            return path;
        }

        public void UpdateBoundingBox()
        {
            throw new NotImplementedException();
        }

        public void UpdateCamera()
        {
            switch (ViewType)
            {
                default:
                case CameraType.Perspective:
                    UP = new Vector3(0, 1, 0);
                    Evaluate_UPVector();
                    break;

                case CameraType.Plan:
                    UP = new Vector3(0, 0, 1);
                    Right = new Vector3(-1, 0, 0);
                    Target = Position * new Vector3(1, 0, 1);
                    break;

                case CameraType.Section:
                    UP = new Vector3(1, 0, 0);
                    Right = new Vector3(0, 1, 0);

                    break;

                case CameraType.None:
                    //Up vector is calculated externally
                    break;
            }

            ViewTransform = Matrix4.LookAt(Position, Target, UP);
        }

        public void UpdateFOV(float value)
        {
            FOV = MathHelper.Clamp(value, 1, 90);
            ActivatePrespective();
        }

        public void UploadDefaults(Base_Shader ShaderModel)
        {
            ShaderModel.SetFloat(ShaderModel.Location_NearDistance, NearDistance);
            ShaderModel.SetFloat(ShaderModel.Location_FarDistance, FarDistance);
        }

        internal void ScopeTo(IRenderable.BoundingBox modelBBX)
        {
            var target = modelBBX.GetCG();
            var direction = Position - target;
            float range = 0;
            Vector3 pos = new Vector3();
            if (CameraModel.ActiveCamera.IsPerspective)
            {
                range = (modelBBX.Max - modelBBX.GetCG()).Length * 2;
                pos = target + direction.Normalized() * range;
            }
            else
            {
                range = modelBBX.Max.Y - modelBBX.Min.Y;
                pos = target + new Vector3(0, range + 5, 0); //5: is just a safe distance to avoid setting camera inside the bbx
            }
            var targetheight = (modelBBX.Max - modelBBX.Min).Length;

            CameraModel.ActiveCamera.AnimateCameraPosition(pos);
            CameraModel.ActiveCamera.AnimateCameraTarget(target);
            CameraModel.ActiveCamera.AnimateCameraHeight(targetheight);
        }

        public void AlignCamera(IRenderable.BoundingBox modelBBX)
        {
            var target = modelBBX.GetCG();
            var direction = Position - target;
            var range = (modelBBX.Max - modelBBX.GetCG()).Length * 2;
            Vector3 pos = new Vector3();

            if (ViewType == CameraType.Plan)
            {
                pos = modelBBX.GetCG() * new Vector3(1, 2, 1) + new Vector3(0, 10, 0);
                var dim = modelBBX.GetDimensions();
                SetHeight(Math.Max(dim.X, dim.Z));
            }
            else
            {
                pos = target + direction.Normalized() * range;
                SetHeight((modelBBX.Max - modelBBX.Min).Length);
            }

            Position = pos;
            Target = target;

            UpdateCamera();
        }

        private void Evaluate_DirectionVector()
        {
            Direction = (Position - Target).Normalized();//this direction is inverted
        }

        private Vector3 Get_NormalizedDeviceCoordinates(Point mousePosition)
        {
            //01- Transfer Mouse point to NDC normalized Device Coordinates.
            //Scale the range and invert Y direction since the Y at screen is @ Top while OpenGL @ Bottom
            float x = 2.0f * (float)mousePosition.X / scene.game.Width - 1.0f;
            float y = (2.0f * (float)mousePosition.Y / scene.game.Height) - 1.0f;
            var ray_NDCEnd = new Vector3(x, -y, 1); //-y since the above method is extracted from LWJGL

            return ray_NDCEnd;
        }

        private Vector4 Get_RayInEyeSpace(Vector4 ray_clipEnd)
        {
            //03-Moving to Camera Space Coordinates
            //this will require multiplying the ray with the inverse of the Projection Matrix (Going backward concept)
            var ray_EyeEnd = ProjectionTransform.Inverted() * ray_clipEnd;
            return new Vector4(ray_EyeEnd.X, ray_EyeEnd.Y, -1, 0);
        }

        private Vector3 Get_RayInWorldSpace(Vector4 ray_EyeEnd)
        {
            var ray_wrdEnd = ViewTransform * ray_EyeEnd;
            ray_wrdEnd.Xyz.Normalize();
            return new Vector3(ray_wrdEnd);
        }

        private void modelMove(object sender, MoveingEvent e)
        {
            var newLocation = e.Transform.ExtractTranslation();
            var diff = newLocation - Target;
            Position += diff;
            Target += diff;

            UpdateCamera();
        }

        public void UpdateViewMode()
        {
            if (!IsPerspective)
            {
                Activate_Ortho();
            }
            else
            {
                ActivatePrespective();
            }
        }

        public void Render_UIControls()
        {
            throw new NotImplementedException();
        }
    }
}