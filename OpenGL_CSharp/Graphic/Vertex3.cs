namespace OpenGL_CSharp.Graphic
{

    class Vertex3 : Vertex2
    {
        public static new int vcount = 3;
        public Vertex3(float v1, float v2, float v) : base(v1, v2)
        {
            Z = v;
        }

        //Coordinates           
        public float Z { get; set; }

        override public float[] data()
        {
            return new float[] { X, Y, Z };
        }
    }
}
