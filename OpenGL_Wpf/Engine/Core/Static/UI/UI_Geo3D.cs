using ImGuiNET;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Space.Camera;
using System.Drawing;

namespace Simple_Engine.Engine.Core.Static
{
    public static class UI_Geo3D
    {
        public static void Render_Clipping(Base_Geo3D Model)
        {
            if (Model == null) return;
            var val = Model.EnableClipPlans;
            if (ImGui.Checkbox("Clip Model", ref val))
            {
                Model.SetEnableClipPlans(val);
            }

            if (Model.EnableClipPlans)
            {
                ImGui.BeginGroup();
                foreach (var clip in Model.ClipPlans)
                {
                    UI_Shared.Render_IsActive(clip);

                    ImGui.SameLine();

                    var prev = (clip.LocalTransform.ExtractTranslation() * clip.ClipDirection).Length;
                    var valv = prev;

                    if (clip.IsActive)
                    {
                        UI_Shared.DragFloat(clip.Name, ref valv, ref prev, (x) =>
                        {
                            clip.MoveLocal(clip.ClipDirection * x);
                        });
                    }
                    else
                    {
                        ImGui.TextDisabled("Disabled");
                    }
                }

                ImGui.EndGroup();
            }
        }

        public static void Render_Clipping2(Base_Geo3D Model)
        {
            if (Model == null) return;
            var clipenab = Model.EnableClipPlans;
            if (ImGui.Checkbox("Clip Model", ref clipenab))
            {
                Model.SetEnableClipPlans(clipenab);
            }

            if (Model.EnableClipPlans)
            {
                ImGui.BeginGroup();
                foreach (var clip in Model.ClipPlans)
                {
                    var act = clip.IsActive;

                    if (ImGui.Checkbox($"##{clip.Name}", ref act))
                    {
                        clip.IsActive = act;
                    }

                    ImGui.SameLine();

                    var prev = (clip.LocalTransform.ExtractTranslation() * clip.ClipDirection).Length;
                    var val = prev;

                    if (clip.IsActive)
                    {
                        UI_Shared.DragFloat(clip.Name, ref val, ref prev, (x) =>
                           {
                               clip.MoveLocal(clip.ClipDirection * x);
                           });
                    }
                    else
                    {
                        ImGui.TextDisabled("Disabled");
                    }
                }

                if (ImGui.Checkbox("Global Clipping", ref CameraModel.EnableClipPlans))
                {
                    foreach (var clip in Model.ClipPlans)
                    {
                        clip.SetAsGlobal(CameraModel.EnableClipPlans);
                    }
                }
                ImGui.EndGroup();
            }
        }

        public static void RightClick(Base_Geo3D Model)
        {
            if (Model == null) return;

            if (ImGui.GetIO().MouseClicked[(int)ImGuiMouseButton.Left])
            {
                UI_Shared.OpenContext = false;
            }
            if (ImGui.GetIO().MouseReleased[(int)ImGuiMouseButton.Right])
            {
                if (UI_Shared.IsAnyCaptured())
                {
                    return;
                }

                var vec2 = ImGui.GetIO().MouseClickedPos[(int)ImGuiMouseButton.Right];
                var pos = new Point((int)vec2.X, (int)vec2.Y);
                var testModel = CameraModel.ActiveCamera.PickObject(pos);
                if (testModel == null || testModel != Base_Geo.SelectedModel)
                {
                    UI_Shared.OpenContext = false;
                }
                else
                {
                    UI_Shared.OpenContext = true;
                    ImGui.SetNextWindowPos(ImGui.GetIO().MouseClickedPos[(int)ImGuiMouseButton.Right]);
                }
            }

            if (UI_Shared.OpenContext)
            {
                ImGui.SetNextWindowSize(new System.Numerics.Vector2());
                if (ImGui.Begin("Context", ref UI_Shared.OpenContext, ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar))
                {
                    if (ImGui.MenuItem("Test"))
                    {
                        UI_Shared.OpenContext = false;
                    }

                    ImGui.End();
                }
            }
        }
    }
}