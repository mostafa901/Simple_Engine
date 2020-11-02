using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.Axis;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Core.Events;
using Simple_Engine.Engine.Geometry.Render;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.Particles;
using Simple_Engine.Engine.Render;
using Simple_Engine.Extentions;
using Simple_Engine.ToolBox;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using OpenTK.Input;
using Shared_Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Simple_Engine.Engine.Core.Abstracts
{
    public abstract class Base_Geo2D : Base_Geo, IDrawable2D
    {
        public float GravityEffect = 0;
        public Base_Geo2D()
        {
            Positions = new List<Vector2>();

        }

        public Vector3 Position { get; set; }
        public List<Vector2> Positions { get; set; }
        public Vector3 Velocity { get; set; }
        public Mesh3D AddMesh(Matrix4 mat)
        {
            ShaderModel.EnableInstancing = true;
            var mesh = new Mesh3D(this);
            mesh.LocalTransform = mat;
            Meshes.Add(mesh);
            return mesh;
        }

        public override void CloneModel(Base_Geo sourceModel)
        {
            base.CloneModel(sourceModel);
            Positions = ((Base_Geo2D)sourceModel).Positions;
        }

        public void Default_RenderModel()
        {

            Renderer.RenderModel();
            ShaderModel.UploadDefaults(this);
        }

        public void Dispose()
        {
            ShaderModel.Dispose();
            Renderer.Dispose();
            TextureModel?.Dispose();
        }

        protected void Build_DefaultModel()
        {
            DrawType = PrimitiveType.TriangleStrip;
            LocalTransform = Matrix4.Identity;
            DefaultColor = new Vector4(Randoms.Next(.1f, 1.0f), Randoms.Next(.1f, 1.0f), Randoms.Next(.1f, 1.0f), 0.5f);
            Default_Setup_Position();
            Default_Setup_Indeces();
            Default_Setup_Normals();
            Default_Setup_TextureCoordinates();
        }
        private void Default_Setup_Indeces()
        {
            Indeces = new List<int>();
            Indeces.AddRange(new List<int> { 0, 1, 2, 1, 2, 3 });
        }

        private void Default_Setup_Normals()
        {
            Normals = new List<Vector3>();
            for (int i = 0; i < 4; i++)
            {
                Normals.Add(new Vector3(0, 1, 0));
            }
        }

        private void Default_Setup_Position()
        {
            var Width = GetWidth();
            var Height = GetHeight();
            Vector2 v0 = new Vector2(Width, Height);
            Vector2 v1 = new Vector2(-Width, Height);
            Vector2 v2 = new Vector2(-Width, -Height);
            Vector2 v3 = new Vector2(Width, -Height);

            Positions = new List<Vector2>();
            Positions.Add(v2);
            Positions.Add(v1);
            Positions.Add(v3);
            Positions.Add(v0);
        }

        private void Default_Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
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

        
    }
}