using com.sun.java.swing.plaf.motif;
using ImGuiNET;
using Simple_Engine.CoreModel;
using Simple_Engine.Views.ThreeD.Engine.Core;
using Simple_Engine.Views.ThreeD.Engine.Core.Abstracts;
using Simple_Engine.Views.ThreeD.Engine.Core.Events;
using Simple_Engine.Views.ThreeD.Engine.Core.Interfaces;
using Simple_Engine.Views.ThreeD.Engine.Core.Serialize;
using Simple_Engine.Views.ThreeD.Engine.GameSystem;
using Simple_Engine.Views.ThreeD.Engine.Geometry;
using Simple_Engine.Views.ThreeD.Engine.ImGui_Set;
using Simple_Engine.Views.ThreeD.Engine.ImGui_Set.Controls;
using Simple_Engine.Views.ThreeD.Engine.Render;
using Simple_Engine.Views.ThreeD.Engine.Water.Render;
using Simple_Engine.Views.ThreeD.Extentions;
using Simple_Engine.Views.ThreeD.ToolBox;
using Newtonsoft.Json;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenXmlPowerTools;
using Shared_Lib.Extention;
using Shared_Lib.Extention.Serialize_Ex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using Point = System.Drawing.Point;

namespace Simple_Engine.Views.ThreeD.Engine.Space
{
    public class CameraModel : IRenderable
    {
        public static CameraModel ActiveCamera;
        private Base_Geo hoockedModel;
        private Scene scene;
        private bool loadUI;

        private Point StartPoint = new Point();

        public CameraModel()
        {
        }

        public CameraModel(Scene activeScene, bool loadUI)
        {
            Name = "Active Camera";
            this.scene = activeScene;
            this.loadUI = loadUI;

            ActivatePrespective();
            scene.game.MouseDown += Game_MouseDown;
            scene.game.Load += Game_Load;
            Animate = new AnimationComponent(this);
        }

        public AnimationComponent Animate { get; set; }

        [JsonIgnore]
        public IRenderable.BoundingBox BBX { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Vector3 Direction { get; set; } = new Vector3(0, 0, 1);

        public float FarDistance { get; set; } = 2000f;

        public float FOV { get; private set; } = 45;

        public float Height { get; set; } = 10;

        public int Id { get; set; }

        public bool IsPrespective { get; set; } = false;

        public string Name { get; set; }

        public float NearDistance { get; set; } = .1f;

        public float Pitch { get; set; }

        public Vector3 Position { get; set; }

        public Matrix4 ProjectionTransform { get; set; } = Matrix4.Identity;

        public Vector3 Right { get; set; } = new Vector3(1, 0, 0);

        public float Roll { get; set; }

        public Vector3 Target { get; set; }

        public ImgUI_Controls Ui_Controls { get; set; }

        public Vector3 UP { get; set; } = new Vector3(0, 1, 0);

        public Matrix4 ViewTransform { get; set; } = Matrix4.Identity;

        public float Width { get; set; } = 10;

        //how much we are looking up - down wards
        public float Yaw { get; set; }

        public void Activate_Ortho()
        {
            ProjectionTransform = Matrix4.CreateOrthographic(Width, Height, NearDistance, FarDistance);
            IsPrespective = false;
        }

        //how much we are looking left or right
        public void Activate_PanCameraModel()
        {
 
            scene.game.MouseMove += (s, e) =>
            {
                if (e.Mouse.MiddleButton == ButtonState.Pressed)
                {
                    PanCamera(e.Position);
                }
            };
        }

        public void ActivatePrespective()
        {
            ProjectionTransform = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), (float)Width / Height, NearDistance, FarDistance);

            IsPrespective = true;
        }

        public void AnimateCamrea(Vector3 ToPosition, Vector3 ToTargetPosition)
        {
            float duration = 1000;
            Animate.AnimPositions.Add(new AnimVector3(this, duration, Position, ToPosition, (x) =>
            {
                Position = x;
                UpdateCamera();
            }));

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
            float r = 10;
            var camx = (float)Math.Sin(time);
            var camy = (float)Math.Cos(time);
            Position = new Vector3(camx, 0, camy);
            UP = new Vector3(0, 1, 0);
            UpdateCamera();
        }

        public void Create_UIControls()
        {
            Ui_Controls = new Imgui_Window($"Camera {Name}");

            new Imgui_String(Ui_Controls, "Name", () => Name, (x) =>
            {
                Name = x;
            });

            new Imgui_DragFloat(Ui_Controls, "Width", () => Width, (x) =>
            {
                Width += x;
                UpdateViewMode();
            });
            new Imgui_DragFloat(Ui_Controls, "Height", () => Height, (x) =>
            {
                Height += x;
                UpdateViewMode();
            });

            new Imgui_DragFloat3(Ui_Controls, "Position", () => Position, (x) =>
            {
                Position += x;
                UpdateCamera();
            });

            new Imgui_CheckBox(Ui_Controls, "Display Mode", () => IsPrespective, (x) =>
            {
                if (x)
                {
                    ActivatePrespective();
                }
                else
                {
                    Activate_Ortho();
                }
            });
        }

