﻿using OpenTK;
using OpenTK.Input;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.GameSystem;
using System;
using System.Drawing;

namespace Simple_Engine.Engine.Space.Camera
{
    public partial class CameraModel
    {
        private float GetSpeed()
        {
            var speed = (float)DisplayManager.UpdatePeriod * .0005f;
            if (Keyboard.GetState().IsKeyDown(Key.ControlLeft))
            {
                return speed * 4;
            }
            else
            {
            }
            return speed;
        }

        public void Game_MoveTarget(MouseMoveEventArgs e)
        {
        }

        public void Game_KeyDown(KeyboardKeyEventArgs e)
        {
            if (UI_Shared.IsAnyCaptured()) return;
            float speed = 1f;
            Vector3 transvector = new Vector3();

            if (e.Key == Key.C && e.Alt)
            {
                if (Base_Geo.SelectedModel == null)
                {
                    ScopeTo(scene.BBX, true);
                }
                else
                {
                    ScopeTo(Base_Geo.SelectedModel.BBX, true);
                }
            }
            // Evaluate_UPVector();
            if (Base_Geo.SelectedModel == null)
            {
                if (e.Key == Key.W)
                {
                    if (ViewType == CameraType.Plan)
                    {
                        transvector =  speed * UP;
                    }
                    else
                    {
                        transvector = -speed * Direction;
                    }
                }
                if (e.Key == Key.S)
                {
                    if (ViewType == CameraType.Plan)
                    {
                        transvector = -speed * UP;
                    }
                    else
                    {
                        transvector = speed * Direction;
                    }
                }
                if (e.Key == Key.A)
                {
                    transvector = Right.Normalized() * -speed;
                }
                if (e.Key == Key.D)
                {
                    transvector = Right.Normalized() * speed;
                }
                Position += transvector;
                Target += transvector;
            }

            UpdateCamera();
        }

        private void MoveTarget(Point position)
        {
            if (UI_Shared.IsAnyCaptured()) return;
            if (ViewType != CameraType.Perspective)
            {
                var msg = "3d Person View not enabled while in Plan View";
                UI_Game.DisplayStatusmMessage(msg, 3000);
                return;
            }
            float speed = GetSpeed();

            var dx = position.X - StartPoint.X;
            var dy = position.Y - StartPoint.Y;
            Target += UP * -dy * speed;
            Target += Right * dx * speed;

            UpdateCamera();
            StartPoint = position;
        }

        public void Game_MouseDown(MouseButtonEventArgs e)
        {
            StartPoint = e.Position;
        }

        public void Game_MouseWheel(MouseWheelEventArgs e)
        {
            if (UI_Shared.IsAnyCaptured()) return;
            if (Keyboard.GetState().IsKeyDown(Key.AltLeft))
            {
                if (IsPerspective)
                {
                    UpdateFOV(FOV + e.Delta);
                }
            }
            else
            {
                if (IsPerspective)
                {
                    var speed = GetSpeed() * 100;

                    //Evaluate_UPVector();
                    var displacement = Direction.Normalized() * -e.Delta * speed;
                    Position += displacement;
                    if (hoockedModel == null)
                    {
                        Target += displacement;
                    }
                    UpdateCamera();
                }
                else
                {
                    SetHeight(height += 10 * Math.Sign(-e.Delta));

                    Activate_Ortho();
                }
            }
        }

        public void PanCamera(Point mousePosition)
        {
            if (UI_Shared.IsAnyCaptured()) return;
            float speed = GetSpeed() * 4.5f;

            var dx = mousePosition.X - StartPoint.X;
            var dy = mousePosition.Y - StartPoint.Y;

            var HorztransVector = Right * (-dx * speed);
            var VertransVector = UP * (dy * speed);
            var transVector = HorztransVector + VertransVector;
            Position += transVector;

            if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft))
            {
                //rotate around object, no need to translate target
                if (Base_Geo.SelectedModel != null)
                {
                    AnimateCameraTarget(Base_Geo.SelectedModel.BBX.GetCG(), 100);
                }
            }
            else
            {
                Target += transVector;
            }

            UpdateCamera();
            StartPoint = mousePosition;
        }

        //how much we are looking left or right
        public void Game_MouseMove(MouseMoveEventArgs e)
        {
            if (e.Mouse.MiddleButton == ButtonState.Pressed)
            {
                PanCamera(e.Position);
            }

            if (e.Mouse.RightButton == ButtonState.Pressed)
            {
                MoveTarget(e.Position);
            }
        }
    }
}