using ImGuiNET;
using OpenTK.Input;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Static;
using System;

namespace Simple_Engine.Engine.GameSystem
{
    public partial class Game
    {
        public void Setup_Events()
        {
            KeyDown += Game_KeyDown;
            MouseDown += Game_MouseDown;
        }

        private void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Base_Geo.SelectedModel != null && e.Button == MouseButton.Right)
            {
                RunOnUIThread(() => ImGui.OpenPopup("Model context"));
            }
        }

        private void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ShowExitMessage();
            }
            if (e.Key == Key.Delete)
            {
                if (Base_Geo.SelectedModel != null)
                {
                    Base_Geo.SelectedModel.Delete();
                }
            }
        }

        private void ShowExitMessage()
        {
            UI_Game.Render_Exit();
        }
    }
}