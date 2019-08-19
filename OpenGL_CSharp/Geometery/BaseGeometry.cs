using OpenGL_CSharp.Graphic;
using OpenGL_CSharp.Shaders;
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

        public Vector3 objectColor;
        public static float specintens = 0.4f;
        public bool IsLight = false;

        public Matrix4 model = Matrix4.Identity;

        public int vbo = -1;
        public int ebo = -1;
        public int vao = -1;

        public float[] vers;

        public BaseShader shader;

        public BaseGeometry()
        {


        }

        public void LoadGeometry()
        {
            if (vers == null) //no need to recreate if already created
                vers = points.SelectMany(o => o.data()).ToArray();           

            if (vbo == -1) //no need to recreate if already created
                vbo = GL.GenBuffer();
             
            //define the type of buffer in gpu memory
            //fill up the buffer with the data
            //we need to define the type of data to be filled and the size in the memory
            GL.BufferData(BufferTarget.ArrayBuffer, vers.Length * sizeof(float), vers, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            shader = new Tex2Frag(new Vector3(1), @"Textures\container.jpg", @"Textures\container_specular.jpg");            


            //Element buffer object
            if (vao == -1) //no need to recreate if already created
                vao = GL.GenBuffer();
            //Binding Vertex array object
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            //element buffer
            //--------------
            if (ebo == -1) ////no need to recreate if already created
                ebo = GL.GenBuffer();
            //Element Buffer
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indeces.Length * sizeof(int), Indeces, BufferUsageHint.StaticDraw);
           
        }


       

        public void Dispose()
        {
            shader.Dispose();

        }
    }
}
