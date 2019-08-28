using OpenGL_CSharp.Graphic;
using OpenTK;
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

            //setup Object Color 
            
            objectColor = new Vector3(.5f, 0.5f, 1f);
            


            //define Indeces
            Indeces =
           new List<int>
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

            //BackFace
            points[02].Normal = new Vertex3(0, 0, -1);
            points[05].Normal = new Vertex3(0, 0, -1);
            points[06].Normal = new Vertex3(0, 0, -1);

            //Bottom Face
            points[09].Normal = new Vertex3(0, -1, 0);
            points[10].Normal = new Vertex3(0, -1, 0);
            points[11].Normal = new Vertex3(0, -1, 0);
            points[12].Normal = new Vertex3(0, -1, 0);

            //Right Face
            points[02].Normal = new Vertex3(01, 0, 0);
            points[03].Normal = new Vertex3(01, 0, 0);
            points[04].Normal = new Vertex3(01, 0, 0);

            //Left Face
            points[02].Normal = new Vertex3(-1, 0, 0);
            points[07].Normal = new Vertex3(-1, 0, 0);
            points[08].Normal = new Vertex3(-1, 0, 0);

            //Front Face
            points[00].Normal = new Vertex3(0, 0, 1);
            points[01].Normal = new Vertex3(0, 0, 1);
            points[02].Normal = new Vertex3(0, 0, 1);

            

        }
    }

}
