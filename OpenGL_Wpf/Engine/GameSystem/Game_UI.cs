using ImGuiNET;
using Simple_Engine.Engine.Core.Serialize;
using Simple_Engine.Engine.ImGui_Set;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Space;
using OpenTK;
using Shared_Lib.Extention;
using Shared_Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Engine.Space.Camera;

namespace Simple_Engine.Engine.GameSystem
{
    public class Game_UI
    {
        private Game game;
        private ImGuiController _controller;
        private Imgui_ToolTip imgui_tooltip;

        public ImgUI_Controls ui_Control { get; private set; }
        internal Imgui_InputFloat Imgui_Counter { get; private set; }

        public Game_UI(Game game)
        {
            this.game = game;
            Generate_UIControls();
            game.KeyPress += Game_KeyPress;
        }

        private void Game_KeyPress(object sender, KeyPressEventArgs e)
        {
            _controller.PressChar(e.KeyChar);
        }

        private void Generate_UIControls()
        {
            _controller = new ImGuiController(game.Width, game.Height);
            ui_Control = new Imgui_Window("Game");
            var imguibar = new Imgui_MenuBar(ui_Control);
            var filemenu = new Imgui_MenuItem(imguibar, "File");

            Create_SaveCamera(filemenu);
            Create_LoadCamera(filemenu);
            Create_SaveScene(filemenu);
            Create_LoadScene(filemenu);
            Create_SaveModels(filemenu);
            Create_ImportModels(filemenu);
        }

        private void Create_LoadCamera(Imgui_MenuItem filemenu)
        {
            var imgui_SaveModelsbutton = new Imgui_Button(filemenu, "Load Camera", (x) =>
            {
                string path = UT_System.LoadFiles(Core.Serialize.Import.GetFilter(Core.Serialize.Import.filter.Simple_EngineModel)).FirstOrDefault();
                if (path != null)
                {
                    var cam = CameraModel.ActiveCamera.Load(path) as CameraModel;
                    CameraModel.ActiveCamera.AnimateCamrea(cam.Position, cam.Target);
                }
                ImGui.CloseCurrentPopup();
            });
        }

        private void Create_SaveCamera(Imgui_MenuItem filemenu)
        {
            var imgui_SaveModelsbutton = new Imgui_Button(filemenu, "Save Camera", (x) =>
            {
                string path = UT_System.SaveFilePath(Core.Serialize.Import.GetFilter(Core.Serialize.Import.filter.Simple_EngineModel));
                if (path == null) return;
                if (!path.EndsWith(".ssd"))
                    path += ".ssd";
                if (path != null)
                {
                    CameraModel.ActiveCamera.Save(path);
                }
                ImGui.CloseCurrentPopup();
            });
        }

        private void Create_ImportModels(Imgui_MenuItem filemenu)
        {
            var imgui_Importbutton = new Imgui_Button(filemenu, "Import Model",
                        (x) =>
                        {
                            SceneModel.ActiveScene.DisposeModels();

                            string path = UT_System.LoadFiles(Core.Serialize.Import.GetFilter(Core.Serialize.Import.filter.Simple_EngineModel)).FirstOrDefault();

                            if (path != null)
                            {
                                GameFactory.DrawSimple_EngineGeometry(SceneModel.ActiveScene, path);
                            }

                            ImGui.CloseCurrentPopup();
                        });
        }

        private void Create_SaveModels(Imgui_MenuItem filemenu)
        {
            var imgui_SaveModelsbutton = new Imgui_Button(filemenu, "Save Models", (x) =>
            {
                string path = UT_System.SaveFilePath(Core.Serialize.Import.GetFilter(Core.Serialize.Import.filter.Simple_EngineModel));
                if (path != null)
                {
                    Core.Serialize.IO.Save(SceneModel.ActiveScene.geoModels, path);
                }
                ImGui.CloseCurrentPopup();
            });
        }

        private void Create_LoadScene(Imgui_MenuItem filemenu)
        {
            var imgui_ImportScenebutton = new Imgui_Button(filemenu, "Load Scene", (x) =>
            {
                var msg = new Imgui_PopModalWindow(ui_Control, "WIP", "Work in progress", () => true, (x) => { });
            });
        }

        private void Create_SaveScene(Imgui_MenuItem filemenu)
        {
            var imgui_Savebutton = new Imgui_Button(filemenu, "Save Scene", (x) =>
            {
                var msg = new Imgui_PopModalWindow(ui_Control, "WIP", "Work in progress", () => true, (x) => { });
            });
        }

        internal void Update(float time)
        {
            _controller?.Update(game, time);
        }

        internal void Dispose()
        {
            _controller?.Dispose();
        }

        internal void Render()
        {
            game.gameEvents.OnRenderingUI();

            ui_Control.BuildModel();
            _controller?.Render();
        }

        internal void UpdateSize()
        {
            _controller?.WindowResized(game.Width, game.Height);
        }
    }
}