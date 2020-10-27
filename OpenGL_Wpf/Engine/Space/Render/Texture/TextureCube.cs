using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Render.Texture
{
    public class TextureCube : Base_Texture
    {
        public TextureCube(string imgPath) : base(TextureMode.TextureCube)
        {
            Setup_CubeTexture(new List<string> { imgPath }, TextureUnit.Texture0);
        }

        public TextureCube(List<string> imgPaths, TextureUnit textureUnit) : base(TextureMode.TextureCube)
        {
            Setup_CubeTexture(imgPaths, textureUnit);
        }

        public TextureCube(string imgPath, TextureUnit textureUnit) : base(TextureMode.TextureCube)
        {
            Setup_CubeTexture(new List<string> { imgPath }, textureUnit);
        }
    }
}