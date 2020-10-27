using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.GUI.Render
{
    public class GUITexture : Base_Texture
    {
        public GUITexture(string imgPath, TextureMode textureTargetType) : base(textureTargetType)
        {
            Setup_2DTexture(imgPath, TextureUnit.Texture0);
        }
    }
}