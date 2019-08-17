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
                    Position = new Vertex3(0.5f, 0.5f, 0.5f),
                    TexCoor = new Vertex2(1.0f, 1.0f),
                    Vcolor = new Vertex4(1f, 00f, 00f, 1.0f)
                }); // 00

            points.Add(
                new Vertex()
                {
                    Position = new Vertex3(0.5f, -0.5f, 0.5f),
                    TexCoor = new Vertex2(1.0f, 0.0f),
                    Vcolor = new Vertex4(00f, 1f, 0f, 1.0f)
                }); // 01

            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(-0.5f, -0.5f, 0.5f),
                   TexCoor = new Vertex2(0.0f, 0.0f),
                   Vcolor = new Vertex4(0f, 0f, 01f, 1.0f)
               }); // 02

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(-0.5f, 0.5f, 0.5f),
                  TexCoor = new Vertex2(0.0f, 1.0f),
                  Vcolor = new Vertex4(.1f, 01f, 0f, 1.0f)
              });   // 03

            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(0.5f, 0.5f, 0.5f),
                   TexCoor = new Vertex2(0.0f, 1.0f),
                   Vcolor = new Vertex4(0f, 1f, 01f, 1.0f)
               }); // 04

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(0.5f, -0.5f, 0.5f),
                  TexCoor = new Vertex2(0.0f, 0.0f),
                  Vcolor = new Vertex4(1f, 00f, .1f, 1.0f)
              });   // 05

            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(0.5f, -0.5f, -0.5f),
                   TexCoor = new Vertex2(1.0f, 0.0f),
                   Vcolor = new Vertex4(1f, 1f, 01f, 1.0f)
               }); // 06

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(0.5f, 0.5f, -0.5f),
                  TexCoor = new Vertex2(1.0f, 1.0f),
                  Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
              });   // 07  
            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(0.5f, 0.5f, -0.5f),
                 TexCoor = new Vertex2(0.0f, 1.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 08

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(0.5f, -0.5f, -0.5f),
                 TexCoor = new Vertex2(0.0f, 0.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 09

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(-0.5f, 0.5f, -0.5f),
                 TexCoor = new Vertex2(1.0f, 1.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 10

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(-0.5f, -0.5f, -0.5f),
                 TexCoor = new Vertex2(1.0f, 0.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 11

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(-0.5f, 0.5f, -0.5f),
                 TexCoor = new Vertex2(0.0f, 1.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 12

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(-0.5f, 0.5f, 0.5f),
                 TexCoor = new Vertex2(1.0f, 1.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 13


            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(-0.5f, -0.5f, 0.5f),
                 TexCoor = new Vertex2(1.0f, 00.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 14

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(-0.5f, -0.5f, -0.5f),
                 TexCoor = new Vertex2(0.0f, 0.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 15

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(-0.5f, -0.5f, -0.5f),
                 TexCoor = new Vertex2(0.0f, 0.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 16

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(-0.5f, -0.5f, 0.5f),
                 TexCoor = new Vertex2(0.0f, 1.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 17

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(0.5f, -0.5f, 0.5f),
                 TexCoor = new Vertex2(1.0f, 1.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 18

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(0.5f, -0.5f, -0.5f),
                 TexCoor = new Vertex2(1.0f, 0.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 19

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(0.5f, 0.5f, -0.5f),
                 TexCoor = new Vertex2(1.0f, 1.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 20

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(-0.5f, 0.5f, -0.5f),
                 TexCoor = new Vertex2(0.0f, 1.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 21

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(-0.5f, 0.5f, 0.5f),
                 TexCoor = new Vertex2(0.0f, 0.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 22

            points.Add(
             new Vertex()
             {
                 Position = new Vertex3(0.5f, 0.5f, 0.5f),
                 TexCoor = new Vertex2(1.0f, 0.0f),
                 Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
             });   // 23

            //define Indeces
            Indeces =
             new int[]
                     {
                         //backface
                        11,10,08,
                        11,08,09,

                        //bottom
                        19,17,16,
                        19,18,17,
                        //top
                        21,22,23,
                        21,23,20,

                        //right face
                       04,05,07,
                       07,05,06,
                         
                        //left face
                        12,15,14,
                        12,14,13,

                          //frontface
                        3,2,1,
                        3,1,0
                 };

            Init();
        }
    }

}
