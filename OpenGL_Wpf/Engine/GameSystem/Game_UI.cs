using ImGuiNET;
using OpenTK;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.Core.Static.UI;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.ImGui_Set;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Scene;

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
            ImGui.DockSpaceOverViewport();
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
            UI_Camera.RenderUI(CameraModel.ActiveCamera);
            UI_Light.RenderUI(LightModel.SelectedLight);
            UI_Fog.RenderUI(SceneModel.ActiveScene.SceneFog);
            Core.Static.UI_Geo.RenderUI(Base_Geo.SelectedModel as Base_Geo);

            _controller?.Render();
        }

        internal void UpdateSizeUI()
        {
            _controller?.WindowResized(Width, Height);
        }
    }
}