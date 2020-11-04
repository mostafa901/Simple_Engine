using OpenTK;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Core.Interfaces
{
    public interface IDrawable3D : IDrawable
    {
        void DrawAxis();

        public List<Vector3> Positions { get; set; }

        void SetDepth(float value);
    }
}