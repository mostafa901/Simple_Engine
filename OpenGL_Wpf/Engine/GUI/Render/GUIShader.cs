using Simple_Engine.Engine.Render;

namespace Simple_Engine.Engine.GUI.Render
{
    public class GUIShader : Shader
    {
        public GUIShader(ShaderMapType mapType, ShaderPath shaderType) : base(mapType, shaderType)
        {
        }

        public override void BindVertexAttributes()
        {
            BindAttribute(PositionLayoutId, "aPosition2");
            BindAttribute(TextureLayoutId, "aTextureCoor");
            BindAttribute(NormalLayoutId, "aNormals");
        }
    }
}