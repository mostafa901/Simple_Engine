using OpenTK;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Extentions;
using System;

namespace Simple_Engine.Engine.ImGui_Set
{
    public static class Imgui_Generator
    {
        public static void GenerateControls(Base_Geo model, ImgUI_Controls imguiWin)
        {
            var imgui_properties = new Imgui_Expander(imguiWin, "Properties");
            var imgui_Modify = new Imgui_Expander(imguiWin, "Modify");
            var imgui_Display = new Imgui_Expander(imguiWin, "Display");

            var props = model.GetType().GetProperties();

            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(Vector3))
                {
                    Add_Vector3(model, imgui_properties, prop);
                }
                else if (prop.PropertyType == typeof(Vector4))
                {
                    if (prop.Name != nameof(model.DefaultColor))
                    {
                        Add_Vector4(model, imgui_properties, prop);
                    }
                }
                if (prop.PropertyType == typeof(string))
                {
                    Add_String(model, imgui_properties, prop);
                }
            }

            Add_Imgui_V3Position(model, imgui_properties);
            Add_Imgui_DefaultColor(model, imgui_properties);
            Add_Imgui_Isolate(model, imgui_Display);
            Add_Imgui_Hide(model, imgui_Display);
            Add_Imgui_CastShadowCheckBox(model, imgui_Modify);
        }

        private static void Add_Imgui_Isolate(Base_Geo model, Imgui_Expander imgui_Display)
        {
            var bt = new Imgui_Button(imgui_Display, "Isolate", null);
            Action<object> action = (x) =>
             {
                 if (SceneModel.ActiveScene.IsolateDisplay)
                 {
                     SceneModel.ActiveScene.ActivateModels();
                     SceneModel.ActiveScene.IsolateDisplay = false;
                 }
                 else
                 {
                     SceneModel.ActiveScene.IsolateModel(model);
                     SceneModel.ActiveScene.IsolateDisplay = true;
                 }
                 if (SceneModel.ActiveScene.IsolateDisplay)
                 {
                     bt.color = new System.Numerics.Vector4(1, 0, 0, 1);
                 }
                 else
                 {
                     bt.color = Imgui_Button.defaultColor;
                 }
             };
            bt.ButtonAction = action;
        }

        private static void Add_Imgui_V3Position(Base_Geo model, Imgui_Expander imgui_properties)
        {
            new Imgui_DragFloat3(imgui_properties, "Position", () => model.LocalTransform.ExtractTranslation(), (x) =>
               {
                   model.MoveWorld(x);
               });
        }

        private static void Add_Imgui_CastShadowCheckBox(Base_Geo model, Imgui_Expander imgui_Modify)
        {
            new Imgui_CheckBox(imgui_Modify, "CastShadow", () => model.CastShadow, (x) =>
              {
                  model.CastShadow = !model.CastShadow;
              });
        }

        private static void Add_Imgui_Hide(Base_Geo model, Imgui_Expander imgui_properties)
        {
            new Imgui_CheckBox(imgui_properties, "Hide", () => !model.IsActive, (x) =>
                {
                    model.IsActive = !x;
                });
        }

        private static void Add_String(Base_Geo model, Imgui_Expander imgui_expander, System.Reflection.PropertyInfo prop)
        {
            new Imgui_InputString(imgui_expander, prop.Name, () => (string)prop.GetValue(model),
             (x) =>
             {
                 prop.SetValue(model, x);
             });
        }

        private static void Add_Vector3(Base_Geo model, Imgui_Expander imgui_expander, System.Reflection.PropertyInfo prop)
        {
            new Imgui_Vector3(imgui_expander, prop.Name, (Vector3)prop.GetValue(model),
            (x) =>
            {
                prop.SetValue(model, x);
            });
        }

        private static void Add_Vector4(Base_Geo model, Imgui_Expander imgui_expander, System.Reflection.PropertyInfo prop)
        {
            new Imgui_Vector4(imgui_expander, prop.Name, (Vector4)prop.GetValue(model),
            (x) =>
            {
                prop.SetValue(model, x);
            });
        }

        public static void Add_Imgui_DefaultColor(IDrawable model, ImgUI_Controls guiparent)
        {
            new Imgui_Color(guiparent, "Default Color", model.DefaultColor, (x) =>
            {
                model.DefaultColor = x.Round(2);

                model.ShaderModel.RunOnUIThread.Push(() =>
                {
                    model.ShaderModel.SetVector4(model.ShaderModel.Location_DefaultColor, model.DefaultColor);
                });
            });
        }
    }
}