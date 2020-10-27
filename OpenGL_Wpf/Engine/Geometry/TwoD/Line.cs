using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Geometry.Render;
using Simple_Engine.Engine.GUI.Render;
using Simple_Engine.Engine.Primitives;
using Simple_Engine.Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Geometry
{
    public class Line : Base_Geo3D
    {
        public Line()
        {
        
        }

        public Line(Vector3 a, Vector3 b)
        {
            DrawType = PrimitiveType.Lines;
            ShaderModel = new Shader(ShaderMapType.Blend, ShaderPath.Color);

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

        public override void Live_Update(Shader ShaderModel)
        {
            base.Live_Update(ShaderModel);
        }

        public override void RenderModel()
        {
            if (Renderer == null)
            {
                Renderer = new GeometryRenderer(this);
            }

            Renderer.RenderModel();
            ShaderModel.UploadDefaults(this);
        }
    }
}