using ImGuiNET;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Render.ShaderSystem;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Extentions;
using System.Linq;

namespace Simple_Engine.Engine.Core.Static
{
    public static class UI_Vertex
    {
        private static Vector3 Model;

        public static void RenderUI(Vector3 model)
        {
            if (model == null) return;
            Model = model;

            RenderWindow();
        }

        private static bool isWindowOpen;

        private static void RenderWindow()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(250, Game.Instance.Height - 20));
            ImGui.SetNextWindowDockID(1, ImGuiCond.Appearing);
            if (ImGui.Begin("Vertex", ref isWindowOpen, ImGuiWindowFlags.DockNodeHost | ImGuiWindowFlags.NoResize))
            {
                var pos = Model.ToSystemNumeric();

                if (ImGui.InputFloat3("Position", ref pos))
                {
                    var model = ((Base_Geo3D)Base_Geo.SelectedModel);
                    if (model != null)
                    {
                        var index = model.Positions.IndexOf(Model);

                        if (index != -1)
                        {
                            Model = model.Positions[index] = pos.ToVector();
                        }
                    }
                }

                if (ImGui.DragFloat("Point Size", ref DisplayManager.PointSize, 0.1f, 0, 10))
                {
                    GL.PointSize(DisplayManager.PointSize);
                }
                ImGui.End();
            }

            ImGui.PopStyleVar();
        }
    }
}