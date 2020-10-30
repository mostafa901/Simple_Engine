using OpenTK;
using OpenTK.Input;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.ImGui_Set;
using Simple_Engine.ToolBox;
using System;

namespace Simple_Engine.Engine.Static.InputControl
{
    public static class KeyControl
    {
        private static float JumpPower = 20;
        private static float Gravity = -10;
        private static float upwardSpeed = 0;
        private static float TerranLevel = 0;

        public static void Update_ActionKey()
        {
            ISelectable model = Base_Geo.SelectedModel as ISelectable;
            if (model == null) return;
            if (Imgui_Helper.IsAnyCaptured()) return;
            if (!model.GetSelected()) return;

            float moveStep = (float)DisplayManager.UpdatePeriod * .01f ;
            float turnStep = (float)DisplayManager.UpdatePeriod * .1f;
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
                var step = (float)DisplayManager.UpdatePeriod * .001f;
                upwardSpeed += Gravity * step;

                model.LocalTransform = eMath.MoveWorld(model.LocalTransform, new Vector3(0, upwardSpeed * step, 0));

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