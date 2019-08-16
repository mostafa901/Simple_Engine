using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_CSharp.Graphic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenGL_CSharp
{
    partial class Program
    {
        static void Main(string[] args)
        {
            //initialize window
            var win = new GameWindow(800, 800, OpenTK.Graphics.GraphicsMode.Default, "Test", GameWindowFlags.Default, DisplayDevice.Default, 3, 3, OpenTK.Graphics.GraphicsContextFlags.Debug);

            //setupsceansettings
            SetupScene(win);
            pipe.win = win;

            win.Load += Win_Load; //one time load on start
            win.UpdateFrame += Win_UpdateFrame; //on each fram do this           
            win.Closing += Win_Closing; //on termination do this
            win.KeyDown += Win_KeyDown; //keydown event
            win.MouseWheel += Win_MouseWheel;
            //start game window
            win.Run(30);

        }

        private static void Win_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            cam.Fov(e.DeltaPrecise);
            cam.updateCamera();
        }

        private static void Win_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {

            var win = (GameWindow)sender;

            if (e.Key == OpenTK.Input.Key.Right)
            {
                cam.Position -= new Vector3(.2f, 0, 0);
                cam.Target -= new Vector3(.2f, 0, 0);
            }
            if (e.Key == OpenTK.Input.Key.Left)
            {
                cam.Position += new Vector3(.2f, 0, 0);
                cam.Target += new Vector3(.2f, 0, 0);

            }

            if (e.Key == OpenTK.Input.Key.Down)
            {
                cam.Position -= new Vector3(0f, .2f, 0);
                cam.Target -= new Vector3(0f, .2f, 0);
            }
            if (e.Key == OpenTK.Input.Key.Up)
            {
                cam.Position += new Vector3(0f, 0.2f, 0);
                cam.Target += new Vector3(0, 0.2f, 0);

            }

            if (e.Key == OpenTK.Input.Key.Escape)
            {
                ((GameWindow)sender).Close();
            }


            if (e.Key == OpenTK.Input.Key.W)
            {
                Vangle += 10;

                var m1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(Vangle));
                cam.View = cam.View * m1;
                cam.Position = cam.View.ExtractTranslation();
            }

            if (e.Key == OpenTK.Input.Key.S)
            {
                Vangle -= 10;
                var m1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(Vangle));
                cam.View = cam.View * m1;
                cam.Position = cam.View.ExtractTranslation();
            }

            if (e.Key == OpenTK.Input.Key.A)
            {
                Hangle -= 10;
                cam.Position = new Vector3((float)Math.Sin(MathHelper.DegreesToRadians(Hangle)) * r, cam.Position.Y, (float)Math.Cos(MathHelper.DegreesToRadians(Hangle)) * r);

            }

            if (e.Key == OpenTK.Input.Key.D)
            {
                Hangle += 10;
                cam.Position = new Vector3((float)Math.Sin(MathHelper.DegreesToRadians(Hangle)) * r, cam.Position.Y, (float)Math.Cos(MathHelper.DegreesToRadians(Hangle)) * r);
            }

            var mouse = Mouse.GetState();



            if (e.Key == Key.ControlLeft && win.Focused)
            {
                var dx = mouse.X / 800f - oldx;
                var dy = mouse.Y / 800f - oldy;

                win.CursorVisible = true;
                cam.Target += new Vector3(dx, dy, 0);
                oldx = mouse.X / 800f;
                oldy = mouse.Y / 800f;
            }
            else
            {
                win.CursorVisible = true;
                cam.Target = Vector3.Zero;
            }


            cam.updateCamera();


        }
        static float oldx = 0;
        static float oldy = 0;

        static float r = 5f;
        static double Hangle = 0;
        static double Vangle = 0;
        static pipelinevars pipe;
        public static Camera cam = new Camera();

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
            public Matrix4 model = Matrix4.Identity;

            public float speed = .3f;
            internal GameWindow win;
        }
        private static void Win_Load(object sender, EventArgs e)
        {

            //defin the shap to be drawn
            var cube = new CreateCube();
            pipe.vers = cube.points.SelectMany(o => o.data()).ToArray();
            pipe.indeces = cube.Indices;


            int vbo = pipe.vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo); //define the type of buffer in gpu memory
            //fill up the buffer with the data
            //we need to define the type of data to be filled and the size in the memory
            GL.BufferData(BufferTarget.ArrayBuffer, pipe.vers.Length * sizeof(float), pipe.vers, BufferUsageHint.StaticDraw);

            //element buffer
            pipe.ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, pipe.ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, pipe.indeces.Length * sizeof(int), pipe.indeces, BufferUsageHint.StaticDraw);

            //Element buffer object
            int vao = pipe.vao = GL.GenBuffer();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, pipe.vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, pipe.ebo);


            //load vertix/Fragment shader
            pipe.vershad = CreateShader(Shaders.VertexShaders.VShader(), ShaderType.VertexShader);
            pipe.fragshad = CreateShader(Shaders.FragmentShaders.TexFrag2Tex(), ShaderType.FragmentShader);


            //create program, link shaders and test the results
            int progid = CreatePrognLinkShader(pipe.vershad, pipe.fragshad);
            GL.UseProgram(progid);


            CreateShap();

            //load Textures
            pipe.texid1 = Textures.Textures.AddTexture(TextureUnit.Texture0, @"C:\Users\mosta\Downloads\container.jpg");

            pipe.texid2 = Textures.Textures.AddTexture(TextureUnit.Texture1, @"D:\My Book\layan photo 6x4.jpg");


        }

        class CreateCube
        {
            public List<Vertex> points { get; set; }

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


            }

            //define Indeces
            public int[] Indices { get; set; } =
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
        }




        static void CreateShap()
        {
            //freeup GPU
            //   cleanup();


            //Set MatrixTransformation            
            Shaders.VertexShaders.SetUniformMatrix(pipe.programId, "model", ref pipe.model);
            Shaders.VertexShaders.SetUniformMatrix(pipe.programId, nameof(Camera.View), ref cam.View);
            Shaders.VertexShaders.SetUniformMatrix(pipe.programId, nameof(Camera.Projection), ref cam.Projection);

            Textures.Textures.Link(TextureUnit.Texture0, pipe.texid1);
            Textures.Textures.Link(TextureUnit.Texture1, pipe.texid2);

            //tell GPU the loation of the textures in the shaders            
            Textures.Textures.SetUniform(pipe.programId, "texture0", 0);
            // Textures.Textures.SetUniform(pipe.programId, "texture1", 1);


            //since we are using vertex, opengl doesn't understand the specs of the vertex so we use vertexpointerattribute for this
            //now it is 5 instead of 3 for the texture coordinates
            var verloc = GL.GetAttribLocation(pipe.programId, "aPos");
            GL.VertexAttribPointer(0, Vertex3.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 0);
            GL.EnableVertexAttribArray(verloc); //now activate Vertexattrib

            //GetTexture coordinates
            var texloc = GL.GetAttribLocation(pipe.programId, "aTexCoord");
            GL.EnableVertexAttribArray(texloc);
            GL.VertexAttribPointer(texloc, Vertex2.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 3 * sizeof(float));

            //vertex Color
            var vercolloc = GL.GetAttribLocation(pipe.programId, "aVerColor");
            GL.EnableVertexAttribArray(vercolloc);
            GL.VertexAttribPointer(vercolloc, Vertex4.vcount, VertexAttribPointerType.Float, false, Vertex.vcount * sizeof(float), 5 * sizeof(float));
        }




        static void RenderShape(FrameEventArgs e)
        {
            //bind vertex object
            GL.BindVertexArray(pipe.vao);

            //orient Camera

            Shaders.VertexShaders.SetUniformMatrix(pipe.programId, nameof(cam.View), ref cam.View);
            Shaders.VertexShaders.SetUniformMatrix(pipe.programId, nameof(cam.Projection), ref cam.Projection);

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
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front); //set which face to be hidden            
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Fill); //set polygon draw mode

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
            cleanup();
        }

        static void cleanup()
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


            GL.DeleteProgram(pipe.programId);
        }

        private static void Win_UpdateFrame(object sender, FrameEventArgs e)
        {
            //clear the scean from any drawing before drawing
            GL.Clear(ClearBufferMask.ColorBufferBit);

            RenderShape(e);

            // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawElements(PrimitiveType.Triangles, pipe.indeces.Length, DrawElementsType.UnsignedInt, 0);
            //swap the buffer (bring what has been rendered in theback to the front)
            var win = (GameWindow)sender;
            win.SwapBuffers();
        }
    }
}
