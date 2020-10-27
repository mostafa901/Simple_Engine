using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Primitives
{
    public class Vertex3: baseVertex
    {
        public Vertex3() : base()
        {
            Z=0;
        }
        public Vertex3(float x, float y, float z) : base(x, y)
        {
            Z = z;
        }

        public float Z { get; set; }

        public override float[] ToFloat()
        {
            return new float[3] { X, Y, Z };
        }

        public Vertex3 Add(Vertex3 v)
        {
            return new Vertex3(v.X + X, v.Y + Y, v.Z + Z);
        }

        public Vertex3 Subtract(Vertex3 v)
        {
            return new Vertex3(X-v.X, Y-v.Y , Z-v.Z );
        }


        public Vertex3 Normalize()
        {
            var mag = Length();
            if (mag == 0) return new Vertex3();
            return new Vertex3(X / mag, Y / mag, Z / mag);
        }

        internal Vertex3 Multiply(float t)
        {
            return new Vertex3(X * t, Y * t, Z * t);
        }

        public float Length()
        {
            return (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
            
        }

    }
}