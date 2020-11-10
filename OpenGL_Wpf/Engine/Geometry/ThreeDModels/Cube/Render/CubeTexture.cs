using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using Simple_Engine.Engine.Render.Texture;

namespace Simple_Engine.Engine.Geometry.ThreeDModels.Cube.Render
{
    public class CubeTexture : Base_Texture
    {
        public CubeTexture(TextureMode textureTargetType) : base(textureTargetType)
        {
            var diffTexture = new TextureCube(@"./samplemodels/crate/crateTexture.png", TextureUnit.Texture0);
            var normalTexture = new TextureCube(@"./samplemodels/crate/crateNormalMap.png", TextureUnit.Texture1);
            TextureIds.Add(diffTexture);
            TextureIds.Add(normalTexture);
            Set_LoadNormalMap(true);
        }

        public override void UploadDefaults(Base_Shader ShaderModel)
        {
            base.UploadDefaults(ShaderModel);
            ShaderModel.SetInt(ShaderModel.Location_CubeDiffuseMap, 0);
            ShaderModel.SetInt(ShaderModel.Location_CubeNormalMap, 1);
        }
    }
}