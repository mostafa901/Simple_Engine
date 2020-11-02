using System;

namespace Simple_Engine.Engine.GameSystem
{
    public static class DisplayManager
    {
        private static double LastFrameTime;
        public static float UpdatePeriod;

        public enum GameState
        {
            Rendering,
            Uploading,
            Idel
        }

        public static GameState Renderstate = GameState.Idel;
        public static GameState RequestRenderstate = GameState.Idel;

        public static float DisplayRatio { get; set; }

        public static void RequestRender(GameState request)
        {
            RequestRenderstate = request;
        }

        public static void Initialize(int width, int height)
        {
            LastFrameTime = DateTime.Now.TimeOfDay.TotalSeconds;
            DisplayRatio = (float)width / height;
        }

        

        public static void FixTime()
        {
            var currentTime = DateTime.Now.TimeOfDay.TotalSeconds;
            UpdatePeriod = (float)(currentTime - LastFrameTime) * 1000;
            LastFrameTime = currentTime;
        }
    }
}