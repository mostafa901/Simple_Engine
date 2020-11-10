using OpenTK;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;

namespace Simple_Engine.Engine.Core.Interfaces
{
    public interface IRenderable
    {
        public BoundingBox BBX { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool CastShadow { get; set; }

        public Vector4 DefaultColor { get; set; }

        void BuildModel();

        void Dispose();

        void Live_Update(Base_Shader ShaderModel);

        IRenderable Load(string path);

        void PostRender(Base_Shader ShaderModel);

        public abstract void PrepareForRender(Base_Shader shaderModel);

        public void Render_UIControls();

        void UploadVAO();

        string Save();

        void UpdateBoundingBox();

        void UploadDefaults(Base_Shader ShaderModel);

        public struct BoundingBox
        {
            public Vector3 Max { get; set; }
            public Vector3 Min { get; set; }

            public Vector3 GetCG()
            {
                var cg = (Max + Min) / 2;
                return cg;
            }

            public Vector3 GetDimensions()
            {
                return (Max - Min);
            }
        }
    }
}