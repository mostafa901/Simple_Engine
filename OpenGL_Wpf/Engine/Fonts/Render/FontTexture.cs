using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;

namespace Simple_Engine.Engine.Fonts
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