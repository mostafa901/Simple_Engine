using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Render;
using OpenTK;
using System;
using System.Linq;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Engine.GameSystem;

namespace Simple_Engine.Engine.Illumination
{
    public class LightModel : IRenderable
    {
        
        public CameraModel lightCamera;
        public bool Active = true;
        public static LightModel SelectedLight;
        public LightModel(LightModel light)
        {
            Attenuation = light.Attenuation;
            DefaultColor = light.DefaultColor;
            LightPosition = light.LightPosition;
            SceneModel.ActiveScene.Lights.Add(this);
        }

        public LightModel()
        {
            SceneModel.ActiveScene.Lights.Add(this);
            Game.Instance.Load += Game_Load;
        }

        private void Game_Load(object sender, EventArgs e)
        {
           
        }

        public Vector3 Attenuation { get; set; } = new Vector3(1, 0, 0);
        public IRenderable.BoundingBox BBX { get; set; }

        

        public int Id { get; set; }
        public Vector4 DefaultColor { get; set; } = new Vector4(.5f, .5f, .5f, 1);
        public Vector3 LightPosition { get; set; }
        public string Name { get; set; }
        
        public ImgUI_Controls Ui_Controls { get; set; }
        public int ShadowMapId { get; internal set; }
        public AnimationComponent Animate { get; set; }
        public float Intensity { get; set; } = 1;
       public bool CastShadow { get ; set; }

        public Line LightRay;

        public void BuildModel()
        {
        }

        public void Create_UIControls()
        {
            
        }

        public void UpdateLightRay()
        {
            if (LightRay == null) return;

            LightRay.StartPoint = LightPosition;
            LightRay.EndPoint = new Vector3();
            LightRay.BuildModel();

            LightRay.RenderModel();
        }

        public void Dispose()
        {
        }

        public Vector3 GetEyeSpacePosition(Matrix4 viewMatrix)
        {
            Vector4 eyeSpacePos = new Vector4(LightPosition.X, LightPosition.Y, LightPosition.Z, 1f);
            eyeSpacePos = viewMatrix * eyeSpacePos;

            return new Vector3(eyeSpacePos);
        }

        public void Live_Update(Shader ShaderModel)
        {
            if (CastShadow)
            {
                ShaderModel.SetMatrix4(ShaderModel.Location_lightProjection, lightCamera.ProjectionTransform);
                ShaderModel.SetMatrix4(ShaderModel.Location_lightViewTransform, lightCamera.ViewTransform);
            }
            UpLoadLightColor(ShaderModel);
        }

        public IRenderable Load(string path)
        {
            throw new NotImplementedException();
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
            Ui_Controls.BuildModel();
        }

        public void RenderModel()
        {
            throw new NotImplementedException();
        }

        public string Save()
        {
            throw new NotImplementedException();
        }

        public void SetShadowTransform()
        {
             
            if (lightCamera == null)
            {
                lightCamera = new CameraModel(SceneModel.ActiveScene,false);
                lightCamera.Target = new Vector3(50, 0, 50);

                lightCamera.Activate_Ortho();
            }

            lightCamera.Position = LightPosition;
            lightCamera.UpdateCamera();
        }

        public void UpdateBoundingBox()
        {
            throw new NotImplementedException();
        }

        public void UploadDefaults(Shader ShaderModel)
        {
            ShaderModel.SetInt(ShaderModel.MaxLightLocation, Shader.MaximumLight);
            UpLoadLightColor(ShaderModel);
            ShaderModel.SetArray3(ShaderModel.LightPositionsLocation, SceneModel.ActiveScene.Lights.Select(o => o.LightPosition), new Vector3());
            ShaderModel.SetArray3(ShaderModel.AttenuationLightLocation, SceneModel.ActiveScene.Lights.Select(o => o.Attenuation), new Vector3(1, 0, 0));
        }

        private static void UpLoadLightColor(Shader ShaderModel)
        {
            ShaderModel.SetArray4(ShaderModel.LightColorLocation, SceneModel.ActiveScene.Lights.Select(o => o.DefaultColor), new Vector4());
        }
    }
}