using OpenTK;
using OpenTK.Input;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.ImGui_Set;
using System;
using System.Drawing;

namespace Simple_Engine.Engine.Space.Camera
{
    public partial class CameraModel
    {
        private float GetSpeed()
        {
            var speed = (float)DisplayManager.UpdatePeriod * .005f;

            if (Keyboard.GetState().IsKeyDown(Key.ControlLeft))
            {
                speed *= 100;
            }
            return speed;
        }

        public void Setup_Events()
        {
            Activate_PanCameraModel();
            Activate_MoveTarget();
            scene.game.MouseWheel += Game_MouseWheel;
            scene.game.KeyDown += Game_KeyDown;
        }

        private void Activate_MoveTarget()
        {
            scene.game.MouseMove += (s, e) =>
            {
                if (e.Mouse.RightButton == ButtonState.Pressed)
                {
                    MoveTarget(e.Position);
                }
            };
        }

        private void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (Imgui_Helper.IsAnyCaptured()) return;
            float speed = 1f;
            Vector3 transvector = new Vector3();

            if (e.Key == Key.C && e.Alt)
            {
                if (Base_Geo.SelectedModel == null)
                {
                    ScopeTo(scene.BBX);
                }
                else
                {
                    ScopeTo(Base_Geo.SelectedModel.BBX);
                }
            }
            // Evaluate_UPVector();
            if (Base_Geo.SelectedModel == null)
            {
                if (e.Key == Key.W)
                {
                    transvector = -speed * Direction;
                }
                if (e.Key == Key.S)
                {
                    transvector = speed * Direction;
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

        private void Game_Load(object sender, EventArgs e)
        {
            Create_UIControls();
        }

        private void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartPoint = e.Position;
        }

        private void Game_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Imgui_Helper.IsAnyCaptured()) return;
            if (Keyboard.GetState().IsKeyDown(Key.AltLeft))
            {
                if (IsPrespective)
                {
                    UpdateFOV(FOV + e.Delta);
                }
            }
            else
            {
                if (IsPrespective)
                {
                    var speed = GetSpeed();

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
                    Height += 10 * Math.Sign(-e.Delta);
                    Width += 10 * Math.Sign(-e.Delta);

                    Activate_Ortho();
                }
            }
        }

        public void PanCamera(Point mousePosition)
        {
            if (Imgui_Helper.IsAnyCaptured()) return;
            float speed = GetSpeed();

            var dx = mousePosition.X - StartPoint.X;
            var dy = mousePosition.Y - StartPoint.Y;

            var HorztransVector = Right * (-dx * speed);
            var VertransVector = UP * (dy * speed);
            var transVector = HorztransVector + VertransVector;
            Position += transVector;

            if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft))
            {
                //rotate around object, no need to translate target
            }
            else
            {
                Target += transVector;
            }

            UpdateCamera();
            StartPoint = mousePosition;
        }

        //how much we are looking left or right
        public void Activate_PanCameraModel()
        {
            scene.game.MouseMove += (s, e) =>
            {
                if (e.Mouse.MiddleButton == ButtonState.Pressed)
                {
                    PanCamera(e.Position);
                }
            };
        }
    }
}