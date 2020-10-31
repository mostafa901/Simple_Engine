using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Render;
using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Core.Interfaces
{
    public interface IRenderable
    {
        public AnimationComponent Animate { get; set; }

        public BoundingBox BBX { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool CastShadow { get; set; }

        public Vector4 DefaultColor { get; set; }

        void BuildModel();

    
        void Dispose();

        void Live_Update(Shader ShaderModel);

        IRenderable Load(string path);

        void PostRender(Shader ShaderModel);

        public abstract void PrepareForRender(Shader shaderModel);

        public void Render_UIControls();

        void RenderModel();

        string Save();

        void UpdateBoundingBox();

        void UploadDefaults(Shader ShaderModel);

        public struct BoundingBox
        {

            public Vector3 Max { get; set; }
            public Vector3 Min { get; set; }

            public Vector3 GetCG()
            {
                var cg = (Max + Min)/2;
                return cg;
            }

            public Vector3 GetDimensions()
            {
                return (Max - Min);
            }

        }
    }
}