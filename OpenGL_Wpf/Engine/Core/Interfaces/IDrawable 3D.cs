using Simple_Engine.Views.ThreeD.Engine.Geometry.Core;

using Simple_Engine.Views.ThreeD.Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Core.Interfaces
{
    public interface IDrawable3D : IDrawable
    {
        void DrawAxis();

        public List<Vector3> Positions { get; set; }

        void SetDepth(float value);
         
    }
}