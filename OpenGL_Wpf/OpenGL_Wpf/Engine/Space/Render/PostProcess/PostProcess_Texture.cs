using InSitU.Views.ThreeD.Engine.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Space.Render.PostProcess
{
    internal class PostProcess_Texture : Base_Texture
    {
        public PostProcess_Texture(TextureMode textureTargetType) : base(textureTargetType)
        {
            Setup_2DTexture("", OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
        }
    }
}