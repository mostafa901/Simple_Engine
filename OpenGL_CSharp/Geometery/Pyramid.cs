﻿using OpenGL_CSharp.Graphic;
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
            points = new List<Vertex>();

            points.Add(
                new Vertex()
                {
                    Position = new Vertex3(0.5f, -0.5f, -0.5f),
                    TexCoor = new Vertex2(1.0f, 0.0f),
                    Vcolor = new Vertex4(00f, 1f, 0f, 1.0f)
                }); // 00

            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(-0.5f, -0.5f, -0.5f),
                   TexCoor = new Vertex2(0.0f, 0.0f),
                   Vcolor = new Vertex4(0f, 0f, 01f, 1.0f)
               }); // 01



            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(0.5f, -0.5f, -0.5f),
                   TexCoor = new Vertex2(0.0f, 0.0f),
                   Vcolor = new Vertex4(0f, 1f, 01f, 1.0f)
               }); // 02



            points.Add(
               new Vertex()
               {
                   Position = new Vertex3(-0.5f, -0.5f, -0.5f),
                   TexCoor = new Vertex2(0.0f, 0.0f),
                   Vcolor = new Vertex4(1f, 1f, 01f, 1.0f)
               }); // 03

            points.Add(
              new Vertex()
              {
                  Position = new Vertex3(0.0f, 0.5f, 00f),
                  TexCoor = new Vertex2(0.0f, 1.0f),
                  Vcolor = new Vertex4(.3f, 00.3f, .3f, 1.0f)
              });   // 04




            //define Indeces
            Indeces =
           new int[]
                   {
                         //backface                         
                        2,3,4,
                        //bottom
                        1,3,0,
                        0,3,2,                        
                        //right face
                       2,4,0,
                        //left face
                        3,1,4,
                        
                          //frontface
                        0,4,1

               };

            Init();

        }
    }

}