using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using Simple_Engine.Engine.Render.Texture;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Illumination.Render
{
    internal class SkyBoxTexture : Base_Texture
    {
        public SkyBoxTexture() : base(TextureMode.TextureCube)
        {
            List<string> textures = new List<string>();
            textures.Add(@"./SampleModels/LandScape/Texture/Right.png");
            textures.Add(@"./SampleModels/LandScape/Texture/Left.png");
            textures.Add(@"./SampleModels/LandScape/Texture/Top.png");
            textures.Add(@"./SampleModels/LandScape/Texture/Bottom.png");
            textures.Add(@"./SampleModels/LandScape/Texture/Back.png");
            textures.Add(@"./SampleModels/LandScape/Texture/Front.png");

            var dayTextureModel = new TextureCube(textures, TextureUnit.Texture1);

            textures.Clear();
            textures.Add(@"./SampleModels/LandScape/Texture/nightRight.png");
            textures.Add(@"./SampleModels/LandScape/Texture/nightLeft.png");
            textures.Add(@"./SampleModels/LandScape/Texture/nightTop.png");
            textures.Add(@"./SampleModels/LandScape/Texture/nightBottom.png");
            textures.Add(@"./SampleModels/LandScape/Texture/nightBack.png");
            textures.Add(@"./SampleModels/LandScape/Texture/nightFront.png");

            var nightTextureModel = new TextureCube(textures, TextureUnit.Texture2);

            TextureIds.Add(dayTextureModel);
            TextureIds.Add(nightTextureModel);
        }

        public override void UploadDefaults(Base_Shader shaderModel)
        {
            base.UploadDefaults(shaderModel);
            shaderModel.SetInt(shaderModel.dayTexureLocation, 1);
            shaderModel.SetInt(shaderModel.nightTexureLocation, 2);
        }
    }
}