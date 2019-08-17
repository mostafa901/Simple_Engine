using OpenGL_CSharp.Graphic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Geometery
{
    class BaseGeometry
    {
        public List<Vertex> points;
        public int[] Indeces;

        public Matrix4 model = Matrix4.Identity;

        public int vbo;
        public int ebo;
        public int vao;

        public void Init()
        {
            var vers = points.SelectMany(o => o.data()).ToArray();
            

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo); //define the type of buffer in gpu memory
                                                          //fill up the buffer with the data
                                                          //we need to define the type of data to be filled and the size in the memory
            GL.BufferData(BufferTarget.ArrayBuffer, vers.Length * sizeof(float), vers, BufferUsageHint.StaticDraw);

            //element buffer
            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indeces.Length * sizeof(int), Indeces, BufferUsageHint.StaticDraw);

            //Element buffer object
            vao = GL.GenBuffer();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BindVertexArray(0);
        }
    }
}