        private void UpdateViewMode()
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
            return Core.Serialize.Import.LoadModel<CameraModel>(path);
        }

        public void PanCamera(Point position)
        {
            if (Imgui_Helper.IsAnyCaptured()) return;
            float speed = GetSpeed();

            var dx = position.X - StartPoint.X;
            var dy = position.Y - StartPoint.Y;

            var HorztransVector = Right * (-dx * speed);
            var VertransVector = UP * (dy * speed);
            var transVector = HorztransVector + VertransVector;
            Position += transVector;

            if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft))
            {
                Target -= transVector;
            }
            else
            {
                Target += transVector;
            }

            UpdateCamera();
            StartPoint = position;
        }

        public ISelectable PickObject(Point mousePosition, Scene activeScene, FBO_MTargets targetfbo)
        {
            //var mouseRayVector = Extract_RayFromScreen(mousePosition);
            float? dist = null;
            ISelectable selectedModel = null;
            bool found = false;
            for (int i = 0; i < activeScene.geoModels.Count; i++)
            {
                var model = activeScene.geoModels.ElementAt(i) as ISelectable;
                if (model is null) continue;
                if (model.ShaderModel.EnableInstancing) continue;

                if (!found)
                {
                    var baseGeo = model as Base_Geo;

                    //  var res = baseGeo.Intersect(mouseRayVector, activeScene.ActiveCamera.Position);

                    GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, targetfbo.FBOId);
                    GL.ReadBuffer(ReadBufferMode.ColorAttachment1);
                    float[] pixelColor = new float[4];
                    GL.ReadPixels(mousePosition.X, Game.Context.Height - mousePosition.Y - 1, 1, 1, PixelFormat.Rgba, PixelType.Float, pixelColor);
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

        public void Render_UIControls()
        {
            Ui_Controls?.BuildModel();
        }

        public void RenderModel()
        {
            throw new NotImplementedException();
        }

        public string Save()
        {
            string path = Path.GetTempPath() + "Camera.ssd";
            Engine.Core.Serialize.IO.Save(this, path);

            return path;
        }

        public void Setup_Events()
        {
            Activate_PanCameraModel();
            Activate_MoveTarget();
            scene.game.MouseWheel += Game_MouseWheel;
            scene.game.KeyDown += Game_KeyDown;
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
            CameraModel.ActiveCamera.AnimateCamrea(pos, target);
        }

        private void Activate_MoveTarget()
        {
            scene.game.MouseMove += (s, e) =>
            {
                if (e.Mouse.RightButton == ButtonState.Pressed)
                {
                    MoveTarget(e.Position);
                }
            };
        }

        private void Evaluate_DirectionVector()
        {
            Direction = (Position - Target).Normalized();//this direction is inverted
        }

        private void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (Imgui_Helper.IsAnyCaptured()) return;
            float speed = 1f;
            Vector3 transvector = new Vector3();

            if (e.Key == Key.C && e.Alt)
            {
                if (Base_Geo.SelectedModel == null)
                {
                    ScopeTo(scene.BBX);
                }
                else
                {
                    ScopeTo(Base_Geo.SelectedModel.BBX);
                }
            }
            // Evaluate_UPVector();
            if (Base_Geo.SelectedModel == null)
            {
                if (e.Key == Key.W)
                {
                    transvector = -speed * Direction;
                }
                if (e.Key == Key.S)
                {
                    transvector = speed * Direction;
                }
                if (e.Key == Key.A)
                {
                    transvector = Right.Normalized() * -speed;
                }
                if (e.Key == Key.D)
                {
                    transvector = Right.Normalized() * speed;
                }
                Position += transvector;
                Target += transvector;
            }

            UpdateCamera();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            Create_UIControls();
        }

        private void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartPoint = e.Position;
        }

        private void Game_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Imgui_Helper.IsAnyCaptured()) return;
            if (Keyboard.GetState().IsKeyDown(Key.AltLeft))
            {
                if (IsPrespective)
                {
                    UpdateFOV(FOV + e.Delta);
                }
            }
            else
            {
                if (IsPrespective)
                {
                    var speed = GetSpeed();

                    //Evaluate_UPVector();
                    var displacement = Direction.Normalized() * -e.Delta * speed;
                    Position += displacement;
                    if (hoockedModel == null)
                    {
                        Target += displacement;
                    }
                    UpdateCamera();
                }
                else
                {
                    Height += 10 * Math.Sign(-e.Delta);
                    Width += 10 * Math.Sign(-e.Delta);

                    Activate_Ortho();
                }
            }
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

        private float GetSpeed()
        {
            var speed = (float)DisplayManager.UpdatePeriod * .005f;

            if (Keyboard.GetState().IsKeyDown(Key.ControlLeft))
            {
                speed *= 100;
            }
            return speed;
        }

        private void modelMove(object sender, MoveingEvent e)
        {
            var newLocation = e.Transform.ExtractTranslation();
            var diff = newLocation - Target;
            Position += diff;
            Target += diff;

            UpdateCamera();
        }

        private void MoveTarget(Point position)
        {
            if (Imgui_Helper.IsAnyCaptured()) return;
            float speed = GetSpeed();

            var dx = position.X - StartPoint.X;
            var dy = position.Y - StartPoint.Y;
            Target += UP * -dy * speed;
            Target += Right * dx * speed;

            UpdateCamera();
            StartPoint = position;
        }
    }
}