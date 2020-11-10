using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;

namespace Simple_Engine.Engine.GUI.Render
{
    public class GUIShader : Vertex_Shader
    {
        public GUIShader(ShaderPath shaderType) : base(shaderType)
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