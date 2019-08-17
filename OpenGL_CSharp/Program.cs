using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_CSharp.Geometery;
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
        public static pipelinevars pipe;
        public static Camera cam = new Camera();

        public class pipelinevars
        {
            public int programId;

            public float offsetX = 0.5f;
            public float speed = .3f;
            internal GameWindow win;

            public List<Geometery.BaseGeometry> geos = new List<BaseGeometry>();
        }
        private static void Win_Load(object sender, EventArgs e)
        {
            setupGeoStructure();
        }

        static void setupGeoStructure()
        {
            //defin the shap to be drawn
            // pipe.geos.Add(new Pyramid());
            pipe.geos.Add(new CreateCube());
            pipe.geos.Add(new CreateCube());

            pipe.geos[0].model = pipe.geos[0].model * Matrix4.CreateTranslation(-0.75f, 0f, 0f);
            pipe.geos[1].model = pipe.geos[1].model * Matrix4.CreateTranslation(0.75f, 0f, 0f);
             
        }



        private static void SetupScene(GameWindow win)
        {
            //intialize holder for the project main variables
            pipe = new pipelinevars();
            //defin viewport size
            GL.Viewport(100, 100, 700, 700);
            GL.ClearColor(Color.CornflowerBlue);//set background color
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.CullFace(CullFaceMode.Back); //set which face to be hidden            
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill); //set polygon draw mode
            GL.Enable(EnableCap.DepthTest);
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
            for (int i = 0; i < pipe.geos.Count; i++)
            {
                GL.DeleteBuffer(pipe.geos[i].vbo);
                GL.DeleteVertexArray(pipe.geos[i].vao);
                GL.DeleteShader(pipe.geos[i].vershad);
                GL.DeleteShader(pipe.geos[i].fragshad);
            }



            GL.DeleteProgram(pipe.programId);
        }

        static void RenderShape(FrameEventArgs e, int i)
        {
            //bind vertex object
            GL.BindVertexArray(pipe.geos[i].vao);

            //orient Camera, MatrixTransformation
            Shaders.VertexShaders.SetUniformMatrix(pipe.programId, nameof(BaseGeometry.model), ref pipe.geos[i].model);
            Shaders.VertexShaders.SetUniformMatrix(pipe.programId, nameof(cam.View), ref cam.View);
            Shaders.VertexShaders.SetUniformMatrix(pipe.programId, nameof(cam.Projection), ref cam.Projection);

            //Use vertix shaders holder to the GPU memory
            //--------------
            Textures.Textures.Link(TextureUnit.Texture0, pipe.geos[i].texid1);
            // Textures.Textures.Link(TextureUnit.Texture1, pipe.texid2);

            GL.UseProgram(pipe.programId);

        }

        private static void Win_UpdateFrame(object sender, FrameEventArgs e)
        {
            //clear the scean from any drawing before drawing
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);


            for (int i = 0; i < pipe.geos.Count; i++)
            {
                RenderShape(e, i);
                GL.DrawElements(PrimitiveType.Triangles, pipe.geos[i].Indeces.Length, DrawElementsType.UnsignedInt, 0);
                GL.BindVertexArray(0);
            }

            //swap the buffer (bring what has been rendered in theback to the front)
            pipe.win.SwapBuffers();


        }
    }
}
