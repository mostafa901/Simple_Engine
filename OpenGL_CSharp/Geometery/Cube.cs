using OpenGL_CSharp.Graphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Geometery
{
    class CreateCube : BaseGeometry
    {

        public CreateCube()
        {
            points = new List<Vertex>();

            points.Add(
                new Vertex()
                {
                    Position = new Vertex3(0.5f, 0.5f, -0.5f),
                    TexCoor = new Vertex2(1.0f, 1.0f),
                    Vcolor = new Vertex4(1f, 00f, 00f, 1.0f)
                }); // 00

            points.Add(
                new Vertex()
                {
                    Position = new Vertex3(0.5f, -0.5f, -0.5f),
                    TexCoor = new Vertex2(1.0f, 0.0f),
                    Vcolor = new Vertex4(00f, 1f, 0f, 1.0f)
                }); // 01

            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(-0.5f, -0.5f, -0.5f),
                   TexCoor = new Vertex2(0.0f, 0.0f),
                   Vcolor = new Vertex4(0f, 0f, 01f, 1.0f)
               }); // 02

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(-0.5f, 0.5f, -0.5f),
                  TexCoor = new Vertex2(0.0f, 1.0f),
                  Vcolor = new Vertex4(.1f, 01f, 0f, 1.0f)
              });   // 03

            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(0.5f, -0.5f, 0.5f),
                   TexCoor = new Vertex2(0.0f, 0.0f),
                   Vcolor = new Vertex4(0f, 1f, 01f, 1.0f)
               }); // 04

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(0.5f, 0.5f, 0.5f),
                  TexCoor = new Vertex2(0.0f, 1.0f),
                  Vcolor = new Vertex4(1f, 00f, .1f, 1.0f)
              });   // 05

            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(-0.5f, -0.5f, 0.5f),
                   TexCoor = new Vertex2(0.0f, 0.0f),
                   Vcolor = new Vertex4(1f, 1f, 01f, 1.0f)
               }); // 06

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(-0.5f, 0.5f, 0.5f),
                  TexCoor = new Vertex2(0.0f, 1.0f),
                  Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
              });   // 07  




            //define Indeces
            Indeces =
             new int[]
                     {
                         //backface
                        6,7,4,
                        7,5,4,

                        //bottom
                        1,2,6,
                        4,1,6,

                        //top
                        0,7,3,
                        0,5,7,

                        //right face
                       4,0,1,
                        5,0,4,
                         
                        //left face
                        6,3,7,
                        6,2,3,

                          //frontface
                        3,2,1,
                        3,1,0


                 };

            Init();
        }
    }

}
