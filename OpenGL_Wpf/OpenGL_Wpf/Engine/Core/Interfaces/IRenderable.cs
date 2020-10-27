using InSitU.Views.ThreeD.Engine.ImGui_Set.Controls;
using InSitU.Views.ThreeD.Engine.Render;
using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Core.Interfaces
{
    public interface IRenderable
    {
        public AnimationComponent Animate { get; set; }

        public BoundingBox BBX { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ImgUI_Controls Ui_Controls { get; set; }

        void BuildModel();

        void Create_UIControls();

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
            public float Depth;
            public float Height;
            public float Width;

            public Vector3 CG { get; set; }
            public Vector3 Max { get; set; }
            public Vector3 Min { get; set; }
        }
    }
}