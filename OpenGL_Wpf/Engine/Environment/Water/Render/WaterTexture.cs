using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.Texture;
using Simple_Engine.Engine.Space.Scene;
using System.Linq;
using static Simple_Engine.Engine.Water.Render.FBO;

namespace Simple_Engine.Engine.Water.Render
{
    internal class WaterTexture : Base_Texture
    {
        public float WaveSpeed { get; set; } = .005f;
        public float moveFactor { get; set; } = 0f;

        public WaterTexture(TextureMode textureTargetType) : base(textureTargetType)
        {
            AddReflection(TextureUnit.Texture0, new Vector4(0, 1, 0, 0f));
            AddRefraction(TextureUnit.Texture1, new Vector4(0, -1, 0, 0f));
            TextureIds.Add(new TextureSample2D(@"./Engine/Environment/Water/Render/Source/WaterNoise.png", TextureUnit.Texture2));
            TextureIds.Add(new TextureSample2D(@"./Engine/Environment/Water/Render/Source/WaterNormalMap.png", TextureUnit.Texture3));
            AddDepthTexture(TextureUnit.Texture4);
            // Set_LoadNormalMap(true);
        }

        public override void Dispose()
        {
            
            var fbo = SceneModel.ActiveScene.FBOs.First(o => o.Name == FboName.WorldReflection);
            fbo.CleanUp();

            fbo = SceneModel.ActiveScene.FBOs.First(o => o.Name == FboName.WorldRefraction);
            fbo.CleanUp();

            base.Dispose();
        }

        public void AddReflection(TextureUnit textureUnit, Vector4 clipPlan)
        {
            var textureModel = new TextureSample2D(textureUnit);
            var fbo = new Water_FBOReflection(Game.Instance.Width, Game.Instance.Height);
            fbo.ClipPlan = clipPlan;
            textureModel.TextureId = fbo.TextureId;
            SceneModel.ActiveScene.FBOs.Add(fbo);
            TextureIds.Add(textureModel);
        }

        public void AddRefraction(TextureUnit textureUnit, Vector4 clipPlan)
        {
            var textureModel = new TextureSample2D(textureUnit);
            var fbo = new Water_FBORefraction(Game.Instance.Width, Game.Instance.Height);
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
            moveFactor += WaveSpeed * (float)Game.Instance.RenderPeriod;
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