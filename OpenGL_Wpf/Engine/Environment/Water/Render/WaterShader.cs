using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using Simple_Engine.Engine.Space.Camera;

namespace Simple_Engine.Engine.Water.Render
{
    internal class WaterShader : Vertex_Shader
    {
        public WaterShader(ShaderPath shaderType) : base(shaderType)
        {
        }

        public int DudvLocation;
        public int moveFactorLocation;

        public int CameraPositionLocation { get; private set; }
        public int NormalMapLocation { get; private set; }
        public int DepthMapLocation { get; private set; }
        public int ReflectionLocation { get; private set; }
        public int RefractionLocation { get; private set; }

        public override void LoadAllUniforms()
        {
            base.LoadAllUniforms();
            ReflectionLocation = GetLocation("Reflection");
            RefractionLocation = GetLocation("Refraction");
            DudvLocation = GetLocation("Dudv");
            moveFactorLocation = GetLocation("moveFactor");
            NormalMapLocation = GetLocation("NormalMap");
            DepthMapLocation = GetLocation("DepthMap");
        }

        public override void BindVertexAttributes()
        {
            BindAttribute(PositionLayoutId, "aPosition");
            BindAttribute(TextureLayoutId, "aTextureCoor");
            BindAttribute(NormalLayoutId, "aNormals");
        }

        public override void Live_Update()
        {
            base.Live_Update();
            SetVector3(CameraPositionLocation, CameraModel.ActiveCamera.Position);
        }
    }
}