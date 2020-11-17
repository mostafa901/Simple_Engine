using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Shared_Lib.Extention.Serialize_Ex;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Core.Serialize;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.Fonts.Core;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.Particles.Render;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Environment;
using Simple_Engine.Engine.Static.InputControl;
using Simple_Engine.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple_Engine.Engine.Space.Scene
{
    public partial class SceneModel : IRenderable
    {
        public static SceneModel ActiveScene;

        //this will update all the shader models per Render
        public Stack<Action<Base_Shader>> RunOnAllShaders = new Stack<Action<Base_Shader>>();

        public SceneModel(Game mainGame)
        {
            game = mainGame;
            Lights = new List<LightModel>();
            BBX = new IRenderable.BoundingBox();

            Setup_RenderSetings();
            Setup_Events();
        }

        public IRenderable.BoundingBox BBX { get; set; }
        public bool CastShadow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector4 DefaultColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void BuildModel()
        {
            SceneFog = new Fog();
            SceneFog.SetFogColor(new Vector3(.5f, .5f, .5f));

            Setup_Camera();
            Setup_SceneLight();
            Setup_Grid();
            Setup_Font();
            SelectedShader = new Vertex_Shader(ShaderPath.SingleColor);
        }

        private void Setup_Font()
        {
            FontFactory.GenerateFont();
        }

        void IRenderable.Dispose()
        {
            throw new NotImplementedException();
        }

        public void Live_Update(Base_Shader ShaderModel)
        {
            ShaderModel.SetBool(ShaderModel.IsToonRenderLocation, IsToonMode);
            CameraModel.ActiveCamera.Live_Update(ShaderModel);
            SceneFog.Live_Update(ShaderModel);
            Lights.First().Live_Update(ShaderModel);
        }

        public void PostRender(Base_Shader ShaderModel)
        {
        }

        public void PrepareForRender(Base_Shader shaderModel)
        {
            if (ModelstoRemove.Any())
            {
                while (ModelstoRemove.Any())
                {
                    var model = ModelstoRemove.Pop();
                    geoModels.Remove(model);
                }
            }

            if (ModelsforUpload.Any())
            {
                var id = 0;
                if (geoModels.Any()) id = geoModels.Max(o => o.Id);
                int max = 10;
                if (ModelsforUpload.Count < 10)
                {
                    max = ModelsforUpload.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    var model = ModelsforUpload.Pop();

                    model.Id = ++id;

                    model.UploadVAO();

                    geoModels.Add(model);
                    if (!model.IsSystemModel)
                    {
                        model.UpdateBoundingBox();
                        UpdateBoundingBox();
                    }
                }
            }

            CameraModel.ActiveCamera.UploadVAO();
            KeyControl.Update_ActionKey();
            Render_UIControls();
        }

        public void UploadVAO()
        {
        }

        public string Save()
        {
            return this.JSerialize(JsonTools.GetSettings());
        }

        public void UpdateBoundingBox()
        {
            var model = geoModels.LastOrDefault();
            if (model != null)
            {
                var vecMax = new Vector3(model.BBX.Max.Max_X(BBX.Max), model.BBX.Max.Max_Y(BBX.Max), model.BBX.Max.Max_Z(BBX.Max));
                var vecMin = new Vector3(model.BBX.Min.Min_X(BBX.Min), model.BBX.Min.Min_Y(BBX.Min), model.BBX.Min.Min_Z(BBX.Min));
                BBX = new IRenderable.BoundingBox
                {
                    Max = vecMax,
                    Min = vecMin,
                };
            }
        }

        public void UploadDefaults(Base_Shader ShaderModel)
        {
            CameraModel.ActiveCamera.UploadDefaults(ShaderModel);
            SceneFog.UploadDefaults(ShaderModel);
            UploadLightsDefaults(ShaderModel);
        }

        public virtual void UploadLightsDefaults(Base_Shader ShaderModel)
        {
            foreach (var light in Lights)
            {
                light.UploadDefaults(ShaderModel);
            }
        }

        internal void ActivateModels()
        {
            foreach (var model in geoModels)
            {
                model.IsActive = true;
            }
        }

        internal void Dispose()
        {
            foreach (IDrawable geo in geoModels)
            {
                RemoveModels(geo);
            }
            for (int i = FBOs.Count - 1; i >= 0; i--)
            {
                var fbo = FBOs[i];
                fbo.CleanUp();
            }
        }

        internal void DisposeModels(bool disposeSystemModels = false)
        {
            foreach (var model in geoModels)
            {
                if (model.IsSystemModel && !disposeSystemModels) continue;

                ModelstoRemove.Push(model);
            }
        }

        internal void IsolateModel(IDrawable xmodel)
        {
            foreach (var model in geoModels)
            {
                model.IsActive = false;
            }
            xmodel.IsActive = true;
        }

        internal void RemoveModels(IDrawable model)
        {
            if (model == null) return;

            ModelstoRemove.Push(model);
        }

        internal void Render()
        {
            UploadVAO();
            for (int i = 0; i < geoModels.Count; i++)
            {
                var model = geoModels.ElementAt(i);

                if (model.IsActive)
                {
                    model.PrepareForRender(model.GetShaderModel());
                    foreach (var action in RunOnAllShaders)
                    {
                        action(model.GetShaderModel());
                    }
                    model.Renderer.Draw();

                    model.GetShaderModel().Stop();

                    if (model.Particles != null)
                    {
                        foreach (var particle in model.Particles)
                        {
                            ParticleSystem.Draw(this, model);
                        }
                    }
                }
            }
            RunOnAllShaders.Clear();
        }

        internal void UpLoadModels(IDrawable model)
        {
            model.Id = geoModels.Count;
            ModelsforUpload.Push(model);
        }

        private void Game_Load(object sender, EventArgs e)
        {
        }

        private void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (UI_Shared.IsAnyCaptured()) return;

            if (e.IsPressed && e.Button == MouseButton.Left)
            {
                var model = CameraModel.ActiveCamera.PickObject(e.Position);
                if (model == null)
                {
                    Base_Geo.SelectedModel?.Set_Selected(false);
                }
                else
                {
                    model?.Set_Selected(true);
                }

                return;
            }
        }

        private void Setup_Camera()
        {
            CameraModel.PerspectiveCamera = new CameraModel(this, CameraModel.CameraType.Perspective);
            CameraModel.PerspectiveCamera.UpdateCamera();

            CameraModel.PlanCamera = new CameraModel(this, CameraModel.CameraType.Plan);
            CameraModel.PlanCamera.Position = new Vector3(0, 10, 0);
            CameraModel.PlanCamera.UpdateCamera();

            CameraModel.ActiveCamera = CameraModel.Create(this, CameraModel.CameraType.Perspective);
            CameraModel.ActiveCamera.AnimateCameraPosition(new Vector3(-2, 1f, -2), 2000);
            CameraModel.ActiveCamera.AnimateCameraTarget(new Vector3(0, 1f, 0), 2000);
        }

        private void Setup_Events()
        {
            game.Load += Game_Load;
            game.MouseDown += Game_MouseDown;
        }

        private void Setup_Grid()
        {
            //   GameFactory.DrawGrid(this);
        }

        private void Setup_RenderSetings()
        {
            GL.Enable(EnableCap.DepthTest); //requires ClearBufferMask.DepthBufferBit @ render frame
            GL.Enable(EnableCap.CullFace); //avoid rendering Faces that are.
            GL.CullFace(CullFaceMode.Back); //back from Camera
            GL.ShadeModel(ShadingModel.Flat);
            GL.LineWidth(DisplayManager.LineSize); //greater than 1 will cause error in future opengl versions
            GL.PointSize(DisplayManager.PointSize);
            //GL.Enable(EnableCap.ScissorTest);
            //GL.Scissor(200, 200, 600, 600);
        }

        private void Setup_SceneLight()
        {
            var sunlight = new LightModel();
            sunlight.DefaultColor = new Vector4(1.3f);
            sunlight.LightPosition = new Vector3(-10000, 10000, -10000);
            LightModel.SelectedLight = sunlight;
        }
    }
}