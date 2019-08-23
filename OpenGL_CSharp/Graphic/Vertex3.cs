using Assimp;
using OpenTK;

namespace OpenGL_CSharp.Graphic
{

    public class Vertex3 : Vertex2
    {
        public static new int vcount = 3;
        public Vector3 vector3;

      

        public Vertex3(float v1, float v2, float v) : base(v1, v2)
        {
            Z = v;
            vector3 = new Vector3(base.vector2);
            vector3.Z = Z;
        }

        //Coordinates           
        public float Z { get; set; }

        override public float[] data()
        {
            return new float[] { X, Y, Z };
        }

        public override string ToString()
        {
            return $"{vector2.ToString()}, {Z.ToString()}";
        }
    }
}
