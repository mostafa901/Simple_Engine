using ImGuiNET;
using org.omg.CORBA;
using sun.util.resources.cldr.en;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InSitU.Views.ThreeD.Engine.GameSystem
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