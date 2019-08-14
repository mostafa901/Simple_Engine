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
            var win = new GameWindow(800, 800, OpenTK.Graphics.GraphicsMode.Default, "Test", GameWindowFlags.Default);

            //setupsceansettings
            SetupScene(win);
            win.Load += Win_Load; //one time load on start
            win.UpdateFrame += Win_UpdateFrame; //on each fram do this           
            win.Closing += Win_Closing; //on termination do this
            win.KeyDown += Win_KeyDown; //keydown event
            win.Unload += Win_Unload;
            //start game window
            win.Run(3,3);

        }

        private static void Win_Unload(object sender, EventArgs e)
        {
            // After the program ends, we have to manually cleanup our buffers.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(pipe.vao);
            GL.DeleteBuffer(pipe.vbo);
            GL.DeleteBuffer(pipe.ebo);

        }

        private static void Win_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Right)
            {
                pipe.offsetX += .2f;
            }
            if(e.Key==OpenTK.Input.Key.Left)
            {
                pipe.offsetX -= .2f;
            }
            if(e.Key==OpenTK.Input.Key.Escape)
            {
                ((GameWindow)sender).Close();
            }
        }

        static pipelinevars pipe;
        class pipelinevars
        {
            public int programId, vbo,vao,ebo;
            public float offsetX = 0.5f;
            public float[] vers;
            internal int[] indeces;
        }
        private static void Win_Load(object sender, EventArgs e)
        {
            var win = (GameWindow)sender;
            //load vertix/Fragment shader
            var vershad = CreateShader(Shaders.VertexShaders.VShader(), ShaderType.VertexShader);
            var fragshad = CreateShader(Shaders.FragmentShaders.Frag(), ShaderType.FragmentShader);

            //create program, link shaders and test the results
            int progid = CreatePrognLinkShader(vershad, fragshad);
            GL.UseProgram(progid);

            DrawShape();

          
        }

        static void DrawShape()
        {
            //defin the shap to be drawn
            pipe.vers = new float[]
             {
                -pipe.offsetX,-.5f+pipe.offsetX,0,
                 -pipe.offsetX,0f+pipe.offsetX,0,
                .5f+pipe.offsetX,-.5f+pipe.offsetX,0,
               00f+pipe.offsetX,.5f+pipe.offsetX,0
             };

            //define Indeces
            pipe.indeces = new int[]
            {
                0,1,2,3
            };

            //setup vertix holder to the GPU memory
            //--------------

            int vbo = pipe.vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo); //define the type of buffer in gpu memory
            //fill up the buffer with the data
            //we need to define the type of data to be filled and the size in the memory
            GL.BufferData(BufferTarget.ArrayBuffer, pipe.vers.Length * sizeof(float), pipe.vers, BufferUsageHint.StaticDraw);

            int vao = pipe.vao = GL.GenBuffer();
            GL.BindVertexArray(vao);

            //since we are using vertex, opengl doesn't understand the specs of the vertex so we use vertexpointerattribute for this
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0); //now activate that

            //element buffer
            pipe.ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, pipe.ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, pipe.indeces.Length * sizeof(int), pipe.indeces, BufferUsageHint.StaticRead);



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
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //set polygon draw mode

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

            GL.DeleteProgram(pipe.programId);
        }

        private static void Win_UpdateFrame(object sender, FrameEventArgs e)
        {
            //clear the scean from any drawing before drawing
            GL.Clear(ClearBufferMask.ColorBufferBit);

            DrawShape();

            // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawElements(PrimitiveType.LineLoop, pipe.indeces.Length, DrawElementsType.UnsignedInt, 0);
            //swap the buffer (bring what has been rendered in theback to the front)
            var win = (GameWindow)sender;
            win.SwapBuffers();
        }
    }
}
