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
        public int texid1=-1;
        public int texid2=-1;
        public int vershad;
        public int fragshad;
        public void Init()
        {
            var vers = points.SelectMany(o => o.data()).ToArray();

            //Element buffer object
            vao = GL.GenBuffer();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo); //define the type of buffer in gpu memory
                                                          //fill up the buffer with the data
                                                          //we need to define the type of data to be filled and the size in the memory
            GL.BufferData(BufferTarget.ArrayBuffer, vers.Length * sizeof(float), vers, BufferUsageHint.StaticDraw);
            
            //since we are using vertex, opengl doesn't understand the specs of the vertex so we use vertexpointerattribute for this
            //now it is 5 instead of 3 for the texture coordinates
            var verloc = GL.GetAttribLocation(Program.pipe.programId, "aPos");
            GL.VertexAttribPointer(verloc, Vertex3.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 0);
            GL.EnableVertexAttribArray(verloc); //now activate Vertexattrib

            //GetTexture coordinates
            var texloc = GL.GetAttribLocation(Program.pipe.programId, "aTexCoord");
            GL.VertexAttribPointer(texloc, Vertex2.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(texloc);

            //vertex Color
            var vercolloc = GL.GetAttribLocation(Program.pipe.programId, "aVerColor");
            GL.VertexAttribPointer(vercolloc, Vertex4.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(vercolloc);
            
            //element buffer
            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indeces.Length * sizeof(int), Indeces, BufferUsageHint.StaticDraw);
             
            //load vertix/Fragment shader
            vershad = CreateShader(Shaders.VertexShaders.VShader(), ShaderType.VertexShader);
            fragshad = CreateShader(Shaders.FragmentShaders.TexFrag2Tex(), ShaderType.FragmentShader);

            //create program, link shaders and test the results
            int progid = CreatePrognLinkShader(vershad, fragshad);
            GL.UseProgram(progid);

            //load Textures
        if(texid1==-1)    texid1 = Textures.Textures.AddTexture(TextureUnit.Texture0, @"C:\Users\mosta\Downloads\container.jpg");
            //   pipe.texid2 = Textures.Textures.AddTexture(TextureUnit.Texture1, @"D:\My Book\layan photo 6x4.jpg");

        }

        //create shaders
        static int CreateShader(string source, ShaderType shadtype)
        {
            int shadid = GL.CreateShader(shadtype);
            GL.ShaderSource(shadid, source);
            GL.CompileShader(shadid);

            //test if the compilation is correct
            var result = GL.GetShaderInfoLog(shadid);
            if (!string.IsNullOrWhiteSpace(result))
            {
                Console.WriteLine(result);

            }
            return shadid;
        }

        static int CreatePrognLinkShader(int vershad, int fragshad)
        {
            Program.pipe.programId = Program.pipe.programId == -1 ? GL.CreateProgram() : Program.pipe.programId;
            GL.AttachShader(Program.pipe.programId, vershad);
            GL.AttachShader(Program.pipe.programId, fragshad);
            GL.LinkProgram(Program.pipe.programId);

            //test if the prog is fine
            var result = GL.GetProgramInfoLog(Program.pipe.programId);
            if (!string.IsNullOrEmpty(result))
            {
                Console.WriteLine(result);
            }

            //after linking there is no need to keep/attach the shaders and should be cleared from memory
            GL.DetachShader(Program.pipe.programId, vershad);
            GL.DetachShader(Program.pipe.programId, fragshad);
            GL.DeleteShader(vershad);
            GL.DeleteShader(fragshad);

            return Program.pipe.programId;
        }
    }
}
