using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;

namespace Simple_Engine.Engine.Particles.Render
{
    public class ParticleTexture : Base_Texture
    {
        public ParticleTexture(string imgPath, TextureMode textureTargetType) : base(textureTargetType)
        {
            Setup_2DTexture(imgPath, TextureUnit.Texture0);
            numberOfRows = 4;
        }
    }
}