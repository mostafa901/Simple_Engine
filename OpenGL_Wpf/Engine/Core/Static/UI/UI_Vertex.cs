using ImGuiNET;
using OpenTK;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Extentions;

namespace Simple_Engine.Engine.Core.Static
{
    public static class UI_Vertex
    {
        private static Vector3 Model;

        public static void RenderUI(Vector3 model)
        {
            if (model == null) return;
            Model = model;
            if (geo == null)
            {
                geo = new GeometryModel();
                geo.Positions.Add(new Vector3());
                geo.DrawType = OpenTK.Graphics.OpenGL.PrimitiveType.Points;
                geo.ShaderModel = new Render.Shader(Render.ShaderPath.Color);
                geo.Dynamic = Interfaces.IDrawable.DynamicFlag.Positions;
                SceneModel.ActiveScene.UpLoadModels(geo);
            }
            RenderWindow();
        }

        private static bool isWindowOpen;
        private static GeometryModel geo;

        private static void RenderWindow()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(250, Game.Instance.Height - 20));
            ImGui.SetNextWindowDockID(1, ImGuiCond.Appearing);
            if (ImGui.Begin("Vertex", ref isWindowOpen, ImGuiWindowFlags.DockNodeHost | ImGuiWindowFlags.NoResize))
            {
                geo.Positions[0] = Model;
                var pos = Model.ToSystemNumeric();
                if (ImGui.DragFloat3("Position", ref pos, .01f))
                {
                    geo.Positions[0] += pos.ToVector();

                    // Early out if the window is collapsed, as an optimization.
                }
                ImGui.End();
            }

            ImGui.PopStyleVar();
        }
    }
}