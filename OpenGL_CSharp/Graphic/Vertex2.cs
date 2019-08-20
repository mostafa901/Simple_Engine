namespace OpenGL_CSharp.Graphic
{

  public  class Vertex2
    {
        public static int vcount = 2;

        public Vertex2(float v1, float v2)
        {
            X = v1;
            Y = v2;
        }

        //Texture
        public float X { get; set; }
        public float Y { get; set; }

        virtual public float[] data()
        {
            return new float[] { X, Y };
        }
    }

}
