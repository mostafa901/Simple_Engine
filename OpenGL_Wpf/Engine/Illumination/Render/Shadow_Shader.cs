using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;

namespace Simple_Engine.Engine.Illumination.Render
{
    public class Shadow_Shader : Vertex_Shader
    {
        public Shadow_Shader() : base(ShaderPath.Shadow)
        {
        }

        public override void BindVertexAttributes()
        {
            BindAttribute(PositionLayoutId, "aPosition");
            BindAttribute(PositionLayoutId, "aTextureCoor");
            BindAttribute(MatrixLayoutId, "InstanceMatrix");
        }

        public override void UploadDefaults(Base_Geo model)
        {
            Use();

            model?.UploadDefaults(this);

            Stop();
        }
    }
}