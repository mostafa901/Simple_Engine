using Simple_Engine.Views.ThreeD.Engine.Core.Abstracts;
using Simple_Engine.Views.ThreeD.Engine.Geometry.Core;

using Simple_Engine.Views.ThreeD.Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using org.apache.poi.ss.formula.functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Geometry.TwoD
{
    public class Hilbert: Base_Geo2D
    {
        int Count;
        public Hilbert(int count)
        {
            Count = count;
            BuildModel();
            ShaderModel = new Shader( ShaderMapType.LoadColor, ShaderPath.Default);
        }

        public enum direction
        {
            Up, Down, Left, Right
        }

        private Vector2 DrawLine(direction direct, Vector2 point, float step)
        {
            switch (direct)
            {
                case direction.Up:
                    point += new Vector2(0, step);
                    break;

                case direction.Down:
                    point += new Vector2(0, -step);

                    break;

                case direction.Left:
                    point += new Vector2(-step, 0);

                    break;

                case direction.Right:
                    point += new Vector2(step, 0);

                    break;

                default:
                    break;
            }
            Positions.Add(point);
            return point;
        }

        private void startdraw(int count)
        {
            Vector2 start = new Vector2(-.99f, -.99f);
            Positions.Add(start);

            DrawHilbert(count, direction.Up, direction.Right, direction.Down, direction.Left, start);
        }

        private Vector2 DrawHilbert(int count, direction u, direction r, direction d, direction l, Vector2 start)
        {
            float step = .05f;
            if (count > 0)
            {
                count--;

                start = DrawHilbert(count, r, u, l, d, start);
                start = DrawLine(u, start, step);

                start = DrawHilbert(count, u, r, d, l, start);
                start = DrawLine(r, start, step);

                start = DrawHilbert(count, u, r, d, l, start);
                start = DrawLine(d, start, step);

                start = DrawHilbert(count, l, d, r, u, start);
            }
            return start;
        }

        public override void BuildModel()
        {
            DrawType = PrimitiveType.Triangles;

            startdraw(Count);
            Setup_TextureCoordinates();
            ShaderModel = new Shader(ShaderMapType.LoadColor, ShaderPath.Default);

            Setup_Indeces();
        }

        public override void Setup_TextureCoordinates(float xScale= 1, float yScale = 1)
        {
            for (int i = 0; i < Positions.Count; i += 4)
            {
                TextureCoordinates.Add(new Vector2(0, 0));
                TextureCoordinates.Add(new Vector2(0, 1*yScale));
                TextureCoordinates.Add(new Vector2(1*xScale, 1*yScale));
                TextureCoordinates.Add(new Vector2(1*xScale, 0));
            }
        }

        public override void Setup_Indeces()
        {
            for (int i = 0; i < Positions.Count; i++)
            {
                Indeces.Add(i);
            }
        }

        public override void Setup_Position()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Normals()
        {
            throw new NotImplementedException();
        }

        public override void Live_Update(Shader ShaderModel)
        {
            throw new NotImplementedException();
        }

        public override void RenderModel()
        {
            throw new NotImplementedException();
        }
    }
}