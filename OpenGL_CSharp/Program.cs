using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace OpenGL_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //initialize window
            var win = new GameWindow(800, 800, OpenTK.Graphics.GraphicsMode.Default, "Test", GameWindowFlags.Default, DisplayDevice.Default, 3, 3, OpenTK.Graphics.GraphicsContextFlags.Debug);

            //setupsceansettings
            SetupScene(win);
            win.Load += Win_Load; //one time load on start
            win.UpdateFrame += Win_UpdateFrame; //on each fram do this           
            win.Closing += Win_Closing; //on termination do this
            win.KeyDown += Win_KeyDown; //keydown event

            //start game window
            win.Run(10);

        }

        private static void Win_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Right)
            {
                pipe.offsetX += .2f;
            }
            if (e.Key == OpenTK.Input.Key.Left)
            {
                pipe.offsetX -= .2f;
            }
            if (e.Key == OpenTK.Input.Key.Escape)
            {
                ((GameWindow)sender).Close();
            }
        }

        static pipelinevars pipe;
        class pipelinevars
        {
            public int programId, vbo, vao, ebo;
            public float offsetX = 0.5f;
            public float[] vers;
            internal int[] indeces;
            public int texid1;
            internal int fragshad;
            internal int vershad;
            internal int texid2;
        }
        private static void Win_Load(object sender, EventArgs e)
        {
            var win = (GameWindow)sender;

            //defin the shap to be drawn
            pipe.vers = new float[]
             {
               
               // Position         Texture coordinates    Vertex Color
             0.5f,  0.5f, 0.0f,      1.0f, 1.0f,         1f, 00f,00f, // top right
             0.5f, -0.5f, 0.0f,      1.0f, 0.0f,         00f,00f,01f, // bottom right
            -0.5f, -0.5f, 0.0f,      0.0f, 0.0f,         .5f,.5f,01f,// bottom left
            -0.5f,  0.5f, 0.0f,      0.0f, 1.0f,         .1f,01f,.8f,// top left 

             };

            //define Indeces
            pipe.indeces = new int[]
            {
                 0, 1, 3,
                 1, 2, 3
            };


            int vbo = pipe.vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo); //define the type of buffer in gpu memory
            //fill up the buffer with the data
            //we need to define the type of data to be filled and the size in the memory
            GL.BufferData(BufferTarget.ArrayBuffer, pipe.vers.Length * sizeof(float), pipe.vers, BufferUsageHint.StaticDraw);

            //element buffer
            pipe.ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, pipe.ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, pipe.indeces.Length * sizeof(int), pipe.indeces, BufferUsageHint.StaticDraw);

            //load vertix/Fragment shader
            pipe.vershad = CreateShader(Shaders.VertexShaders.VShader(), ShaderType.VertexShader);
            pipe.fragshad = CreateShader(Shaders.FragmentShaders.TexFrag2Tex(), ShaderType.FragmentShader);

            //create program, link shaders and test the results
            int progid = CreatePrognLinkShader(pipe.vershad, pipe.fragshad);
            GL.UseProgram(progid);


            //load Textures
            pipe.texid1 = Textures.Textures.AddTexture(TextureUnit.Texture0, @"C:\Users\mosta\Downloads\container.jpg");
            Textures.Textures.Link(TextureUnit.Texture0, pipe.texid1);

            pipe.texid2 = Textures.Textures.AddTexture(TextureUnit.Texture1, @"D:\My Book\layan photo 6x4.jpg");
            Textures.Textures.Link(TextureUnit.Texture1, pipe.texid2);

            //tel GPU the loation of the textures in the shaders
            var txunloc1 = GL.GetUniformLocation(pipe.programId, "texture0");
            var txunloc2 = GL.GetUniformLocation(pipe.programId, "texture1");
            GL.Uniform1(txunloc1, 0);
            GL.Uniform1(txunloc2, 1);
                       
            //Element buffer object
            int vao = pipe.vao = GL.GenBuffer();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, pipe.ebo);

            //since we are using vertex, opengl doesn't understand the specs of the vertex so we use vertexpointerattribute for this
            //now it is 5 instead of 3 for the texture coordinates
            var verloc = GL.GetAttribLocation(pipe.programId, "aPos");
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(verloc); //now activate Vertexattrib

            //GetTexture coordinates
            var texloc = GL.GetAttribLocation(pipe.programId, "aTexCoord");
            GL.EnableVertexAttribArray(texloc);
            GL.VertexAttribPointer(texloc, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            //vertex Color
            var vercolloc = GL.GetAttribLocation(pipe.programId, "aVerColor");
            GL.EnableVertexAttribArray(vercolloc);
            GL.VertexAttribPointer(vercolloc, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));

        }



        static void DrawShape()
        {
            //bind vertex object
            GL.BindVertexArray(pipe.vao);

            //setup vertix holder to the GPU memory
            //--------------
            Textures.Textures.Link(TextureUnit.Texture0, pipe.texid1);
            Textures.Textures.Link(TextureUnit.Texture1, pipe.texid2);

            GL.UseProgram(pipe.programId);

        }



        static int CreatePrognLinkShader(int vershad, int fragshad)
        {
            int progid = pipe.programId = GL.CreateProgram();
            GL.AttachShader(progid, vershad);
            GL.AttachShader(progid, fragshad);
            GL.LinkProgram(progid);

            //test if the prog is fine
            var result = GL.GetProgramInfoLog(progid);
            if (!string.IsNullOrEmpty(result))
            {
                Console.WriteLine(result);
            }

            //after linking there is no need to keep/attach the shaders and should be cleared from memory
            GL.DetachShader(progid, vershad);
            GL.DetachShader(progid, fragshad);
            GL.DeleteShader(vershad);
            GL.DeleteShader(fragshad);

            return progid;
        }



        private static void SetupScene(GameWindow win)
        {
            //intialize holder for the project main variables
            pipe = new pipelinevars();
            //defin viewport size
            GL.Viewport(100, 100, 700, 700);
            GL.ClearColor(Color.CornflowerBlue);//set background color
            GL.CullFace(CullFaceMode.Back); //set which face to be hidden
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill); //set polygon draw mode



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



        private static void Win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //clear resources from here
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteBuffer(pipe.vbo);
            GL.DeleteVertexArray(pipe.vao);
            GL.DeleteShader(pipe.vershad);
            GL.DeleteShader(pipe.fragshad);
            GL.DeleteTexture(pipe.texid1);
            GL.DeleteTexture(pipe.texid2);

            GL.DeleteProgram(pipe.programId);
        }

        private static void Win_UpdateFrame(object sender, FrameEventArgs e)
        {
            //clear the scean from any drawing before drawing
            GL.Clear(ClearBufferMask.ColorBufferBit);

            DrawShape();

            // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawElements(PrimitiveType.Triangles, pipe.indeces.Length, DrawElementsType.UnsignedInt, 0);
            //swap the buffer (bring what has been rendered in theback to the front)
            var win = (GameWindow)sender;
            win.SwapBuffers();
        }
    }
}
