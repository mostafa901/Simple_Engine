using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.GameSystem
{
    public static class GameDebuger
    {
        #region Debugger

        private static DebugProc callback;

        public static void DebugMode()
        {
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);

            callback = new DebugProc(debugproc);
            GL.DebugMessageCallback(callback, IntPtr.Zero);
            GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare, DebugSeverityControl.DebugSeverityNotification, 0, new int[0], true);
            GL.DebugMessageInsert(DebugSourceExternal.DebugSourceApplication, DebugType.DebugTypeMarker, 0, DebugSeverity.DebugSeverityNotification, -1, "Debug output enabled");
        }

        private static void debugproc(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            if (severity != DebugSeverity.DebugSeverityNotification)
            {
                //Debugger.Break();
            }
            Debug.WriteLine($"\r\n\r\n\r\n***********OpenGL Message**************\r\n\r\n\r\n");
            Debug.WriteLine($"OpenGL Message: {Marshal.PtrToStringAnsi(message)}");
            Debug.WriteLine($"OpenGL Parameter: {Marshal.PtrToStringAnsi(userParam)}");
            Debug.WriteLine($"\r\n\r\n\r\n***********End OpenGL Message**************\r\n\r\n\r\n");

        }

        #endregion Debugger
    }
}