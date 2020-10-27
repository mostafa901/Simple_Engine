using Simple_Engine.Engine.Core;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Geometry.InputControls;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space;
using Simple_Engine.ToolBox;
using Newtonsoft.Json;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Simple_Engine.Engine.Illumination
{
    public class Light : IRenderable
    {
        public bool CastShadow = false;
        public CameraModel lightCamera;
        public bool Active = true;

        public Light(Light light)
        {
            Attenuation = light.Attenuation;
            LightColor = light.LightColor;
            LightPosition = light.LightPosition;
            Game.Context.ActiveScene.Lights.Add(this);
        }

        public Light()
        {
            Game.Context.ActiveScene.Lights.Add(this);
            Game.Context.Load += Game_Load;
        }

        private void Game_Load(object sender, EventArgs e)
        {
            Create_UIControls();
        }

        public Vector3 Attenuation { get; set; } = new Vector3(1, 0, 0);
        public IRenderable.BoundingBox BBX { get; set; }
        public int Id { get; set; }
        public Vector4 LightColor { get; set; } = new Vector4(.5f, .5f, .5f, 1);
        public Vector3 LightPosition { get; set; }
        public string Name { get; set; }
        
        public ImgUI_Controls Ui_Controls { get; set; }
        public int ShadowMapId { get; internal set; }
        public AnimationComponent Animate { get; set; }

        private Line LightRay;

        public void BuildModel()
        {
        }

        public void Create_UIControls()
        {
            Ui_Controls = new Imgui_Window( "Light");
            new Imgui_CheckBox(Ui_Controls, "CastShadow", ()=>CastShadow, (x) =>
               {
                   CastShadow = !CastShadow;
               });

            new Imgui_Color(Ui_Controls, "Color", LightColor, (x) =>
            {
                LightColor = x;
            });

            new Imgui_DragFloat(Ui_Controls, "Intensity", ()=>1, (x) =>
            {
                LightColor += new Vector4(x, x, x, 0);
            });

            new Imgui_CheckBox(Ui_Controls, "Show Light Ray",()=> false, (x) =>
            {
                if (LightRay == null)
                {
                    LightRay = new Line(new Vector3(), new Vector3(1));
                    LightRay.IsSystemModel = true;
                    LightRay.IsActive = false;
                    LightRay.BuildModel();
                    Game.Context.ActiveScene.UpLoadModels(LightRay);
                }

                {
                    LightRay.IsActive = !LightRay.IsActive;
                    if (LightRay.IsActive)
                    {
                        UpdateLightRay();
                    }
                }
            });

            var imgui_pos = new Imgui_DragFloat3(Ui_Controls, "Position", ()=>LightPosition, (x) =>
            {
                LightPosition += x;
                SetShadowTransform();
                UpdateLightRay();
            });
            imgui_pos.Width = 200;
        }

        private void UpdateLightRay()
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
            CastShadow = true;
            if (lightCamera == null)
            {
                lightCamera = new CameraModel(Game.Context.ActiveScene,false);
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
            ShaderModel.SetArray3(ShaderModel.LightPositionsLocation, Game.Context.ActiveScene.Lights.Select(o => o.LightPosition), new Vector3());
            ShaderModel.SetArray3(ShaderModel.AttenuationLightLocation, Game.Context.ActiveScene.Lights.Select(o => o.Attenuation), new Vector3(1, 0, 0));
        }

        private static void UpLoadLightColor(Shader ShaderModel)
        {
            ShaderModel.SetArray4(ShaderModel.LightColorLocation, Game.Context.ActiveScene.Lights.Select(o => o.LightColor), new Vector4());
        }
    }
}