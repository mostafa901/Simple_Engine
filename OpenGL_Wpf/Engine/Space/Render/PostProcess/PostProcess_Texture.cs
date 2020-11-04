using Simple_Engine.Engine.Core.Abstracts;

namespace Simple_Engine.Engine.Space.Render.PostProcess
{
    internal class PostProcess_Texture : Base_Texture
    {
        public PostProcess_Texture(TextureMode textureTargetType) : base(textureTargetType)
        {
            Setup_2DTexture("", OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
        }
    }
}