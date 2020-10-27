using Simple_Engine.Views.ThreeD.Engine.Core.Abstracts;
using Simple_Engine.Views.ThreeD.Engine.Render;
using Simple_Engine.Views.ThreeD.Engine.Render.Texture;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Illumination.Render
{
    internal class SkyBoxTexture : Base_Texture
    {
        public SkyBoxTexture() : base(TextureMode.TextureCube)
        {
            List<string> textures = new List<string>();
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/Right.png");
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/Left.png");
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/Top.png");
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/Bottom.png");
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/Back.png");
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/Front.png");

            var dayTextureModel = new TextureCube(textures, TextureUnit.Texture1);

            textures.Clear();
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/nightRight.png");
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/nightLeft.png");
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/nightTop.png");
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/nightBottom.png");
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/nightBack.png");
            textures.Add(@"./Views/ThreeD/SampleModels/LandScape/Texture/nightFront.png");

            var nightTextureModel = new TextureCube(textures, TextureUnit.Texture2);

            TextureIds.Add(dayTextureModel);
            TextureIds.Add(nightTextureModel);
        }

        public override void UploadDefaults(Shader shaderModel)
        {
            base.UploadDefaults(shaderModel);
            shaderModel.SetInt(shaderModel.dayTexureLocation, 1);
            shaderModel.SetInt(shaderModel.nightTexureLocation, 2);
        }
    }
}