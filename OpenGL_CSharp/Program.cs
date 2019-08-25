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
            GL.Viewport(0, 0, 800, 800);
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
            if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft))
            {
                cam.Target += new Vector3(0, e.DeltaPrecise, 0);
            }
            else
            {
                cam.Fov(e.DeltaPrecise);
            }
            cam.updateCamera();
        }

        private static void Win_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            var win = (GameWindow)sender;

            Hangle = Math.Atan2(cam.Position.X, cam.Position.Z);
            Vangle = Math.Atan2(cam.Position.Z, cam.Position.Y);

            if (e.Key == Key.V)
            {
                pipe.geos.ForEach(o => o.shader.IsBlin = !o.shader.IsBlin);
            }

            if (e.Key == Key.Z || e.Key == Key.X)
            {

                if (e.Key == Key.Z)
                {
                    pipe.geos.ForEach(o => o.shader.specintens -= 1f);

                }
                else
                {
                    pipe.geos.ForEach(o => o.shader.specintens += 1f);

                }


            }

            if (e.Key == OpenTK.Input.Key.Right)
            {

                var oldr = cam.Position.Length;
                cam.Target += new Vector3(.5f, 0f, 0); ;
                r += cam.Position.Length - r;
            }
            if (e.Key == OpenTK.Input.Key.Left)
            {
                var oldr = cam.Position.Length;
                cam.Target += new Vector3(-.5f, 0f, 0); ;
                r += cam.Position.Length - r;
            }

            if (e.Key == OpenTK.Input.Key.Down)
            {
                cam.Target += new Vector3(0, -.5f, 0);
            }

            if (e.Key == OpenTK.Input.Key.Up)
            {
                cam.Target += new Vector3(0, .5f, 0);
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
                //  cam.Target = Vector3.Zero;
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


        


        static private Matrix4 FromMatrix(Assimp.Matrix4x4 mat)
        {
            Matrix4 m = new Matrix4();
            m.M11 = mat.A1;
            m.M12 = mat.A2;
            m.M13 = mat.A3;
            m.M14 = mat.A4;
            m.M21 = mat.B1;
            m.M22 = mat.B2;
            m.M23 = mat.B3;
            m.M24 = mat.B4;
            m.M31 = mat.C1;
            m.M32 = mat.C2;
            m.M33 = mat.C3;
            m.M34 = mat.C4;
            m.M41 = mat.D1;
            m.M42 = mat.D2;
            m.M43 = mat.D3;
            m.M44 = mat.D4;
            return m;

        }
        private static void Win_Load(object sender, EventArgs e)
        {
            r = cam.Position.Length; //update the current distance from the camera to position 0
            cam.Target = new Vector3(0, 1, 0);
            //Create Light Source
            List<LightSource> lightSources = LightSource. SetupLights();

            Assimp.AssimpContext imp = new Assimp.AssimpContext();
            imp.SetConfig(new Assimp.Configs.NormalSmoothingAngleConfig(66f));
            var scene = imp.ImportFile("Models/untitled.obj", Assimp.PostProcessSteps.Triangulate | Assimp.PostProcessSteps.FlipUVs);

            var model = FromMatrix(scene.RootNode.Transform);
            //  model.Transpose();
            var srs = LightSource.SetupLights();

			scene.Meshes.ForEach(m =>
            {
                var duck = new BaseGeometry();

                duck.points = new List<Vertex>();
                duck.Indeces = new List<int>();

                for (int j = 0; j < m.FaceCount; j++)
                {
                    var f = m.Faces[j];
                    for (int i = 0; i < f.IndexCount; i++)
                    {
                        var ind = f.Indices[i];
                        duck.Indeces.Add(ind);

                        var ver = Vertex.FromVertex3(m.Vertices[ind]);
                        var normal = Vertex.FromVertex3(m.Normals[ind]);
                        var text = new Vertex2(0, 0);
                        if (m.HasTextureCoords(0))
                        {
                            text = Vertex.FromVertex2(m.TextureCoordinateChannels[0][ind]);
                        }

                        var vcol = new Vertex4(1f, .5f, 0f, 1f);
                        if (m.HasVertexColors(ind))
                            vcol = Vertex.FromVertex4(m.VertexColorChannels[0][ind]);

                        duck.points.Add(new Vertex()
                        {
                            Normal = normal,
                            Position = ver,
                            TexCoor = text,
                            Vcolor = vcol
                        });

                        Debug.WriteLine($"\r\n\r\n--------------\r\nFace: {j}  - Ind: {ind}\r\n{duck.points.Count - 1}: {duck.points.Last().ToString()}");
                    }


                }

                duck.LoadGeometry();
              //  pipe.geos.Add(duck);
              //  duck.model = model;
                duck.objectColor = new Vector3(1f, 0f, 1f);
                duck.shader = new Tex2Frag(duck.objectColor);
                
                duck.shader.LightSources = lightSources;

            });

            var plan = new Plan();
            pipe.geos.Add(plan);
            plan.LoadGeometry();
            plan.model *= Matrix4.CreateTranslation(new Vector3(0, 0f, 0)) * Matrix4.CreateScale(22f);
            plan.shader = new Tex2Frag(@"Textures\container.jpg", @"Textures\container_specular.jpg");
            plan.shader.specintens = 3;
            plan.shader.LightSources = lightSources;

#if true

            //Light Source Geometry
            var pyr = new Cube();
            // pipe.geos.Add(pyr);           
            pyr.LoadGeometry();

            pyr.shader = new LampFrag();
            ((LampFrag)pyr.shader).LoadLampPointFragment();

            Random rn = new Random(5);
            for (int i = 0; i < 10; i++)
            {
                var shade = new Tex2Frag(@"Textures\container.jpg", @"Textures\container_specular.jpg"); ;

                //defin the shap to be drawn             
                var cube = new Cube();
                pipe.geos.Add(cube);
                var ang = (float)Math.Cos(i * 20) + rn.Next(-10, 10);
                var ang1 = (float)Math.Cos(i * 30) + rn.Next(-10, 10);
                var ang2 = (float)Math.Cos(i * 40) + rn.Next(-10, 10);
                cube.model *= Matrix4.CreateTranslation(-0.75f * ang, .1f * ang1, .2f * ang2);
                cube.LoadGeometry();
                cube.shader = shade;

                cube.shader.LightSources = lightSources;

            }
#endif


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

                GL.DrawElements(PrimitiveType.Triangles, geo.Indeces.ToArray().Length, DrawElementsType.UnsignedInt, 0);
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
