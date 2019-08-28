using Assimp;

namespace OpenGL_CSharp.Graphic
{

  public  class Vertex4 : Vertex3
    {
        public static new int vcount = 4;
        public OpenTK.Vector4 vector4;
		

		 

		public Vertex4(float v1, float v2, float v3, float v4) : base(v1, v2, v3)
        {

            A = v4;
            vector4 = new OpenTK.Vector4(base.vector3, A);
        }

       
        public float A { get; set; }

        override public float[] data()
        {
            return new float[] { X, Y, Z, A };
        }

        public override string ToString()
        {
            return $"{vector3.ToString()}, {A.ToString()}";
        }
    }

}
