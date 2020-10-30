using ImGuiNET;
using OpenTK;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Extentions;
using System;

namespace Simple_Engine.Engine.Core.Static
{
    public static class UI_Shared
    {
        public static void Render_YesNOModalMessage(string Name, string Message, Action<bool> ResponseAction)
        {
            bool isOpen = true;

            // Always center this window when appearing
            System.Numerics.Vector2 center = new System.Numerics.Vector2(ImGui.GetIO().DisplaySize.X * 0.5f, ImGui.GetIO().DisplaySize.Y * 0.5f);
            ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new System.Numerics.Vector2(0.5f, 0.5f));

            if (ImGui.BeginPopupModal(Name, ref isOpen, ImGuiWindowFlags.Modal))
            {
                ImGui.Text(Message);
                ImGui.Separator();

                if (ImGui.Button("Yes", new System.Numerics.Vector2(120, 0)))
                {
                    ResponseAction(true);
                    ImGui.CloseCurrentPopup();
                    isOpen = false;
                }
                ImGui.SetItemDefaultFocus();
                ImGui.SameLine();
                if (ImGui.Button("Cancel", new System.Numerics.Vector2(120, 0)))
                {
                    ResponseAction(false);
                    ImGui.CloseCurrentPopup();
                    isOpen = false;
                }

                ImGui.EndPopup();
            }
        }

        public static bool isInputChanged(string name, ref string val)
        {
            return ImGui.InputText(name, ref val, 200);
        }

        public static bool IsExpanded(string name)
        {
            return ImGui.CollapsingHeader(name, ImGuiTreeNodeFlags.OpenOnArrow);
        }

        public static void Render_Color(IRenderable Model)
        {
            var InitialValue = Model.DefaultColor.ToSystemNumeric();
            if (ImGui.ColorPicker4("##picker", ref InitialValue))
            {
                Model.DefaultColor = InitialValue.ToVector().Round(2);

                if (Model is IDrawable)
                {
                    var drawable = Model as IDrawable;
                    drawable.ShaderModel.RunOnUIThread.Push(() =>
                     {
                         drawable.ShaderModel.SetVector4(drawable.ShaderModel.Location_DefaultColor, drawable.DefaultColor);
                     });
                }
            }
        }

        public static void Render_Name(IRenderable model)
        {
            string val = model.Name ?? "";
            if (UI_Shared.isInputChanged("Name", ref val))
            {
                model.Name = val;
            }
        }

        public static void Render_Progress(float progress, float max,string message)
        {
            bool open = true;
           if( ImGui.Begin("Progress", ref open, ImGuiWindowFlags.None))
        {
                // Animate a simple progress bar
                if (false)
                {
                    progress += max * 0.4f * ImGui.GetIO().DeltaTime;
                    if (progress >= +1.1f) { progress = +1.1f; max *= -1.0f; }
                    if (progress <= -0.1f) { progress = -0.1f; max *= -1.0f; }
                }
                progress = progress / max;
                // Typically we would use ImVec2(-1.0f,0.0f) or ImVec2(-FLT_MIN,0.0f) to use all available width,
                // or ImVec2(width,0.0f) for a specified width. ImVec2(0.0f,0.0f) uses ItemWidth.
                ImGui.ProgressBar(progress, new System.Numerics.Vector2(0.0f, 0.0f));

                ImGui.Text(message);

              //  float progress_saturated = MathHelper.Clamp(progress, 0.0f, 1.0f);
                //char buf[32];
                //sprintf(buf, "%d/%d", (int)(progress_saturated * 1753), 1753);
               // ImGui.ProgressBar(progress, new System.Numerics.Vector2(0, 0), $"{(int)(progress_saturated * 1753),1753} %");
                ImGui.End();
            }
        }
    }
}