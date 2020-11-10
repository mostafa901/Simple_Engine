using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;

namespace Simple_Engine.Engine.Fonts
{
    public class FontShader : Vertex_Shader
    {
        public FontShader(ShaderPath shaderType) : base(shaderType)
        {
            string path = @"./Engine/Fonts/Render/Source/";
            Setup_Shader($"{path}VertexShader_Font.vert", $"{path}FragmentShader_Font.frag");
        }

        public override void BindVertexAttributes()
        {
            BindAttribute(PositionLayoutId, "aPosition2");
            BindAttribute(TextureLayoutId, "aTextureCoor");
        }
    }
}