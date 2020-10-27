using InSitU.Views.ThreeD.Engine.Core.Abstracts;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.Engine.Render.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Geometry.Terrain.Render
{
    public class TerrainTexture : Base_Texture
    {
        public TerrainTexture() : base(TextureMode.Blend)
        {
            var blendTexture = new TextureSample2D(@"./Views/ThreeD/SampleModels/LandScape/Texture/blendMap.png", TextureUnit.Texture0);
            var grassFlowerTexture = new TextureSample2D(@"./Views/ThreeD/SampleModels/LandScape/Texture/grassflowers.png", TextureUnit.Texture1);
            var roadTexture = new TextureSample2D(@"./Views/ThreeD/SampleModels/LandScape/Texture/path.png", TextureUnit.Texture2);
            var dirtTexture = new TextureSample2D(@"./Views/ThreeD/SampleModels/LandScape/Texture/mud.png", TextureUnit.Texture3);
            var grassTexture = new TextureSample2D(@"./Views/ThreeD/SampleModels/LandScape/Texture/grassy2.png", TextureUnit.Texture4);

            TextureIds.Add(blendTexture);
            TextureIds.Add(grassFlowerTexture);
            TextureIds.Add(roadTexture);
            TextureIds.Add(dirtTexture);
            TextureIds.Add(grassTexture);
        }

        public override void UploadDefaults(Shader shaderModel)
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