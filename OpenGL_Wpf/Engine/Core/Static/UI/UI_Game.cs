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
            
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);

            if (ImGui.Begin("Game", ref isWindowOpen, ImGuiWindowFlags.MenuBar| ImGuiWindowFlags.DockNodeHost))
            {
                Render_MenuBar();

                ImGui.End();
            }
            ImGui.PopStyleVar();

           
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
                UI_Shared.Render_YesNOModalMessage(title, "Exit Engine?\r\nAny changes will not be saved.", responseAction);
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
    }
}