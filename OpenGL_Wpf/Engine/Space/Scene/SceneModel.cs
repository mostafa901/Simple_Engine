using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Shared_Lib.Extention.Serialize_Ex;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Core.Serialize;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.ImGui_Set;
using Simple_Engine.Engine.Particles.Render;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Environment;
using Simple_Engine.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple_Engine.Engine.Space.Scene
{
    public partial class SceneModel : IRenderable
    {
        public static SceneModel ActiveScene;

        public SceneModel(Game mainGame)
        {
            game = mainGame;
            Lights = new List<LightModel>();
            BBX = new IRenderable.BoundingBox();

            Setup_RenderSetings();
            Setup_Events();
        }

        public IRenderable.BoundingBox BBX { get; set; }
        public Vector4 DefaultColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CastShadow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void BuildModel()
        {
            SceneFog = new Fog();
            SceneFog.SetFogColor(new Vector3(.5f, .5f, .5f));
            SceneFog.Active = false;

            Setup_Camera();
            Setup_SceneLight();

            SelectedShader = new Shader(ShaderMapType.Blend, ShaderPath.SingleColor);
        }

        void IRenderable.Dispose()
        {
            throw new NotImplementedException();
        }

        public void Live_Update(Shader ShaderModel)
        {
            ShaderModel.SetBool(ShaderModel.IsToonRenderLocation, IsToonMode);
            CameraModel.ActiveCamera.Live_Update(ShaderModel);
            SceneFog.Live_Update(ShaderModel);
            Lights.First().Live_Update(ShaderModel);
        }

        public void PostRender(Shader ShaderModel)
        {
        }

        public void PrepareForRender(Shader shaderModel)
        {
            if (ModelstoRemove.Any())
            {
                while (ModelstoRemove.Any())
                {
                    var model = ModelstoRemove.Pop();
                    geoModels.Remove(model);
                    model.Dispose();
                }
            }

            if (!ModelsforUpload.Any()) return;

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

                model.RenderModel();

                geoModels.Add(model);
                model.UpdateBoundingBox();
                UpdateBoundingBox();
            }
        }

        public void RenderModel()
        {
            Render_UIControls();
            CameraModel.ActiveCamera.Animate.Update();
            CameraModel.ActiveCamera.RenderModel();
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
                var trans = model.LocalTransform.ExtractTranslation();
                BBX = new IRenderable.BoundingBox
                {
                    Max = new Vector3(model.BBX.Max.Max_X(BBX.Max), model.BBX.Max.Max_Y(BBX.Max), model.BBX.Max.Max_Z(BBX.Max)),
                    Min = new Vector3(model.BBX.Min.Min_X(BBX.Min), model.BBX.Min.Min_Y(BBX.Min), model.BBX.Min.Min_Z(BBX.Min)),
                };
            }
        }

        public void UploadDefaults(Shader ShaderModel)
        {
            CameraModel.ActiveCamera.UploadDefaults(ShaderModel);
            SceneFog.UploadDefaults(ShaderModel);
            UploadLightsDefaults(ShaderModel);
        }

        public virtual void UploadLightsDefaults(Shader ShaderModel)
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
            foreach (var fbo in FBOs)
            {
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
            ModelstoRemove.Push(model);
        }

        internal void Render()
        {
            RenderModel();
            for (int i = 0; i < geoModels.Count; i++)
            {
                var model = geoModels.ElementAt(i);

                if (model.IsActive)
                {
                    model.PrepareForRender(model.ShaderModel);

                    model.Renderer.Draw();

                    model.ShaderModel.Stop();

                    if (model.Particles != null)
                    {
                        foreach (var particle in model.Particles)
                        {
                            ParticleSystem.Draw(this, model);
                        }
                    }
                }
            }

            Core.Static.UI_Geo.RenderUI(Base_Geo.SelectedModel);
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
            var ActiveCamera = new CameraModel(this, true);
            ActiveCamera.Position = new Vector3(-2, 0, -2);
            ActiveCamera.Target = new Vector3(0, 0, 0);
            ActiveCamera.Setup_Events();
            ActiveCamera.UpdateCamera();

            CameraModel.ActiveCamera = ActiveCamera;
        }

        private void Setup_Events()
        {
            game.Load += Game_Load;
            game.MouseDown += Game_MouseDown;
        }

        private void Setup_RenderSetings()
        {
            GL.Enable(EnableCap.DepthTest); //requires ClearBufferMask.DepthBufferBit @ render frame
            GL.Enable(EnableCap.CullFace); //avoid rendering Faces that are.
            GL.CullFace(CullFaceMode.Back); //back from Camera
            GL.ShadeModel(ShadingModel.Flat);
            GL.LineWidth(1f); //greater than 1 will cause error in future opengl versions

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