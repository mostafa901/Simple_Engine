using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;

namespace Simple_Engine.Engine.Geometry.ThreeDModels.Cube.Render
{
    public class CubeShader : Vertex_Shader
    {
        public CubeShader() : base(ShaderPath.Cube)
        {
        }

        public override void Live_Update()
        {
            base.Live_Update();
            SetInt(Location_ShaderType, (int)ShaderType);
        }
    }
}