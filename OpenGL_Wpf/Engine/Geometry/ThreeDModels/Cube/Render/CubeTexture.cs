using Simple_Engine.Views.ThreeD.Engine.Core.Abstracts;
using Simple_Engine.Views.ThreeD.Engine.Render;
using Simple_Engine.Views.ThreeD.Engine.Render.Texture;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Geometry.ThreeDModels.Cube.Render
{
    public class CubeTexture : Base_Texture
    {
        public CubeTexture(TextureMode textureTargetType) : base(textureTargetType)
        {
            var diffTexture = new TextureCube(@"./Views/threed/samplemodels/crate/crateTexture.png", TextureUnit.Texture0);
            var normalTexture = new TextureCube(@"./Views/threed/samplemodels/crate/crateNormalMap.png", TextureUnit.Texture1);
            TextureIds.Add(diffTexture);
            TextureIds.Add(normalTexture);
            Set_LoadNormalMap(true);
        }

        public override void UploadDefaults(Shader ShaderModel)
        {
            base.UploadDefaults(ShaderModel);
            ShaderModel.SetInt(ShaderModel.Location_CubeDiffuseMap, 0);
            ShaderModel.SetInt(ShaderModel.Location_CubeNormalMap, 1);
        }
    }
}