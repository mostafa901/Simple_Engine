using Simple_Engine.Engine.Render;

namespace Simple_Engine.Engine.Fonts
{
    public class FontShader : Shader
    {
        public FontShader(ShaderMapType mapType, ShaderPath shaderType) : base(mapType, shaderType)
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