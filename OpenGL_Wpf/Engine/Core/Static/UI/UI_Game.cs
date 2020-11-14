using ImGuiNET;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Geometry.ThreeDModels;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Scene;
using System;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Core.Static
{
    public static class UI_Game
    {
        private static Game game;

        private static bool isWindowOpen;

        public static void RenderUI(Game thisgame)
        {
            if (thisgame == null) return;
            game = thisgame;

            TotalHeight = 0;

            RenderWindow();
            Render_Status();
        }

        public static string StatusMessage = "";

        public static float TotalHeight = 50;

        internal static void Render_Exit()
        {
            cus_CMD cmd = new cus_CMD();
            Action<bool> responseAction = (x) =>
            {
                if (x)
                {
                    game.Exit();
                }
            };

            UI_Shared.Render_YesNOModalMessage("Exit", "Exit Engine?\r\nAny changes will not be saved.", responseAction);

            game.RenderOnUIThread(cmd);
        }

        private static void Render_FileMenu()
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.Button("Load Models"))
                {
                    game.LoadModels();
                }

                if (ImGui.Button("Save Models"))
                {
                    game.SaveModels();
                }
                ImGui.Separator();
                if (ImGui.Button("Import Models"))
                {
                    game.ImportModels();
                }
                ImGui.EndMenu();
            }
        }

        private static bool showDemo = false;
        private static int postProcessSelection = 0;

        private static void Render_MenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                Render_FileMenu();
                if (ImGui.BeginMenu("Demo"))
                {
                    ImGui.MenuItem("Show IMGUI Demo", "", ref showDemo, true);
                    Render_Geometry();
                    Render_PostProcess();
                    Render_Text();
                    Render_ColorChannel();

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }

            if (showDemo)
            {
                ImGui.ShowDemoWindow();
            }
        }

        private static void Render_Geometry()
        {
            if (ImGui.BeginMenu("Geometry"))
            {
                if (ImGui.MenuItem("Add Dragon"))
                {
                    GameFactory.DrawDragon(SceneModel.ActiveScene, null);
                }

                if (ImGui.MenuItem("Add Fog", "", ref SceneModel.ActiveScene.SceneFog.Active))
                {
                }

                if (ImGui.MenuItem("Add Terrain"))
                {
                    var terrain = GameFactory.Draw_Terran(SceneModel.ActiveScene) as Terran;
                }

                if (ImGui.MenuItem("Add Water"))
                {
                    GameFactory.Draw_Water(SceneModel.ActiveScene);
                    //GameFactory.DrawEarth(SceneModel.ActiveScene);
                }
                if (ImGui.MenuItem("Add Sky", "", SkyBox.ActiveSky != null))
                {
                    if (SkyBox.ActiveSky == null)
                    {
                        SkyBox.ActiveSky = GameFactory.DrawSkyBox(SceneModel.ActiveScene);
                    }
                    else
                    {
                        SceneModel.ActiveScene.RemoveModels(SkyBox.ActiveSky);
                        SkyBox.ActiveSky = null;
                    }
                }

                if (ImGui.MenuItem("Add Grid", "", Grid.ActiveGrid != null))
                {
                    if (Grid.ActiveGrid == null)
                    {
                        GameFactory.DrawGrid(SceneModel.ActiveScene);
                    }
                    else
                    {
                        SceneModel.ActiveScene.RemoveModels(Grid.ActiveGrid);
                        Grid.ActiveGrid = null;
                    }
                }

                ImGui.EndMenu();
            }
        }

        private static void Render_PostProcess()
        {
            if (ImGui.BeginMenu("PostProcess"))
            {
                ImGui.RadioButton("Normal", ref postProcessSelection, 0);
                ImGui.RadioButton("Contrast", ref postProcessSelection, 1);
                ImGui.RadioButton("Blur", ref postProcessSelection, 2);
                ImGui.RadioButton("Sepia", ref postProcessSelection, 3);

                if (postProcessSelection == 1)
                {
                    game.contrastEffect.IsActive = true;
                }
                else
                {
                    game.contrastEffect.IsActive = false;
                }

                if (postProcessSelection == 2)
                {
                    game.hBlureEffect.IsActive = true;
                }
                else
                {
                    game.hBlureEffect.IsActive = false;
                }

                if (postProcessSelection == 3)
                {
                    game.sepiaEffect.IsActive = true;
                }
                else
                {
                    game.sepiaEffect.IsActive = false;
                }

                ImGui.EndMenu();
            }
        }

        private static void Render_Text()
        {
            if (ImGui.BeginMenu("Text"))
            {
                if (ImGui.MenuItem("Render Text"))
                {
                    int linewidth = 300;
                    var gui = new Fonts.GuiFont("this is from UI", linewidth, 20);
                    gui.TextPosition = new OpenTK.Vector2(1 - (float)linewidth / 800);
                    gui.BuildModel();
                    var cmd = new cus_CMD();
                    cmd.Action = (a) =>
                    {
                        gui.Text = DisplayManager.UpdatePeriod.ToString();
                        gui.BuildModel();
                    };
                    game.RenderOnUIThread(cmd);
                    SceneModel.ActiveScene.GuiTextModel = gui;
                }
                ImGui.EndMenu();
            }
        }

        private static int ColorChannel = 0;

        private static void Render_ColorChannel()
        {
            if (ImGui.BeginMenu("Color Channel"))
            {
                ImGui.RadioButton("Normal Color", ref ColorChannel, 0);
                ImGui.RadioButton("Selection Buffer", ref ColorChannel, 1);
                ImGui.RadioButton("Vertex Buffer0", ref ColorChannel, 2);
                ImGui.RadioButton("Vertex Buffer1", ref ColorChannel, 3);
                ImGui.RadioButton("Vertex Buffer2", ref ColorChannel, 4);

                if (ColorChannel == 0)
                {
                    CameraModel.ActiveCamera.ColorChannel = OpenTK.Graphics.OpenGL.ReadBufferMode.ColorAttachment0;
                }

                if (ColorChannel == 1)
                {
                    CameraModel.ActiveCamera.ColorChannel = OpenTK.Graphics.OpenGL.ReadBufferMode.ColorAttachment1;
                }

                if (ColorChannel == 2)
                {
                    CameraModel.ActiveCamera.ColorChannel = OpenTK.Graphics.OpenGL.ReadBufferMode.ColorAttachment2;
                }
                if (ColorChannel == 3)
                {
                    CameraModel.ActiveCamera.ColorChannel = OpenTK.Graphics.OpenGL.ReadBufferMode.ColorAttachment3;
                }
                if (ColorChannel == 4)
                {
                    CameraModel.ActiveCamera.ColorChannel = OpenTK.Graphics.OpenGL.ReadBufferMode.ColorAttachment4;
                }

                ImGui.EndMenu();
            }
        }

        internal static void DisplayStatusmMessage(string msg, int v)
        {
            StatusMessage = msg;
            Task.Run(async () =>
            {
                while (v > 0)
                {
                    v -= 1000;
                    await Task.Delay(1000);
                }
                StatusMessage = "";
            });
        }

        private static void RenderWindow()
        {
            float height = 25;

            ImGui.SetNextWindowSize(new System.Numerics.Vector2(game.Width, height));
            ImGui.SetNextWindowPos(new System.Numerics.Vector2());
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            if (ImGui.Begin("Game", ref isWindowOpen,
            ImGuiWindowFlags.MenuBar |
            ImGuiWindowFlags.NoTitleBar |
               ImGuiWindowFlags.NoResize |
             ImGuiWindowFlags.NoBackground |
            ImGuiWindowFlags.NoMove))
            {
                Render_MenuBar();

                ImGui.End();
            }
            ImGui.PopStyleVar();
        }

        private static void Render_Status()
        {
            float height = 25;
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(game.Width, height));
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, game.Height - height));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            if (ImGui.Begin("StatusBar", ref isWindowOpen,
            ImGuiWindowFlags.NoTitleBar |
            ImGuiWindowFlags.NoMove |
             ImGuiWindowFlags.NoResize
            ))
            {
                ImGui.Text($"Status: {StatusMessage}");

                ImGui.End();
            }
            ImGui.PopStyleVar();
        }
    }
}