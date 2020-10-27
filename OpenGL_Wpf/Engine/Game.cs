using ImGuiNET;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Fonts;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Geometry;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Geometry.ThreeDModels;
using Simple_Engine.Engine.Geometry.TwoD;
using Simple_Engine.Engine.GUI;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.Illumination.Render;
using Simple_Engine.Engine.ImGui_Set;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Primitives;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space;
using Simple_Engine.Engine.Space.Render.PostProcess;
using Simple_Engine.Engine.Water.Render;
using Simple_Engine.Extentions;
using Simple_Engine.ToolBox;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Shared_Lib;
using Shared_Lib.Extention;
using Shared_Lib.IO;
using Shared_Library.Extention;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace Simple_Engine.Engine
{
    public class Game : GameWindow
    {
        public static Game Context;

        public Scene ActiveScene;

        public Game_FBOs gameFbos;

        public Game_Events gameEvents;

        public Game_UI uiGame;

        public Game(int width, int height, string title) : base(width, height, new GraphicsMode(ColorFormat.Empty, 24, 8, 0), title
                                , GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.Default
        )
        {
            GameDebuger.DebugMode();
            DisplayManager.Initialize();

            Context = this;

            ActiveScene = new Scene(this);
            ActiveScene.BuildModel();

            uiGame = new Game_UI(this);
            gameEvents = new Game_Events(this);
            gameFbos = new Game_FBOs(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            var terrain = GameFactory.Draw_Terran(ActiveScene) as Terran;
            terrain.IsSystemModel = true;
            GameFactory.DrawDragon(ActiveScene, terrain);
            ActiveScene.FBOs.Add(new Shadow_FBO(ActiveScene.Lights.First(), 1024, 1024));

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            uiGame.Update((float)e.Time);
            ActiveScene.PrepareForRender(null);
            foreach (var fbo in ActiveScene.FBOs)
            {
                if (fbo.Name == FBO.FboName.undefined) continue;
                fbo.BindFrameBuffer();
                fbo.ClearFrame();

                fbo.RenderFrame(ActiveScene.geoModels);
                fbo.UnbindCurrentBuffer();
            }

            //texture_FBO.BindFrameBuffer();
            //texture_FBO.ClearFrame();
            gameFbos.mTargets_FBO.BindFrameBuffer();
            gameFbos.mTargets_FBO.ClearFrame();
            //GL.Enable(EnableCap.StencilTest);
            //GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

            ClearFrame();

            ActiveScene.Render();

            gameFbos.mTargets_FBO.UnbindCurrentBuffer();
            gameFbos.mTargets_FBO.ResolveResults(0, ReadBufferMode.ColorAttachment0);

            //texture_FBO.UnbindCurrentBuffer();

            // contrastEffect.ProcessEffect(mTargets_FBO.Color00BufferId);

            //hBlureEffect.ProcessEffect(texture_FBO.TextureAttachId);
            //contrastEffect.ProcessEffect(texture_FBO.TextureAttachId);

            // sepiaEffect.ProcessEffect(texture_FBO.TextureAttachId);
            // ActiveScene.GuiTextModel?.Render();

            ImGui.ShowDemoWindow();
            DisplayManager.FixTime();
            uiGame.Render();

            base.Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            CameraModel.ActiveCamera.Height = Height;
            CameraModel.ActiveCamera.Width = Width;
            CameraModel.ActiveCamera.ActivatePrespective();

            //Update OtherFrames
            gameFbos.mTargets_FBO.UpdateSize(Width, Height);

            // Tell ImGui of the new size
            uiGame.UpdateSize();
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            ActiveScene.Dispose();
            uiGame.Dispose();
            base.OnUnload(e);
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

        private void LoadModels()
        {
            //var gui = new GuiModel(.25f, .25f, 0.5f, 0);
            //gui.BuildModel();
            //gui.ShaderModel = new Shader(ShaderMapType.Blend, ShaderPath.GUI);
            //gui.ShaderModel.EnableInstancing = true;
            //ActiveScene.ModelsforUpload.Add(gui);

            //All lights must be placed first
            //gameFactory.DrawSkyBox();

            Terran terrain = null;
            terrain = GameFactory.Draw_Terran(ActiveScene) as Terran;
            //var dragon = gameFactory.DrawDragon(terrain);

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