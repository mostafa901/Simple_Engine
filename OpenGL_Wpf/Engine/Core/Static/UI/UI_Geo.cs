using ImGuiNET;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Scene;
using System;
using System.Drawing;

namespace Simple_Engine.Engine.Core.Static
{
    public static class UI_Geo
    {
        private static Base_Geo Model;

        public static void RenderUI(Base_Geo model)
        {
            if (model == null) return;
            Model = model;
            RenderWindow();
        }

        private static bool isWindowOpen;

        private static void RenderWindow()
        {
            RightClick();

            ImGui.SetNextWindowDockID(1, ImGuiCond.Appearing);
            if (ImGui.Begin("Geometry", ref isWindowOpen, ImGuiWindowFlags.None))
            {
                ImGui.SetNextWindowCollapsed(false, ImGuiCond.Appearing);
                if (UI_Shared.IsExpanded("Properties"))
                {
                    UI_Shared.Render_Name(Model);
                    Render_UID();
                    UI_Shared.Render_Color(Model);
                }
                if (UI_Shared.IsExpanded("Display"))
                {
                    UI_Shared.Render_Isolate(Model);
                    UI_Shared.Render_CastShadow(Model);
                    Render_Clipping();
                }

                // Early out if the window is collapsed, as an optimization.
                ImGui.End();
            }
        }

        private static void Render_Clipping()
        {
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
                ImGui.SetNextItemOpen(clipenab);
                if (ImGui.Checkbox("Global Clipping", ref Shader.ClipGlobal))
                {
                    foreach (var clip in Model.ClipPlans)
                    {
                        clip.SetAsGlobal(Shader.ClipGlobal);
                    }
                }
                ImGui.EndGroup();
            }
        }

        private static void Render_UID()
        {
            string val = Model.Uid ?? "";
            if (UI_Shared.isInputChanged("Unique Id", ref val))
            {
                Model.Uid = val;
            }
        }



        private static void RightClick()
        {
            if (ImGui.GetIO().MouseClicked[(int)ImGuiMouseButton.Left])
            {
                if (UI_Shared.IsAnyCaptured()) return;

                var vec2 = ImGui.GetIO().MouseClickedPos[(int)ImGuiMouseButton.Left];
                var pos = new Point((int)vec2.X, (int)vec2.Y);
                var testModel = CameraModel.ActiveCamera.PickObject(pos);
                if (testModel == null || testModel != Base_Geo.SelectedModel)
                {
                   UI_Shared. OpenContext = false;
                }
                else
                {
                    UI_Shared.OpenContext = true;
                    ImGui.SetNextWindowPos(ImGui.GetIO().MouseClickedPos[(int)ImGuiMouseButton.Left]);
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

        internal static void DeleteModel(IDrawable model)
        {
            var cmd = new cus_CMD();
            Action<bool> response = (x) =>
            {
                if (x)
                {
                    Base_Geo.SelectedModel.Set_Selected(false);
                    SceneModel.ActiveScene.RemoveModels(model);
                }
                Game.Instance.Dispose_RenderOnUIThread(cmd);
            };
            cmd.Action = (x) =>
            {
                string title = "Delete?";
                ImGui.OpenPopup(title);

                UI_Shared.Render_YesNOModalMessage(title, "do you Want to delete model?", response);
            };
            Game.Instance.RenderOnUIThread(cmd);
        }
    }
}