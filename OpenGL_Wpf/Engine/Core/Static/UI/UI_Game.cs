using ImGuiNET;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.GameSystem;
using System;

namespace Simple_Engine.Engine.Core.Static
{
    public static class UI_Game
    {
        private static Game game;

        public static void RenderUI(Game thisgame)
        {
            if (thisgame == null) return;
            game = thisgame;
          
            RenderWindow();
        }

        private static bool isWindowOpen;

        private static void RenderWindow()
        {
            ImGui.SetNextWindowDockID(1, ImGuiCond.Appearing);
            if (ImGui.Begin("Game", ref isWindowOpen, ImGuiWindowFlags.MenuBar| ImGuiWindowFlags.DockNodeHost))
            {
                Render_MenuBar();

                // Early out if the window is collapsed, as an optimization.
                ImGui.End();
            }
        }

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
                UI_Shared.Render_YesNOModalMessage(title, "Exit Engine?", responseAction);
            };
            game.RenderOnUIThread(cmd);
        }

        private static void Render_MenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                Render_FileMenu();
                if (ImGui.BeginMenu("Tools"))
                {
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
        }

        private static void Render_FileMenu()
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.Button("Load Models"))
                {
                    game.ImportModels();
                }
                if (ImGui.Button("Save Models"))
                {
                    game.SaveModels();
                }

                ImGui.EndMenu();
            }
        }


    }
}