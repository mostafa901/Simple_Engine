using ImGuiNET;
using OpenTK;
using Simple_Engine.Engine.Geometry;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Extentions;

namespace Simple_Engine.Engine.Core.Static
{
    public static class UI_Light
    {
        private static LightModel light;

        public static void RenderUI(LightModel model)
        {
            if (model == null) return;
            light = model;

            RenderWindow();
        }

        private static bool isWindowOpen;

        private static void RenderWindow()
        {
            ImGui.SetNextWindowDockID(1, ImGuiCond.Appearing);
            if (ImGui.Begin("Light", ref isWindowOpen, ImGuiWindowFlags.DockNodeHost))
            {
                UI_Shared.Render_Name(light);

                Render_CastShadow();
                Render_Color();
                Render_Intenisty();
                Render_ShowLightRay();
                Render_Position();

                // Early out if the window is collapsed, as an optimization.
                ImGui.End();
            }
        }

        private static void Render_Position()
        {
            var val = light.LightPosition.ToSystemNumeric();
            if (ImGui.DragFloat3("Position", ref val))
            {
                light.LightPosition = val.ToVector();
                light.SetShadowTransform();
                light.UpdateLightRay();
            }
        }

        private static void Render_ShowLightRay()
        {
            bool val = light.LightRay?.IsActive ?? false;
            if (ImGui.Checkbox("Show Light Ray", ref val))
            {
                if (light.LightRay == null)
                {
                    light.LightRay = new Line(new Vector3(), new Vector3(1));
                    light.LightRay.IsSystemModel = true;
                    light.LightRay.IsActive = false;
                    light.LightRay.BuildModel();
                    SceneModel.ActiveScene.UpLoadModels(light.LightRay);
                }

                {
                    light.LightRay.IsActive = !light.LightRay.IsActive;
                    if (light.LightRay.IsActive)
                    {
                        light.UpdateLightRay();
                    }
                }
            }
        }

        private static void Render_Intenisty()
        {
            float val = light.Intensity;
            float prev = val;
            if (ImGui.DragFloat("Intensity", ref val, .1f,0,10))
            {
                var diff = val - prev;
                light.DefaultColor += new Vector4(diff, diff, diff, 1);
                light.Intensity += diff;
            }
        }

        private static void Render_Color()
        {
            UI_Shared.Render_Color(light);
        }

        private static void Render_CastShadow()
        {
            ImGui.Checkbox("CastShadow", ref light.CastShadow);
        }
    }
}