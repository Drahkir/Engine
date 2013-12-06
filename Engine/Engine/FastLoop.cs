using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace Engine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr hWnd;
        public Int32 msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }

    public class FastLoop
    {
        public delegate void LoopCallback(double elapsedTime);

        private readonly LoopCallback _callback;
        private readonly PreciseTimer _timer = new PreciseTimer();

        public FastLoop(LoopCallback callback)
        {
            _callback = callback;
            Application.Idle += OnApplicationEnterIdle;
        }

        [SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(
            out Message msg,
            IntPtr hWnd,
            uint messageFilterMin,
            uint messageFilterMax,
            uint flags);

        private void OnApplicationEnterIdle(object sender, EventArgs e)
        {
            while (IsAppStillIdle())
            {
                _callback(_timer.GetElapsedTime());
            }
        }

        private bool IsAppStillIdle()
        {
            Message msg;
            return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
        }
    }
}