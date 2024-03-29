﻿using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Extentions;
using System;
using System.Collections.Generic;
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
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(250, Game.Instance.Height - 20));
            ImGui.SetNextWindowDockID(1, ImGuiCond.Appearing);
            if (ImGui.Begin("Geometry", ref isWindowOpen, ImGuiWindowFlags.DockNodeHost | ImGuiWindowFlags.NoResize))
            {
                UI_Shared.Render_IsActive(Model);
                if (UI_Shared.IsExpanded("Properties"))
                {
                    UI_Shared.Render_Name(Model);
                    Render_UID();
                    UI_Shared.Render_Color(Model);
                    Render_AllowReflection();
                }
                if (UI_Shared.IsExpanded("Display"))
                {
                    UI_Shared.Render_Isolate(Model);
                    UI_Shared.Render_CastShadow(Model);
                    ImGui.Separator();
                    if (ImGui.Button("Plan"))
                    {
                        CameraModel.PlanCamera.AlignCamera(Model.BBX);
                        CameraModel.ActiveCamera.UpdateViewTo(CameraModel.PlanCamera);
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Perspective"))
                    {
                        CameraModel.PerspectiveCamera.AlignCamera(Model.BBX);
                        CameraModel.ActiveCamera.UpdateViewTo(CameraModel.PerspectiveCamera);
                    }
                    ImGui.Separator();

                    Render_DrawTypes();
                    UI_Geo3D.Render_Clipping(Model as Base_Geo3D);
                }
                UI_Geo3D.RightClick(Model as Base_Geo3D);

                // Early out if the window is collapsed, as an optimization.
                ImGui.End();
            }

            ImGui.PopStyleVar();
        }

        private static void Render_DrawTypes()
        {
            var drawnames = new List<PrimitiveType> { PrimitiveType.Points, PrimitiveType.Lines, PrimitiveType.Triangles, PrimitiveType.TriangleStrip };
            foreach (var prim in drawnames)
            {
                if (prim == Model.DrawType)
                {
                    ImGui.PushID("Red");
                    ImGui.PushStyleColor(ImGuiCol.Button, ImGui.ColorConvertFloat4ToU32(OpenTK.Graphics.Color4.Red.ToNumericVector4()));
                    ImGui.Button(prim.ToString());

                    ImGui.PopStyleColor();
                    ImGui.PopID();
                }
                else
                {
                    if (ImGui.Button(prim.ToString()))
                    {
                        Model.DrawType = prim;
                    }
                }
            }
        }

        private static void Render_AllowReflection()
        {
            var val = Model.AllowReflect;
            if (ImGui.Checkbox("Allow Reflection", ref val))
            {
                Model.AllowReflect = val;
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
            };

            UI_Shared.Render_YesNOModalMessage("Delete?", "do you Want to delete model?", response);
        }
    }
}