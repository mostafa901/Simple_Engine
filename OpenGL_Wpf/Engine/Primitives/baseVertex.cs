using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Primitives
{
    public  class baseVertex
    {
        public baseVertex()
        {
            X = Y = 0;
        }
        public baseVertex(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public virtual float[] ToFloat()
        {
            return new float[2] { X, Y };
        }

        public  baseVertex Add(baseVertex v)
        {
            return new baseVertex(v.X + X, v.Y + Y);
        }

    }
}
