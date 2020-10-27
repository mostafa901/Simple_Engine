using InSitU.Views.ThreeD.Engine.Core.Abstracts;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Fonts
{
    public class FontTexture : Base_Texture
    {
        public FontTexture(string imgPath, TextureMode textureTargetType) : base(textureTargetType)
        {
            Setup_2DTexture(imgPath, TextureUnit.Texture0);
            Set_Texture_Tiling(TextureWrapMode.ClampToEdge);
        }
    }
}