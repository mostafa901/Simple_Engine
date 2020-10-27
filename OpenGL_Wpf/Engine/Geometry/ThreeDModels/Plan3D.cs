using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Fonts;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Geometry.Render;
using Simple_Engine.Engine.GUI.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Geometry.ThreeDModels
{
    public class Plan3D : Base_Geo3D
    {
        public Plan3D(float width)
        {
            SetWidth(width);
            SetHeight(width);
        }

        public override void BuildModel()
        {
            DrawType = PrimitiveType.TriangleStrip;
             
            Setup_Position();
            Setup_Indeces();
            Setup_Normals();
            Setup_TextureCoordinates();
        }

        public override void RenderModel()
        {
            Renderer = new GeometryRenderer(this);
            Default_RenderModel();
        }

        public void Dispose()
        {
            ShaderModel.Dispose();
            Renderer.Dispose();
            TextureModel?.Dispose();
        }

        public override void Setup_Position()
        {
            var width = GetWidth()/2;
            var height = GetHeight()/2;
            if (height == 0) height = GetDepth()/2;
            Vector3 v0 = new Vector3(width, height, 0);
            Vector3 v1 = new Vector3(-width, height, 0);
            Vector3 v2 = new Vector3(-width, -height, 0);
            Vector3 v3 = new Vector3(width, -height, 0);

            Positions = new List<Vector3>();
            Positions.Add(v2);
            Positions.Add(v1);
            Positions.Add(v3);
            Positions.Add(v0);
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
            TextureCoordinates = new List<Vector2>();
            var v0 = new Vector2(0, 0);
            var v1 = new Vector2(0, 1 * yScale);
            var v2 = new Vector2(1 * xScale, 0);
            var v3 = new Vector2(1 * xScale, 1 * yScale);

            TextureCoordinates.Add(v0);
            TextureCoordinates.Add(v1);
            TextureCoordinates.Add(v2);
            TextureCoordinates.Add(v3);
        }

        public override void Setup_Normals()
        {
            Normals = new List<Vector3>();
            for (int i = 0; i < 4; i++)
            {
                //-z since this is a default modeled plan facing camera
                Normals.Add(new Vector3(0, 0, -1));
            }
        }

        public override void Setup_Indeces()
        {
            Indeces = new List<int>();
            Indeces.AddRange(new List<int> { 0, 1, 2, 1, 2, 3 });
        }
    }
}