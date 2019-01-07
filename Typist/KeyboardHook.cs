using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace Typist
{
    /// <summary>
    /// A keyboard hook wrapper offering simple access
    /// to keyDown events from the system's input loop.
    /// Simplifies useage of the low level hooks.
    /// 
    /// Each key press fires an event which carries an
    /// easy to understand string representation of 
    /// the pressed key.
    /// </summary>
    class KeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;

        private const int WM_SYSKEYDOWN = 0x0104;

        private LowLevelKeyboardProc _proc;

        private IntPtr _hookID = IntPtr.Zero;                

        public KeyboardHook()
        {
            _proc = HookCallback;
        }

        public void enable()  
        {      
            _hookID = SetHook(_proc);
            Application.Run();
        }

        public void disable()
        {
            UnhookWindowsHookEx(_hookID);
            Application.ExitThread();
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN) || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                var key = (Keys)vkCode;
                var textualRepresentation = key.ToString();
                OnKeyPressed(textualRepresentation);              
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        protected virtual void OnKeyPressed(String key)
        {
            EventHandler<String> handler = KeyPressed;
            if (handler != null)
            {
                handler(this, key);
            }
        }     

        public event EventHandler<String> KeyPressed;    

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, 
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
