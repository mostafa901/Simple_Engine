using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Water.Render
{
    internal class WaterShader : Shader
    {
        public WaterShader(ShaderMapType mapType, ShaderPath shaderType) : base(mapType, shaderType)
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
            CameraPositionLocation = GetLocation("CameraPosition");
            NormalMapLocation = GetLocation("NormalMap");
            DepthMapLocation = GetLocation("DepthMap");
        }

        

        public override void BindAttributes()
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