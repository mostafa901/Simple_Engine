using ImGuiNET;
using OpenTK;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Extentions;

namespace Simple_Engine.Engine.Core.Static
{
    public static class UI_Camera
    {
        private static CameraModel camera;

        public static void RenderUI(CameraModel model)
        {
            if (model == null) return;
            camera = model;

            RenderWindow();
        }

        private static bool isWindowOpen;

        private static void RenderWindow()
        {
            //ImGui.SetNextWindowDockID(0, ImGuiCond.Always);
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, 20));
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(250, Game.Instance.Height - 20));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            if (ImGui.Begin("Camera", ref isWindowOpen,   ImGuiWindowFlags.None))
            {
                ImGui.Text("Camera Settings");
                UI_Shared.Render_Name(camera);

                RenderDisplayMode();
                Render_FOV();
                Render_Width();
                Render_Height();
                Render_Position();
                Render_Target();
                RenderCameraLine();
                // Early out if the window is collapsed, as an optimization.
                ImGui.End();
            }
            ImGui.PopStyleVar();

        }

        private static void Render_FOV()
        {
            var val = camera.FOV;

            if (ImGui.DragFloat("FOV", ref val, .01f, 1f, 90))
            {
                camera.UpdateFOV(val);
            }
        }

        private static void Render_Position()
        {
            var pos = camera.Position.ToSystemNumeric();
            if (ImGui.DragFloat3("Position", ref pos, .01f))
            {
                camera.Position = pos.ToVector();
                camera.UpdateCamera();
            }
        }

        private static void Render_Target()
        {
            var pos = camera.Target.ToSystemNumeric();
            if (ImGui.DragFloat3("Target", ref pos, .01f))
            {
                camera.Target = pos.ToVector();
                camera.UpdateCamera();
            }
        }

        private static void RenderCameraLine()
        {
            ImGui.Checkbox("Show Camera Line", ref camera.IsDirectionVisible);
        }

        private static void RenderDisplayMode()
        {
            bool val = camera.IsPrespective;
            if (ImGui.Checkbox("Display Mode", ref val))
            {
                if (val)
                {
                    camera.ActivatePrespective();
                }
                else
                {
                    camera.Activate_Ortho();
                }
            }
        }

        private static void Render_Height()
        {
            var val = camera.Height;
            if (ImGui.DragFloat("Height", ref val))
            {
                camera.Height = MathHelper.Clamp(val, 50, 2000);
                camera.UpdateViewMode();
            }
        }

        private static void Render_Width()
        {
            var val = camera.Width;
            if (ImGui.DragFloat("Width", ref val))
            {
                camera.Width = MathHelper.Clamp(val, 50, 2000);
                camera.UpdateViewMode();
            }
        }
    }
}