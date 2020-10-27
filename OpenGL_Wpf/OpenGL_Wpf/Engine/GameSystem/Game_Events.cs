using InSitU.Views.ThreeD.Engine.ImGui_Set;
using InSitU.Views.ThreeD.Engine.ImGui_Set.Controls;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.GameSystem
{
    public class Game_Events
    {
        private Game game;
        private Imgui_PopModalWindow exitmsg;

        public event EventHandler RenderingUI;

        public void OnRenderingUI()
        {
            RenderingUI?.Invoke(null, null);
        }

        public Game_Events(Game game)
        {
            this.game = game;

            game.KeyDown += Game_KeyDown;
        }

        private void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (RenderingUI == null)
                {
                    RenderingUI += showExitMessage;
                }
            }
        }

        private void showExitMessage(object sender, EventArgs e)
        {
            if (exitmsg == null)
            {
                exitmsg = new Imgui_PopModalWindow(null, "Exit", "Do you Want To Exit?", () => true, (x) =>
                {
                    if (x)
                    {
                        game.Exit();
                    }
                    exitmsg = null;
                    RenderingUI -= showExitMessage;

                });
            }
            else
            {
                exitmsg.BuildModel();
            }
        }
    }
}