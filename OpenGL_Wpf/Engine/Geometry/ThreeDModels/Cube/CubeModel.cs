using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.ThreeDModels.Cube.Render;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Geometry.Cube
{
    public class CubeModel : Base_Geo3D, ISelectable
    {
        public bool Selected { get; set; }

        public CubeModel()
        {
        }

        public CubeModel(float width)
        {
            SetHeight(width);
            SetWidth(width);
            SetDepth(width);
        }

        public override float GetWidth()
        {
            return base.GetWidth() * 2;
        }

        public override float GetHeight()
        {
            return base.GetWidth() * 2;
        }

        public override float GetDepth()
        {
            return base.GetWidth() * 2;
        }

        public override void BuildModel()
        {
            DrawType = PrimitiveType.Triangles;
            Setup_Position();
            Setup_Indeces();
            Setup_TextureCoordinates();
            Setup_Normals();
            //PivotPoint = new Vector3(0, -GetWidth() / 2, 0);
        }

        public void NegateModel()
        {
            Vector3 scalarVector = new Vector3(1, -1, 1);

            if (GetShaderModel().EnableInstancing)
            {
                foreach (var mesh in Meshes)
                {
                    var pos = mesh.LocalTransform.ExtractTranslation() * scalarVector;
                    mesh.MoveTo(pos);
                    mesh.Scale(scalarVector);
                }
            }
            else
            {
                var pos = LocalTransform.ExtractTranslation() * -scalarVector;
                MoveTo(pos);
                Scale(scalarVector);
            }
        }

        public override void Setup_Normals()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Normals.Add(new Vector3(1, 0, 0));
                }
            }
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
            for (int i = 0; i < 6; i++)
            {
                TextureCoordinates.Add(new Vector2());
                TextureCoordinates.Add(new Vector2(0, 1 * yScale));
                TextureCoordinates.Add(new Vector2(1 * xScale, 1 * yScale));
                TextureCoordinates.Add(new Vector2(1 * xScale, 0));
            }
        }

        public override void Setup_Indeces()
        {
            var indecies = new List<int>();

            //bottom
            indecies.AddRange(new int[] { 3, 2, 1, 1, 2, 0 });

            //Top
            indecies.AddRange(new int[] { 4, 6, 5, 5, 6, 7 });

            //Right
            indecies.AddRange(new int[] { 4, 5, 0, 0, 5, 1 });

            //Left
            indecies.AddRange(new int[] { 6, 2, 7, 7, 2, 3 });

            //Front
            indecies.AddRange(new int[] { 7, 3, 5, 5, 3, 1 });

            //Back
            indecies.AddRange(new int[] { 6, 4, 2, 2, 4, 0 });

            Indeces = indecies;
        }

        public override void Setup_Position()
        {
            var Width = GetWidth() / 2;
            var Height = GetHeight() / 2;
            var Depth = GetDepth() / 2;
            //Bottom
            var point0 = new Vector3(Width, -Height, -Depth);
            var point1 = new Vector3(Width, -Height, Depth);
            var point2 = new Vector3(-Width, -Height, -Depth);
            var point3 = new Vector3(-Width, -Height, Depth);

            //Top
            var point4 = new Vector3(Width, Height, -Depth);
            var point5 = new Vector3(Width, Height, Depth);
            var point6 = new Vector3(-Width, Height, -Depth);
            var point7 = new Vector3(-Width, Height, Depth);

            List<Vector3> vertices = new List<Vector3>();

            vertices.Add(point0);
            vertices.Add(point1);
            vertices.Add(point2);
            vertices.Add(point3);
            vertices.Add(point4);
            vertices.Add(point5);
            vertices.Add(point6);
            vertices.Add(point7);

            Positions = vertices;
        }

        public override void UploadVAO()
        {
            if (Renderer == null)
            {
                Renderer = new CubeRenderer(this);
            }
            Default_RenderModel();
        }

        public override void Live_Update(Base_Shader ShaderModel)
        {
            base.Live_Update(ShaderModel);
            foreach (var mesh in Meshes)
            {
                mesh.Live_Update(ShaderModel);
            }
        }
    }
}