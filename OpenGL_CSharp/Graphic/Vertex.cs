using System.Collections.Generic;


namespace OpenGL_CSharp.Graphic
{

    class Vertex
    {
        public static int vcount;


        public Vertex3 Vcolor { get; set; }    
        public Vertex2 TexCoor { get; set; }
        public Vertex3 Position { get; set; }
        public Vertex3 Normal { get; set; }

        public Vertex()
        {
            vcount = Vertex2.vcount + Vertex3.vcount + Vertex4.vcount + Vertex3.vcount;
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

    }
}
