using OpenGL_Wpf.Geometery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Geometery
{
    public class Plan : Geometry
    {
        public Plan()
        {
			Name = "Plan";

			points = new List<Graphic.Vertex>()
            {
                new Graphic.Vertex()
                {
                     Position=new Graphic.Vertex3(0.5f,0f,0.5f),
                      Normal = new Graphic.Vertex3(0,1,0),
                       TexCoor=new Graphic.Vertex2(1,0),

                },//00

                new Graphic.Vertex()
                {
                     Position=new Graphic.Vertex3(0.5f,0f,-0.5f),
                      Normal = new Graphic.Vertex3(0,1,0),
                       TexCoor=new Graphic.Vertex2(1,1),

                },//01

                new Graphic.Vertex()
                {
                     Position=new Graphic.Vertex3(-0.5f,0f,-0.5f),
                      Normal = new Graphic.Vertex3(0,1,0),
                       TexCoor=new Graphic.Vertex2(0,1)
                },//02

                new Graphic.Vertex()
                {
                     Position=new Graphic.Vertex3(-0.5f,0f,0.5f),
                      Normal = new Graphic.Vertex3(0,1,0),
                       TexCoor=new Graphic.Vertex2(0,0)
                }//03
            };

            Indeces = new List<int>
            {
                0,1,2,
                2,3,0
            };

            objectColor = new OpenTK.Vector3(.2f);

            points.ForEach(p =>
            {
                p.Vcolor = new Graphic.Vertex4(.2f, .2f, .2f,1);
            });

        }
    }
}
