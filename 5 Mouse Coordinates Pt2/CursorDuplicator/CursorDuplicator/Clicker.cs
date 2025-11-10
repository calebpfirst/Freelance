//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using Gma.UserActivityMonitor;
//using System.Threading;
//using System.Runtime.InteropServices;

//namespace CursorDuplicator
//{
//    class Clicker
//    {
//        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
//        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;
//        [DllImport("user32.dll")]
//        private static extern void mouse_event(
//               UInt32 dwFlags, // motion and click options
//               UInt32 dx, // horizontal position or change
//               UInt32 dy, // vertical position or change
//               UInt32 dwData, // wheel movement
//               IntPtr dwExtraInfo // application-defined information
//        );
//        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]

//        public static extern int SetCursorPos(int x, int y);

//        public static void StartClick(Point location)
//        {
//            Cursor.Position = location;
//            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
//        }
//        public static void EndClick(Point location)
//        {
//            Cursor.Position = location;
//            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
//        }
//    }
//}
