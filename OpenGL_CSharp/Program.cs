using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenGL_CSharp.Geometery;
using OpenGL_CSharp.Graphic;
using OpenGL_CSharp.Shaders;
using OpenGL_CSharp.Shaders.Light;
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
            win.UpdateFrame += Win_UpdateFrame; //on each frame do this     

            //Navigate setting
            //----------------
            win.Closing += Win_Closing; //on termination do this
            win.KeyDown += Win_KeyDown; //keydown event
            win.MouseWheel += Win_MouseWheel;

            //start game window
            win.Run(15);
        }

        private static void SetupScene(GameWindow win)
        {
            //intialize holder for the project main variables
            pipe = new Pipelinevars();
            //defin viewport size
            GL.Viewport(100, 100, 700, 700);
            GL.ClearColor(0, 0, .18f, 1);//set background color
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.CullFace(CullFaceMode.Back); //set which face to be hidden            
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill); //set polygon draw mode
            GL.Enable(EnableCap.DepthTest);
        }

        #region Navigation
        private static void Win_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            cam.Fov(e.DeltaPrecise);
            cam.updateCamera();
        }

        private static void Win_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            var win = (GameWindow)sender;

            Hangle = Math.Atan2(cam.Position.X, cam.Position.Z);
            Vangle = Math.Atan2(cam.Position.Z, cam.Position.Y);

            if (e.Key == Key.F)
            {
                light.lightPosition += new Vector3(-1,0,0);
            }

            if (e.Key == Key.H)
            {
                light.lightPosition += new Vector3(1,0,0);
            }
            if (e.Key == Key.T)
            {
                light.lightPosition += new Vector3(0,1,0);
            }
            if (e.Key == Key.G)
            {
                light.lightPosition += new Vector3(0,-1,0);
            }

            if (e.Key == Key.Z || e.Key == Key.X)
            {
                if (e.Key == Key.Z)
                {
                    BaseGeometry.specintens -= 5f;
                }
                else
                {
                    BaseGeometry.specintens += 5f;
                }

                pipe.geos.ForEach(o =>
                {
                    BaseGeometry.specintens = BaseGeometry.specintens < 0 ? 0.1f : BaseGeometry.specintens;

                });
            }

            if (e.Key == OpenTK.Input.Key.Right)
            {

                var oldr = cam.Position.Length;
                cam.Position += new Vector3(1.1f, 0f, 0); ;
                r += cam.Position.Length - r;
            }
            if (e.Key == OpenTK.Input.Key.Left)
            {
                var oldr = cam.Position.Length;
                cam.Position += new Vector3(-1.1f, 0f, 0); ;
                r += cam.Position.Length - r;
            }

            if (e.Key == OpenTK.Input.Key.Down)
            {
                cam.Position += new Vector3(0, -1.1f, 0);
            }

            if (e.Key == OpenTK.Input.Key.Up)
            {
                cam.Position += new Vector3(0, 1.1f, 0);
            }

            if (e.Key == OpenTK.Input.Key.Escape)
            {
                ((GameWindow)sender).Close();
            }

            if (e.Key == OpenTK.Input.Key.W)
            {
                Vangle += inrement;

                cam.Position = new Vector3(cam.Position.X, (float)Math.Cos(Vangle) * r, (float)Math.Sin(Vangle) * r);

            }

            if (e.Key == OpenTK.Input.Key.S)
            {
                Vangle -= inrement;

                cam.Position = new Vector3(cam.Position.X, (float)Math.Cos(Vangle) * r, (float)Math.Sin(Vangle) * r);

            }

            if (e.Key == OpenTK.Input.Key.A)
            {
                Hangle -= inrement;

                cam.Position = new Vector3((float)Math.Sin(Hangle) * r, cam.Position.Y, (float)Math.Cos(Hangle) * r);

            }

            if (e.Key == OpenTK.Input.Key.D)
            {
                Hangle += inrement;

                cam.Position = new Vector3((float)Math.Sin(Hangle) * r, cam.Position.Y, (float)Math.Cos(Hangle) * r);
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
        static float inrement = 0.1744f;
        static double Hangle = 0;
        static double Vangle = 0;
        #endregion

        public static Camera cam = new Camera();
        public static Pipelinevars pipe; //just global class for all required variables
        public class Pipelinevars
        {
            public float offsetX = 0.5f;
            public float speed = .3f;
            internal GameWindow win;
            public List<Geometery.BaseGeometry> geos = new List<BaseGeometry>();
        }

        static LightSource light = new LightSource();

        private static void Win_Load(object sender, EventArgs e)
        {
            r = cam.Position.Length; //update the current distance from the camera to position 0

            //Creaate Light Source
            light.Direction = new Vector3(0, 0, 1);
            //light.lightPosition= new Vector3(.5f, 0.0f, 1f); 

            //Light Source Geometry
            var pyr = new Cube();
            pipe.geos.Add(pyr);
            pyr.model = pyr.model * Matrix4.CreateTranslation(0.75f, 0f, 0f);
            pyr.LoadGeometry();

            pyr.shader = new LampFrag();
            ((LampFrag)pyr.shader).LoadLampPointFragment();

            pyr.shader.light = light;
            light.lightPosition = pyr.model.ExtractTranslation();

            Random rn = new Random(5);
            for (int i = 0; i < 10; i++)
            {
                var shade = new Tex2Frag(new Vector3(1), @"Textures\container.jpg", @"Textures\container_specular.jpg"); ;

                //defin the shap to be drawn             
                var cube = new Cube();
                pipe.geos.Add(cube);
                var ang = (float)Math.Cos(i * 20) + rn.Next(-10, 10);
                var ang1 = (float)Math.Cos(i * 30) + rn.Next(-10, 10);
                var ang2 = (float)Math.Cos(i * 40) + rn.Next(-10, 10);
                cube.model *= Matrix4.CreateTranslation(-0.75f * ang, .2f * ang1, .3f * ang2);
                cube.LoadGeometry();
                cube.shader = shade;

                cube.shader.light = light;
                cube.shader.light.ambient = new Vector3(.1f); //decrease the effect of the ambient for the materialed models
                //if light source is point light
                //cube.shader.light.Direction = pyr.shader.light.lightPosition - cube.model.ExtractTranslation();

            }

        }

        private static void Win_UpdateFrame(object sender, FrameEventArgs e)
        {
            //clear the scean from any drawing before drawing
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit); //this is required to redraw all the depth changes due to camera/View/Object movement

            for (int i = 0; i < pipe.geos.Count; i++)
            {
                var geo = pipe.geos[i];
                geo.RenderGeometry();
                // geo.shader.Use();





                GL.DrawElements(PrimitiveType.Triangles, geo.Indeces.Length, DrawElementsType.UnsignedInt, 0);
            }

            //swap the buffer (bring what has been rendered in theback to the front)
            pipe.win.SwapBuffers();
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
                var o = pipe.geos[i];
                GL.DeleteBuffer(o.vbo);
                GL.DeleteVertexArray(o.ebo);
                GL.DeleteVertexArray(o.vao);

                o.Dispose();
            }
        }
    }
}
