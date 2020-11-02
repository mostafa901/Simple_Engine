using ImGuiNET;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Space.Environment;
using Simple_Engine.Engine.Space.Scene;
using System;

namespace Simple_Engine.Engine.Core.Static.UI
{
    public static class UI_Fog
    {
        public static void RenderUI(Fog model)
        {
            if (model == null) return;
            fogModel = model;
            if (!fogModel.Active) return;
            RenderWindow();
        }

        private static bool isWindowOpen;
        private static Fog fogModel;

        private static void RenderWindow()
        {
            ImGui.SetNextWindowDockID(1);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(250, Game.Instance.Height - UI_Game.TotalHeight));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            if (ImGui.Begin("Fog", ref isWindowOpen, ImGuiWindowFlags.DockNodeHost | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.Text("Fog Settings");
                Render_FogDensity();
                Render_FogSpeed();

                // Early out if the window is collapsed, as an optimization.
                ImGui.End();
            }
            ImGui.PopStyleVar();
        }

        private static void Render_FogDensity()
        {
            var val = fogModel.Density;
            var prev = val;
            UI_Shared.DragFloat("Density", ref val, ref prev, (x) =>
               {
                   fogModel.Density += x;
                   SceneModel.ActiveScene.RunOnAllShaders.Push((shader) => shader.SetFloat(shader.FogDensityLocation, fogModel.Density));
               },step:.001f);
        }

        private static void Render_FogSpeed()
        {
            var val = fogModel.FogSpeed;
            var prev = val;
            UI_Shared.DragFloat("Attenuation", ref val, ref prev, (x) =>
            {
                fogModel.FogSpeed += x;
                SceneModel.ActiveScene.RunOnAllShaders.Push((shader) => shader.SetFloat(shader.FogSpeedLocation, fogModel.FogSpeed));
            });
        }
    }
}