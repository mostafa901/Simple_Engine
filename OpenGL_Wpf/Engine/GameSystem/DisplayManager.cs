using System;

namespace Simple_Engine.Engine.GameSystem
{
    public static class DisplayManager
    {
        private static double LastFrameTime;
        public static double UpdatePeriod;

        public enum GameState
        {
            Rendering,
            Uploading,
            Idel
        }

        public static GameState Renderstate = GameState.Idel;
        public static GameState RequestRenderstate = GameState.Idel;

        public static void RequestRender(GameState request)
        {
             
                RequestRenderstate = request;


             
        }


        public static void Initialize()
        {
            LastFrameTime = DateTime.Now.TimeOfDay.TotalSeconds;
        }

        public static void FixTime()
        {
            var currentTime = DateTime.Now.TimeOfDay.TotalSeconds;
            UpdatePeriod = (currentTime - LastFrameTime) * 1000;
            LastFrameTime = currentTime;
        }
    }
}