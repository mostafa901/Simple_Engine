using OpenTK;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Core.Interfaces
{
    public interface IDrawable2D : IDrawable
    {
        public List<Vector2> Positions { get; set; }
    }
}