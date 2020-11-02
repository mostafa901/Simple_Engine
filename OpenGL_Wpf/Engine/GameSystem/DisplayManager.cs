using System;

namespace Simple_Engine.Engine.GameSystem
{
    public static class DisplayManager
    {
    public enum RenderBufferType
    {
    Normal,
    Selection
    }
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
        //this is created to signal what Render settings to consider prior rendering an object on this frame
        //for example selection buffer we need to disable all blended objects
        public static RenderBufferType CurrentBuffer { get; internal set; }

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