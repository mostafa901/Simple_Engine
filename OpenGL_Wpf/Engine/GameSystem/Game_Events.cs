using ImGuiNET;
using OpenTK.Input;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.Space.Camera;

namespace Simple_Engine.Engine.GameSystem
{
    public partial class Game
    {
        public void Setup_Events()
        {
            KeyDown += Game_KeyDown;
            MouseDown += Game_MouseDown;
            MouseWheel += Game_MouseWheel;
            MouseMove += Game_MouseMove;
        }

        private void Game_MouseMove(object sender, MouseMoveEventArgs e)
        {
            CameraModel.ActiveCamera.Game_MouseMove(e);
        }

        private void Game_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            CameraModel.ActiveCamera.Game_MouseWheel(e);
        }

        private void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (UI_Shared.IsAnyCaptured()) return;

            if (Base_Geo.SelectedModel != null && e.Button == MouseButton.Right)
            {
                RunOnUIThread(() => ImGui.OpenPopup("Model context"));
            }
            CameraModel.ActiveCamera.Game_MouseDown(e);
        }

        private void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (UI_Shared.IsAnyCaptured()) return;

            if (e.Key == Key.Escape)
            {
                if (Base_Geo.SelectedModel != null)
                {
                    Base_Geo.SelectedModel.Set_Selected(false);
                }
                else
                {
                    ShowExitMessage();
                }
            }
            if (e.Key == Key.Delete)
            {
                if (Base_Geo.SelectedModel != null)
                {
                    Base_Geo.SelectedModel.Delete();
                }
            }

            CameraModel.ActiveCamera.Game_KeyDown(e);
        }

        private void ShowExitMessage()
        {
            UI_Shared.OpenContext = false;
            UI_Game.Render_Exit();
        }
    }
}