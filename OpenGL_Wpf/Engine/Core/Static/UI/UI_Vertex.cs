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
        public static void RenderUI()
        {
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
                var model = ((Base_Geo3D)Base_Geo.SelectedModel);
                if (model != null)
                {
                    var count = model.DrawType == PrimitiveType.Points ? 1 : 2;
                    for (int i = 0; i < count; i++)
                    {
                        var vpos = Base_Geo3D.SelectedVertex[i];
                        if (vpos == null) continue;
                        var pos = vpos.ToSystemNumeric();

                        if (ImGui.DragFloat3($"Position V{i}", ref pos, .1f))
                        {
                            var index = model.Positions.IndexOf(Base_Geo3D.SelectedVertex[i]);

                            if (index != -1)
                            {
                                Base_Geo3D.SelectedVertex[i] = pos.ToVector();
                                model.Positions[index] = Base_Geo3D.SelectedVertex[i];
                            }
                        }
                    }

                    if (model.DrawType == PrimitiveType.Points)
                    {
                        if (ImGui.DragFloat("Point Size", ref DisplayManager.PointSize, 0.1f, 0, 20))
                        {
                            GL.PointSize(DisplayManager.PointSize);
                        }
                    }
                    if (model.DrawType == PrimitiveType.Lines)
                    {
                        if (ImGui.DragFloat("Line Size", ref DisplayManager.LineSize, 0.1f, 0, 20))
                        {
                            GL.LineWidth(DisplayManager.LineSize);
                        }
                    }
                }

                ImGui.End();
            }

            ImGui.PopStyleVar();
        }
    }
}