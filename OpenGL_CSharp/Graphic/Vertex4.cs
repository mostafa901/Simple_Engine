namespace OpenGL_CSharp.Graphic
{

    class Vertex4 : Vertex3
    {
        public static new int vcount = 4;

        public Vertex4(float v1, float v2, float v3, float v4) : base(v1, v2, v3)
        {

            A = v4;
           
        }

        public float A { get; set; }

        override public float[] data()
        {
            return new float[] { X, Y, Z, A };
        }
    }

}
