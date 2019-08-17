using OpenGL_CSharp.Graphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Geometery
{
    class Pyramid : BaseGeometry
    {

        public Pyramid()
        {
            #region Points
            points = new List<Vertex>();

            points.Add(
                new Vertex()
                {
                    Position = new Vertex3(-0.5f, -0.5f, 0.5f),
                    TexCoor = new Vertex2(0.0f, 0.0f),
                    Vcolor = new Vertex4(00f, 1f, 0f, 1.0f)
                }); // 00

            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(0.5f, -0.5f, 0.5f),
                   TexCoor = new Vertex2(01f, 0.0f),
                   Vcolor = new Vertex4(0f, 0f, 01f, 1.0f)
               }); // 01



            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(00f, 0.5f, 0f),
                   TexCoor = new Vertex2(0.5f, 0.5f),
                   Vcolor = new Vertex4(0f, 1f, 01f, 1.0f)
               }); // 02

            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(0.5f, -0.5f, 0.5f),
                   TexCoor = new Vertex2(0.0f, 0.0f),
                   Vcolor = new Vertex4(0f, 0f, 01f, 1.0f)
               }); // 03

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(0.5f, -0.5f, -.5f),
                  TexCoor = new Vertex2(1f, 0.0f),
                  Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
              });   // 04


            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(0.5f, -0.5f, -.5f),
                  TexCoor = new Vertex2(0.0f, 0.0f),
                  Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
              });   // 05

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(-0.5f, -0.5f, -0.5f),
                  TexCoor = new Vertex2(0.0f, 1.0f),
                  Vcolor = new Vertex4(00f, 1f, 0f, 1.0f)
              }); // 06



            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(-0.5f, -0.5f, -0.5f),
                  TexCoor = new Vertex2(0.0f, 0.0f),
                  Vcolor = new Vertex4(00f, 1f, 0f, 1.0f)
              }); // 07

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(-0.5f, -0.5f, 0.5f),
                  TexCoor = new Vertex2(0.0f, 1.0f),
                  Vcolor = new Vertex4(00f, 1f, 0f, 1.0f)
              }); // 08

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(-0.5f, -0.5f, 0.5f),
                  TexCoor = new Vertex2(0.0f, 1.0f),
                  Vcolor = new Vertex4(00f, 1f, 0f, 1.0f)
              }); // 09
            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(-0.5f, -0.5f, -0.5f),
                  TexCoor = new Vertex2(0.0f, 0.0f),
                  Vcolor = new Vertex4(00f, 1f, 0f, 1.0f)
              }); // 10
            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(0.5f, -0.5f, -.5f),
                  TexCoor = new Vertex2(1.0f, 0.0f),
                  Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
              });   // 11

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(0.5f, -0.5f, 0.5f),
                 TexCoor = new Vertex2(1.0f, 1.0f),
                 Vcolor = new Vertex4(0f, 0f, 01f, 1.0f)
             }); // 12 
            #endregion

            //define Indeces
            Indeces =
           new int[]
                   {
                         //backface                         
                        06,02,05,
                        //bottom
                        11,12,09,
                        10,11,09,
                        //right face
                       03,04,02,
                        //left face
                        07,08,02,                        
                          //frontface
                        00,01,02

               };

            

        }
    }

}
