using ImGuiNET;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Extentions;
using System;
using System.Diagnostics;
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
            //if rightclick
            {
                RightClick();
            }

            ImGui.SetNextWindowDockID(1, ImGuiCond.Appearing);
            if (ImGui.Begin("Geometry", ref isWindowOpen, ImGuiWindowFlags.None))
            {
                if (UI_Shared.IsExpanded("Properties"))
                {
                    UI_Shared.Render_Name(Model);
                    Render_UID();
                    UI_Shared.Render_Color(Model);

                }

                // Early out if the window is collapsed, as an optimization.
                ImGui.End();
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

        public static bool OpenContext;

        private static void RightClick()
        {
            if (  ImGui.GetIO().MouseClicked[(int)ImGuiButtonFlags.MouseButtonLeft])
            {
                var vec2 = ImGui.GetIO().MouseClickedPos[(int)ImGuiButtonFlags.MouseButtonLeft];
                var pos = new Point((int)vec2.X, (int)vec2.Y);
                if (CameraModel.ActiveCamera.PickObject(pos) == null)
                {
                    OpenContext = false;
                }
                else
                {
                    OpenContext = true;
                    ImGui.SetNextWindowPos(ImGui.GetIO().MouseClickedPos[(int)ImGuiButtonFlags.MouseButtonLeft]);
                }
            }

            if (OpenContext)
            {
                ImGui.SetNextWindowSize(new System.Numerics.Vector2());
                if (ImGui.Begin("Context", ref OpenContext, ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar))
                {
                    if (ImGui.MenuItem("Test"))
                    {
                        OpenContext = false;
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