using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Events;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Geometry;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Engine.Water.Render;
using System;
using System.IO;
using System.Linq;
using static Shared_Lib.IO.UT_System;
using Point = System.Drawing.Point;

namespace Simple_Engine.Engine.Space.Camera
{
    public partial class CameraModel : IRenderable
    {
        private Line cameraLine;

        public CameraModel()
        {
        }

        public CameraModel(SceneModel activeScene, bool loadUI)
        {
            Name = "Active Camera";
            this.scene = activeScene;
            

            ActivatePrespective();
            scene.game.MouseDown += Game_MouseDown;
            scene.game.Load += Game_Load;
            Animate = new AnimationComponent(this);
        }

        public AnimationComponent Animate { get; set; }
        public Vector4 DefaultColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
                    cameraLine = new Line(Position, Target);
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
            ProjectionTransform = Matrix4.CreateOrthographic(Width, Height, NearDistance, FarDistance);
            IsPrespective = false;
        }

        public void ActivatePrespective()
        {
            ProjectionTransform = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), (float)Width / Height, NearDistance, FarDistance);

            IsPrespective = true;
        }

        public void AnimateCamreaPosition(Vector3 ToPosition, float duration = 1000)
        {
            Animate.AnimPositions.Add(new AnimVector3(this, duration, Position, ToPosition, (x) =>
            {
                Position = x;
                UpdateCamera();
            }));
        }

        public void AnimateCamreaTarget(Vector3 ToTargetPosition, float duration = 1000)
        {
            Animate.AnimPositions.Add(new AnimVector3(this, duration, Target, ToTargetPosition, (x) =>
              {
                  Target = x;
                  UpdateCamera();
              }));
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

        public void Live_Update(Shader ShaderModel)
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
                if (model.ShaderModel.EnableInstancing) continue;

                if (!found)
                {
                    var baseGeo = model as Base_Geo;

                    //  var res = baseGeo.Intersect(mouseRayVector, activeScene.ActiveCamera.Position);

                    GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, targetfbo.FBOId);
                    GL.ReadBuffer(ReadBufferMode.ColorAttachment1);
                    float[] pixelColor = new float[4];
                    GL.ReadPixels(mousePosition.X, Game.Instance.Height - mousePosition.Y - 1, 1, 1, PixelFormat.Rgba, PixelType.Float, pixelColor);
                    GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);

                    //check if the mouse is indeed over the model, or just close by another object
                    var pickedColor = new Vector4((float)Math.Round(pixelColor[0], 2), (float)Math.Round(pixelColor[1], 2), (float)Math.Round(pixelColor[2], 2), (float)Math.Round(pixelColor[3], 2));

                    model.Set_Selected(model.DefaultColor == pickedColor);

                    if (model.GetSelected())
                    {
                        found = true;
                        selectedModel = model;

                        var worldRay = Extract_RayFromScreen(mousePosition);
                        var p = ((Base_Geo3D)selectedModel).Intersect(worldRay.Normalized(), Position);
                        if (p.Distance != 0 && p.NormalPlan.Length != 0)
                        {
                        }
                        continue;
                    }
                }

                model?.Set_Selected(false);
            }
            Base_Geo.SelectedModel = selectedModel as Base_Geo;
            return selectedModel;
        }

        public void PostRender(Shader ShaderModel)
        {
            throw new NotImplementedException();
        }

        public void PrepareForRender(Shader shaderModel)
        {
            throw new NotImplementedException();
        }

        public void RenderModel()
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
            Evaluate_UPVector();
            ViewTransform = Matrix4.LookAt(Position, Target, UP);
        }

        public void UpdateFOV(float value)
        {
            FOV = MathHelper.Clamp(value, 1, 90);
            ActivatePrespective();
        }

        public void UploadDefaults(Shader ShaderModel)
        {
            ShaderModel.SetFloat(ShaderModel.Location_NearDistance, NearDistance);
            ShaderModel.SetFloat(ShaderModel.Location_FarDistance, FarDistance);
        }

        internal void ScopeTo(IRenderable.BoundingBox modelBBX)
        {
            var target = modelBBX.CG;
            var direction = Position - target;
            var range = (modelBBX.Max - modelBBX.CG).Length * 2;
            var pos = target + direction.Normalized() * range;
            CameraModel.ActiveCamera.AnimateCamreaPosition(pos);
            CameraModel.ActiveCamera.AnimateCamreaTarget(target);
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
            if (!IsPrespective)
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