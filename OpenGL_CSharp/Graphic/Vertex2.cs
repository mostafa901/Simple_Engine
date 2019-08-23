using Assimp;
using OpenTK;

namespace OpenGL_CSharp.Graphic
{

  public  class Vertex2 :Vertex
    {
        public new static int vcount = 2;
        public Vector2 vector2;
        public Vertex2(float v1, float v2)
        {
            X = v1;
            Y = v2;

            vector2 = new Vector2(X, Y);
        }

      

        //Texture
        public float X { get; set; }
        public float Y { get; set; }

        override public float[] data()
        {
            return new float[] { X, Y };
        }

        public override string ToString()
        {
            return $"{X.ToString()}, {Y.ToString()}";
        }
    }

}
