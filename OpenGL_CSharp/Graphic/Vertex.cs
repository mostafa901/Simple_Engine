using Assimp;
using OpenTK;
using System.Collections.Generic;


namespace OpenGL_CSharp.Graphic
{

  public  class Vertex
    {
        public static int vcount;


		public Vertex4 Vcolor { get; set; } 
		public Vertex2 TexCoor { get; set; } 
		public Vertex3 Position { get; set; }
		public Vertex3 Normal { get; set; } 

        public Vertex()
        {
                    //Position        Texture           color           Normal          
            vcount = Vertex3.vcount + Vertex2.vcount + Vertex4.vcount + Vertex3.vcount;
			//Normal = new Vertex3(0, 1, 0);
			//TexCoor = new Vertex2(0, 0);
			//Vcolor = new Vertex4(1, 0, 0, 1);

        }

        public static Vertex3 FromVertex3(Vector3D v3d)
        {
            return new Vertex3(v3d.X, v3d.Y, v3d.Z);
        }

		public static Vertex3 FromVertex3(Vector3 v3d)
		{
			return new Vertex3(v3d.X, v3d.Y, v3d.Z);
		}

		public static Vertex2 FromVertex2(Vector3D v3d)
        {
            return new Vertex2(v3d.X, v3d.Y);
        }

        public static Vertex4 FromVertex4(Color4D v3d)
        {
            return new Vertex4(v3d.R, v3d.G, v3d.B, v3d.A);
        }


        virtual public float[] data()
        {
            var ls = new List<float>();
            ls.AddRange(Position.data());
            ls.AddRange(TexCoor.data());
            ls.AddRange(Vcolor.data());
            ls.AddRange(Normal.data());

            return ls.ToArray();
        }

        public override string ToString()
        {
            return string.Join("\r\n",
                $"P: {Position.ToString()}",
                $"t: {TexCoor.ToString()}",
                $"vc: {Vcolor.ToString()}",
                $"norm: {Normal.ToString()}")
                ;
        }

    }
}
