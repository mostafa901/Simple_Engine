using Simple_Engine.Views.ThreeD.Engine.Core.Abstracts;
using Simple_Engine.Views.ThreeD.Engine.Core.Interfaces;
using Simple_Engine.Views.ThreeD.Engine.GameSystem;
using Simple_Engine.Views.ThreeD.Engine.Geometry.Core;
using Simple_Engine.Views.ThreeD.Engine.Geometry.TwoD;
using Simple_Engine.Views.ThreeD.Engine.ImGui_Set;
using Simple_Engine.Views.ThreeD.ToolBox;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using mshtml;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Geometry.InputControls
{
    public class KeyControl
    {
        public ISelectable model { get; }

        public KeyControl(ISelectable geo)
        {
            model = geo;
        }

        private float JumpPower = 20;
        private float Gravity = -10;
        private float upwardSpeed = 0;
        private float TerranLevel = 0;

        public void ActionKey()
        {
            if (Imgui_Helper.IsAnyCaptured()) return;
            if (!model.GetSelected()) return;

            float moveStep = (float)DisplayManager.UpdatePeriod * 8f;
            float turnStep = (float)DisplayManager.UpdatePeriod * 40f;
            var keyState = Keyboard.GetState();

            if (keyState.IsAnyKeyDown)
            {
                if (keyState.IsKeyDown(OpenTK.Input.Key.Right))
                {
                    model.LocalTransform = eMath.Rotate(model.LocalTransform, moveStep, new Vector3(0, 0, 1));
                }

                if (keyState.IsKeyDown(OpenTK.Input.Key.Left))
                {
                    model.LocalTransform = eMath.Rotate(model.LocalTransform, -moveStep, new Vector3(0, 0, 1));
                }
                if (keyState.IsKeyDown(OpenTK.Input.Key.Up))
                {
                    model.LocalTransform = eMath.Rotate(model.LocalTransform, moveStep, new Vector3(1, 0, 0));
                }

                if (keyState.IsKeyDown(OpenTK.Input.Key.Down))
                {
                    model.LocalTransform = eMath.Rotate(model.LocalTransform, -moveStep, new Vector3(1, 0, 0));
                }

                if (keyState.IsKeyDown(Key.W))
                {
                    model.MoveWorld(new Vector3(-moveStep, 0, 0));
                }
                if (keyState.IsKeyDown(Key.S))
                {
                    model.MoveWorld(new Vector3(moveStep, 0, 0));
                }
                if (keyState.IsKeyDown(Key.A))
                {
                    model.LocalTransform = eMath.Rotate(model.LocalTransform, turnStep, new Vector3(0, 1, 0));
                }
                if (keyState.IsKeyDown(Key.D))
                {
                    model.LocalTransform = eMath.Rotate(model.LocalTransform, -turnStep, new Vector3(0, 1, 0));
                }

                if (keyState.IsKeyDown(Key.Space))
                {
                    if (upwardSpeed == 0)
                    {
                        upwardSpeed = JumpPower;
                    }
                }
            }

            if (upwardSpeed != 0)
            {
                upwardSpeed += Gravity * (float)DisplayManager.UpdatePeriod;

                model.LocalTransform = eMath.MoveWorld(model.LocalTransform, new Vector3(0, upwardSpeed * (float)DisplayManager.UpdatePeriod, 0));

                if (model.LocalTransform.Row3.Y < TerranLevel)
                {
                    upwardSpeed = 0;
                }
            }
            else
            {
               
            }

#if false
            if (keys.Count == 2)
            {
                if (keys.Contains(OpenTK.Input.Key.ShiftLeft))
                {
                    var key = keys.First();
                    var direction = new Vector3(0, 1, 0);
                    if (keys.Contains(OpenTK.Input.Key.Right))
                    {
                        //    mesh.Rotate(step, direction);
                    }

                    if (keys.Contains(OpenTK.Input.Key.Left))
                    {
                        // mesh.Rotate(-step, direction);
                    }
                }
                if (keys.Contains(OpenTK.Input.Key.ControlLeft))
                {
                    if (keys.Contains(OpenTK.Input.Key.KeypadPlus))
                    {
                        // mesh.Scale(new Vector3(1 + step));
                    }
                    if (keys.Contains(OpenTK.Input.Key.KeypadMinus))
                    {
                        //  mesh.Scale(new Vector3(1 - step));
                    }
                    if (keys.Contains(Key.Left))
                    {
                        //  mesh.Move(new Vector3(-step, 0, 0));
                    }
                    if (keys.Contains(Key.Right))
                    {
                        // mesh.Move(new Vector3(step, 0, 0));
                    }

                    if (keys.Contains(Key.Up))
                    {
                        //  mesh.Move(new Vector3(0, step, 0));
                    }
                    if (keys.Contains(Key.Down))
                    {
                        //  mesh.Move(new Vector3(0, -step, 0));
                    }
                }
            }
#endif
        }
    }
}