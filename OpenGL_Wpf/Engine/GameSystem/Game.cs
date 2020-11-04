using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Core.AnimationSystem;
using Simple_Engine.Engine.Illumination.Render;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Engine.Water.Render;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple_Engine.Engine.GameSystem
{
    public partial class Game : GameWindow
    {
        public static Game Instance;

        public Game(int width, int height, string title) : base(width, height, new GraphicsMode(ColorFormat.Empty, 24, 8, 0), title
                                , GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.Default
        )
        {
            WCF_System.Wcf_Engine.Start_Wcf_Engine();

            GameDebuger.DebugMode();
            DisplayManager.Initialize(width, height);

            Instance = this;

            SceneModel.ActiveScene = new SceneModel(this);
            SceneModel.ActiveScene.BuildModel();

            Setup_Events();
            SetupFBOs();
        }

        protected override void OnLoad(EventArgs e)
        {
            SceneModel.ActiveScene.FBOs.Add(new Shadow_FBO(SceneModel.ActiveScene.Lights.First(), 1024, 1024));
            GameFactory.Draw_Water(SceneModel.ActiveScene);
            GameFactory.Draw_Terran(SceneModel.ActiveScene);

            //render this the last to prevent interfering with other frame buffers. Fixes issue #6
            Setup_GameUI();

            base.OnLoad(e);
        }

        private Stack<Action> OnUIThreadActions = new Stack<Action>();
        private List<cus_CMD> RenderOnUIThreadActions = new List<cus_CMD>();

        public void RunOnUIThread(Action action)
        {
            OnUIThreadActions.Push(action);
        }

        public void RenderOnUIThread(cus_CMD action)
        {
            RenderOnUIThreadActions.Add(action);
        }

        public void Dispose_RenderOnUIThread(cus_CMD action)
        {
            RenderOnUIThreadActions.Remove(action);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            SceneModel.ActiveScene.PrepareForRender(null);

            foreach (var fbo in SceneModel.ActiveScene.FBOs)
            {
                if (fbo.Name == FBO.FboName.undefined) continue;
                fbo.BindFrameBuffer();
                fbo.ClearFrame();

                fbo.RenderFrame(SceneModel.ActiveScene.geoModels);
                fbo.UnbindCurrentBuffer();
            }

            UpdateUI((float)e.Time);

            //texture_FBO.BindFrameBuffer();
            //texture_FBO.ClearFrame();
            mTargets_FBO.BindFrameBuffer();
            mTargets_FBO.ClearFrame();
            //GL.Enable(EnableCap.StencilTest);
            //GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

            SceneModel.ActiveScene.Render();

            mTargets_FBO.UnbindCurrentBuffer();
            //change Color Attachment to 01 to render the selection buffer
            mTargets_FBO.ResolveResults(0, ReadBufferMode.ColorAttachment0);

            //texture_FBO.UnbindCurrentBuffer();

            // contrastEffect.ProcessEffect(mTargets_FBO.Color00BufferId);

            //hBlureEffect.ProcessEffect(texture_FBO.TextureAttachId);
            //contrastEffect.ProcessEffect(texture_FBO.TextureAttachId);

            // sepiaEffect.ProcessEffect(texture_FBO.TextureAttachId);
            // ActiveScene.GuiTextModel?.Render();

            DisplayManager.FixTime();
            AnimationMaster.Render(DisplayManager.UpdatePeriod);
            while (OnUIThreadActions.Count != 0)
            {
                OnUIThreadActions.Pop().Invoke();
            }
            for (int i = 0; i < RenderOnUIThreadActions.Count; i++)
            {
                RenderOnUIThreadActions[i].Execute(null);
            }

            RenderUI();
            base.Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            SceneModel.ActiveScene.Dispose();
            _controller?.Dispose();
            base.OnUnload(e);

            WCF_System.Wcf_Engine.Stop_Wcf_Engine();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        private void ClearFrame()
        {
            GL.ClearColor(.2f, .2f, .2f, 1);
            GL.ClearStencil(0);
            GL.StencilMask(0);
            GL.Clear(
            ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit
            );
        }

        private void injectModels()
        {
            //var gui = new GuiModel(.25f, .25f, 0.5f, 0);
            //gui.BuildModel();
            //gui.ShaderModel = new Shader(ShaderMapType.Blend, ShaderPath.GUI);
            //gui.ShaderModel.EnableInstancing = true;
            //ActiveScene.ModelsforUpload.Add(gui);

            //All lights must be placed first
            //gameFactory.DrawSkyBox();

            //   gameFactory.DrawStreetLamp(terrain);

            //gameFactory.Draw_Pushes(terrain);

            //var earth = gameFactory.DrawEarth();
            //earth.CullMode = CullFaceMode.FrontAndBack;

            //Draw_Hilbert();

            //gameFactory.Draw_Rectangle();

            //var cube = gameFactory.DrawCube();
            // cube.DrawAxis();

            //cube.Move(new Vector3(0, 1, 0));
            //cube.Scale(new Vector3(1,-1,1));
            //var pos = cube.LocalTransform.ExtractTranslation();
            //cube.Move(new Vector3(0, 2 * pos.Y , 0));
            //cube.Normals = cube.Normals.Negate(new Vector3(1, -1, 1));
            //cube.CullMode = CullFaceMode.Front;
            //cube.ScaleAroundPoint(new Vector3(1, -1, 1), new Vector3());
            //cube.Normals = cube.Normals.Negate(new Vector3(1, -1, 1));
            //cube.CullMode = CullFaceMode.Back;

            //   gameFactory.DrawSimple_EngineGeometry("");
            //  gameFactory.DrawWater();

            //  gameFactory.DrawText();
        }
    }
}

/* Read pixrl color
*  Bitmap b = new Bitmap(Width, Height);
   var bits = b.LockBits(new System.Drawing.Rectangle(0, 0, 1, 1), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
   GL.ReadPixels(e.X, e.Y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bits.Scan0);
* */

#if false
        private void MoveLamp(Terran terrain, Vector3 mouseRayVector)
        {
            float range = 600;
            //Vector3 startPoint = scene.ActiveCamera.ViewTransform.ExtractTranslation();
            Vector3 startPoint = getPointOnRay(mouseRayVector, 0);
            Vector3 endPoint = getPointOnRay(mouseRayVector, range);

            bool isInRange = IsLineInRange(terrain, startPoint, endPoint);
            if (isInRange)
            {
                var point = GetPoint(terrain, 0, 0, 600, mouseRayVector);
                Debug.WriteLine($"Height: {point}");

                var lamp = ActiveScene.geos.FirstOrDefault(o => o is StreetLamp) as StreetLamp;
                var mesh = lamp.Meshes.First();
                mesh.MoveTo(point);
            }
        }

        private static bool IsLineInRange(Terran terrain, Vector3 startPoint, Vector3 endPoint)
        {
            //check if points intersects terrain
            var above = terrain.GetTerrainHeight(startPoint.X, startPoint.Z);
            var under = terrain.GetTerrainHeight(endPoint.X, endPoint.Z);
            bool isInRange = above < startPoint.Y && under > endPoint.Y;
            return isInRange;
        }

        private Vector3 GetPoint(Terran terrain, int trials, float start, float end, Vector3 mouseRayVector)
        {
            var half = start + ((end - start) / 2);
            if (trials >= 600)
            {
                var endPoint = getPointOnRay(mouseRayVector, half);
                return endPoint;
            }

            if (IsLineInRange(terrain, getPointOnRay(mouseRayVector, start), getPointOnRay(mouseRayVector, end)))
            {
                return GetPoint(terrain, trials + 1, start, half, mouseRayVector);
            }
            else
            {
                return GetPoint(terrain, trials + 1, half, end, mouseRayVector);
            }
        }

        private Vector3 getPointOnRay(Vector3 ray, float distance)
        {
            //Vector3 camPos = scene.ActiveCamera.ViewTransform.ExtractTranslation();
            Vector3 camPos = ActiveScene.ActiveCamera.Position;
            Vector3 start = new Vector3(camPos.X, camPos.Y, camPos.Z);
            Vector3 scaledRay = new Vector3(ray.X * distance, ray.Y * distance, ray.Z * distance);
            return Vector3.Add(start, scaledRay);
        }

#endif