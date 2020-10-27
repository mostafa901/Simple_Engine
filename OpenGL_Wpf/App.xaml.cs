using Simple_Engine.Engine;
using System;
using System.Windows;

namespace Simple_Engine
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            using (var game = new Game(1200, 800, "Game"))
            {
                game.Run(30);
            }

            Environment.Exit(0);
        }
    }
}