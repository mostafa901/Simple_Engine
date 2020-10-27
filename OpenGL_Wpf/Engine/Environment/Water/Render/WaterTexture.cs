using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Simple_Engine.Engine.Water.Render.FBO;
using Simple_Engine.Engine.Space.Scene;

namespace Simple_Engine.Engine.Water.Render
{
    internal class WaterTexture : Base_Texture
    {
        public float WaveSpeed { get; set; } = .005f;
        public float moveFactor { get; set; } = 0f;

        public WaterTexture(TextureMode textureTargetType) : base(textureTargetType)
        {
            AddReflection(TextureUnit.Texture0, new Vector4(0, 1, 0, 0));
            AddRefraction(TextureUnit.Texture1, new Vector4(0, -1, 0, 0));
            TextureIds.Add(new TextureSample2D(@"D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Water\Render\Source\WaterNoise.png", TextureUnit.Texture2));
            TextureIds.Add(new TextureSample2D(@"D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Water\Render\Source\WaterNormalMap.png", TextureUnit.Texture3));
            AddDepthTexture(TextureUnit.Texture4);
           // Set_LoadNormalMap(true);
        }

        public void AddReflection(TextureUnit textureUnit, Vector4 clipPlan)
        {
            var textureModel = new TextureSample2D(textureUnit);
            var fbo = new Water_FBOReflection(Game.Context.Width, Game.Context.Height);
            fbo.ClipPlan = clipPlan;
            textureModel.TextureId = fbo.TextureId;
            SceneModel.ActiveScene.FBOs.Add(fbo);
            TextureIds.Add(textureModel);
        }

        public void AddRefraction(TextureUnit textureUnit, Vector4 clipPlan)
        {
            var textureModel = new TextureSample2D(textureUnit);
            var fbo = new Water_FBORefraction(Game.Context.Width, Game.Context.Height);
            fbo.ClipPlan = clipPlan;
            textureModel.TextureId = fbo.TextureId;
            SceneModel.ActiveScene.FBOs.Add(fbo);
            TextureIds.Add(textureModel);
        }

        internal void AddDepthTexture(TextureUnit textureUnit)
        {
            var textureModel = new TextureSample2D(textureUnit);
            var fbo = SceneModel.ActiveScene.FBOs.Last();
            textureModel.TextureId = fbo.TextureDepthId;
            TextureIds.Add(textureModel);
        }

        public override void Live_Update(Shader shaderModel)
        {
            base.Live_Update(shaderModel);
            moveFactor += WaveSpeed * (float)Game.Context.RenderPeriod;
            moveFactor = moveFactor % 1;
            if (shaderModel is WaterShader shader)
            {
                shaderModel.SetFloat(shader.moveFactorLocation, moveFactor);
            }
        }

        public override void UploadDefaults(Shader ShaderModel)
        {
        
            if (ShaderModel is WaterShader shader)
            {
            
                ShaderModel.SetInt(shader.ReflectionLocation, 0);
                ShaderModel.SetInt(shader.RefractionLocation, 1);
                ShaderModel.SetInt(shader.DudvLocation, 2);
                ShaderModel.SetInt(shader.Location_NormalMap, 3);
                ShaderModel.SetInt(shader.DepthMapLocation, 4);
            }
        }
    }
}