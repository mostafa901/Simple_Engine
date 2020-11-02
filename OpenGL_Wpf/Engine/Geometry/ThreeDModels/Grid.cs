﻿using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Core.Events;

namespace Simple_Engine.Engine.Geometry.TwoD
{
    public class Grid : Base_Geo3D
    {
        public Grid(int rows, int columns)
        {
            DrawType = OpenTK.Graphics.OpenGL.PrimitiveType.Lines;
            Rows = rows;
            Columns = columns;
            IsSystemModel = true;
            ShaderModel = new Engine.Render.Shader(Engine.Render.ShaderMapType.Blend, Engine.Render.ShaderPath.Color);
            Dynamic = Engine.Core.Interfaces.IDrawable.DynamicFlag.Positions;
            CameraModel.OnMoving += Refresh;
        }

        private void Refresh(object sender, MoveingEvent e)
        {
            BuildModel();
        }

        public int Rows { get; }
        public int Columns { get; }

        public override void BuildModel()
        {
            Positions = new System.Collections.Generic.List<Vector3>();
            var pos = CameraModel.ActiveCamera.Position + new Vector3(100, 0, 100);
            /* Horizontal lines. */
            for (int i = -(int)pos.X/2; i <= pos.X/2; i++)
            {
                Positions.Add(new OpenTK.Vector3(0, 0, i));
                Positions.Add(new OpenTK.Vector3(pos.Z, 0, i));
            }
            /* Vertical lines. */
            for (int i = -(int)pos.Z/2; i <=pos.Z/2; i++)
            {
                Positions.Add(new OpenTK.Vector3(i, 0, 0));
                Positions.Add(new OpenTK.Vector3(i, 0, pos.X));
            }

            
        }

        public override void Live_Update(Shader ShaderModel)
        {
            base.Live_Update(ShaderModel);
            ShaderModel.SetMatrix4(ShaderModel.Location_LocalTransform, LocalTransform);
        }

        public override void Setup_Indeces()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Normals()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Position()
        {
            throw new NotImplementedException();
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
            throw new NotImplementedException();
        }
    }
}