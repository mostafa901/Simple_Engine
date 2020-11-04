using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;

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