using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Geometry.Render;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;

namespace Simple_Engine.Engine.Geometry.ThreeDModels
{
    public class Line3D : Base_Geo3D
    {
        public Line3D()
        {
        }

        public Line3D(Vector3 a, Vector3 b)
        {
            DrawType = PrimitiveType.Lines;
            SetShaderModel(new Vertex_Shader(ShaderPath.Color));

            StartPoint = a;
            EndPoint = b;
            BuildModel();
        }

        public Vector3 StartPoint { get; set; }
        public Vector3 EndPoint { get; set; }

        public override void BuildModel()
        {
            Setup_Position();
            Setup_Indeces();
            Setup_Normals();
            modelType = typeof(Line3D);
        }

        public override void Setup_Indeces()
        {
        }

        public override void Setup_Normals()
        {
            Normals.Clear();
            for (int i = 0; i < Positions.Count; i++)
            {
                Normals.Add(new Vector3(0, 1, 0));
            }
        }

        public override void Setup_Position()
        {
            Positions.Clear();
            Positions.Add(StartPoint);
            Positions.Add(EndPoint);
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
            TextureCoordinates.Clear();
            for (int i = 0; i < Positions.Count; i++)
            {
                TextureCoordinates.Add(new Vector2(0, 1));
            }
        }

        public override void Live_Update(Base_Shader ShaderModel)
        {
            base.Live_Update(ShaderModel);
        }

        public override void UploadVAO()
        {
            if (Renderer == null)
            {
                Renderer = new GeometryRenderer(this);
            }

            Renderer.RenderModel();
            GetShaderModel().UploadDefaults(this);
        }
    }
}