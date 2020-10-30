using ImGuiNET;
using OpenTK;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.ImGui_Set;

namespace Simple_Engine.Engine.GameSystem
{
    public partial class Game
    {
        private ImGuiController _controller;

        public void Setup_GameUI()
        {
            _controller = new ImGuiController(Width, Height);
            Imgui_Settings();
             KeyPress += Game_KeyPress;
        }

        public void Imgui_Settings()
        {
            ImGui.CaptureMouseFromApp(true);
            ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable;
            ImGui.GetIO().ConfigDockingAlwaysTabBar = true;
        }

        private void Game_KeyPress(object sender, KeyPressEventArgs e)
        {
            _controller.PressChar(e.KeyChar);
        }

        internal void UpdateUI(float time)
        {
            _controller?.Update(this, time);
        }

        internal void RenderUI()
        {
            UI_Game.RenderUI(this);
            _controller?.Render();
        }

        internal void UpdateSizeUI()
        {
            _controller?.WindowResized(Width, Height);
        }
    }
}