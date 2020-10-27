using InSitU.Views.ThreeD.Engine.Core;
using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.ImGui_Set.Controls;
using InSitU.Views.ThreeD.Engine.Render;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Space.Environment
{
    public class Fog : IRenderable
    {
        public float Density { get; set; } //how strong the fog is, the more the more visibility decrease
        public float FogSpeed { get; set; } //how fast the color fades with fog
        public bool Active { get; set; } = false;

        public Vector4 FogColor { get; set; }
        public string Name { get ; set ; }
        public int Id { get ; set ; }
        public IRenderable.BoundingBox BBX { get ; set ; }
        public ImgUI_Controls Ui_Controls { get ; set ; }
        public AnimationComponent Animate { get; set; }

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
            Active = true;
            SetFogColor(new Vector3(.7f));
        }

        public void SetFogColor(Vector3 fogColor)
        {
            FogColor = new Vector4(fogColor, 1);
        }

        private float Direction = 1;

        public void AnimateDensity(float speed)
        {
            if (FogSpeed >= 5f) Direction = -1;
            if (FogSpeed <= 0f) Direction = 1;

            speed *= Direction;

            FogSpeed += (.001f * speed);
        }

        public void PostRender(Shader ShaderModel)
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

        public void UploadDefaults(Shader ShaderModel)
        {
            ShaderModel.SetBool(ShaderModel.HasFogLocation, Active);

            if (Active)
            {
                AnimateDensity(2);
                ShaderModel.SetVector4(ShaderModel.FogColorLocation, FogColor);
                ShaderModel.SetFloat(ShaderModel.FogDensityLocation, Density);
                ShaderModel.SetFloat(ShaderModel.FogSpeedLocation, FogSpeed);
            }
        }

        public void PrepareForRender(Shader shaderModel)
        {
            throw new NotImplementedException();
        }

        public void Live_Update(Shader ShaderModel)
        {
        }

        public void RenderModel()
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