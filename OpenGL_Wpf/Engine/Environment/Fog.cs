using OpenTK;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using System;

namespace Simple_Engine.Engine.Space.Environment
{
    public class Fog : IRenderable
    {
        public float Density { get; set; } //how strong the fog is, the more the more visibility decrease
        public float FogSpeed { get; set; } //how fast the color fades with fog
        public bool Active = false;

        public Vector4 FogColor { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public IRenderable.BoundingBox BBX { get; set; }
        public ImgUI_Controls Ui_Controls { get; set; }
        public Vector4 DefaultColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CastShadow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Fog()
        {
            Setup_Fog(.001f, 1.8f);
        }

        public Fog(float density, float speed)
        {
            Setup_Fog(density, speed);
        }

        private void Setup_Fog(float density, float speed)
        {
            Density = density;
            FogSpeed = speed;
            Active = false;
            SetFogColor(new Vector3(.7f));
        }

        public void SetFogColor(Vector3 fogColor)
        {
            FogColor = new Vector4(fogColor, 1);
        }

        public void PostRender(Base_Shader ShaderModel)
        {
            throw new NotImplementedException();
        }

        public void BuildModel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void UploadDefaults(Base_Shader ShaderModel)
        {
            ShaderModel.SetBool(ShaderModel.HasFogLocation, Active);

            ShaderModel.SetVector4(ShaderModel.FogColorLocation, FogColor);
            ShaderModel.SetFloat(ShaderModel.FogDensityLocation, Density);
            ShaderModel.SetFloat(ShaderModel.FogSpeedLocation, FogSpeed);
        }

        public void PrepareForRender(Base_Shader shaderModel)
        {
            throw new NotImplementedException();
        }

        public void Live_Update(Base_Shader ShaderModel)
        {
            ShaderModel.SetBool(ShaderModel.HasFogLocation, Active);
        }

        public void UploadVAO()
        {
            throw new NotImplementedException();
        }

        public string Save()
        {
            throw new NotImplementedException();
        }

        public IRenderable Load(string path)
        {
            throw new NotImplementedException();
        }

        public void Render_UIControls()
        {
            throw new NotImplementedException();
        }

        public void UpdateBoundingBox()
        {
            throw new NotImplementedException();
        }

        public void Create_UIControls()
        {
            throw new NotImplementedException();
        }
    }
}