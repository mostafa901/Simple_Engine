using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using Simple_Engine.Engine.Render.Texture;

namespace Simple_Engine.Engine.Geometry.Terrain.Render
{
    public class TerrainTexture : Base_Texture
    {
        public TerrainTexture() : base(TextureMode.Blend)
        {
            var blendTexture = new TextureSample2D(@"./SampleModels/LandScape/Texture/blendMap.png", TextureUnit.Texture0);
            var grassFlowerTexture = new TextureSample2D(@"./SampleModels/LandScape/Texture/grassflowers.png", TextureUnit.Texture1);
            var roadTexture = new TextureSample2D(@"./SampleModels/LandScape/Texture/path.png", TextureUnit.Texture2);
            var dirtTexture = new TextureSample2D(@"./SampleModels/LandScape/Texture/mud.png", TextureUnit.Texture3);
            var grassTexture = new TextureSample2D(@"./SampleModels/LandScape/Texture/grassy2.png", TextureUnit.Texture4);

            TextureIds.Add(blendTexture);
            TextureIds.Add(grassFlowerTexture);
            TextureIds.Add(roadTexture);
            TextureIds.Add(dirtTexture);
            TextureIds.Add(grassTexture);
        }

        public override void UploadDefaults(Base_Shader shaderModel)
        {
            base.UploadDefaults(shaderModel);

            int GrassTextureLocation = shaderModel.GetLocation("GrassTexture");
            int RoadTextureLocation = shaderModel.GetLocation("RoadTexture");
            int DirtTexureLocation = shaderModel.GetLocation("DirtTexure");
            int GrassFlowerTextureLocation = shaderModel.GetLocation("GrassFlowerTexture");

            shaderModel.SetInt(GrassFlowerTextureLocation, 1);
            shaderModel.SetInt(RoadTextureLocation, 2);
            shaderModel.SetInt(DirtTexureLocation, 3);
            shaderModel.SetInt(GrassTextureLocation, 4);
        }
    }
}