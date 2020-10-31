using ImGuiNET;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.GameSystem;
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
                game.Dispose_RenderOnUIThread(cmd);
            };

            cmd.Action = (x) =>
            {
                string title = "Exit";
                ImGui.OpenPopup(title);
                UI_Shared.Render_YesNOModalMessage(title, "Exit Engine?\r\nAny changes will not be saved.", responseAction);
            };
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

        private static void Render_MenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                Render_FileMenu();
                if (ImGui.BeginMenu("Tools"))
                {
                    ImGui.MenuItem("Show Demo", "", ref showDemo, true);

                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }

            if (showDemo)
            {
                ImGui.ShowDemoWindow();
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
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            if (ImGui.Begin("Game", ref isWindowOpen,
            ImGuiWindowFlags.MenuBar |
            ImGuiWindowFlags.NoTitleBar |
               ImGuiWindowFlags.NoResize|
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