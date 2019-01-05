using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace Typist
{
    class KeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;

        private const int WM_SYSKEYDOWN = 0x0104;

        private LowLevelKeyboardProc _proc;

        private IntPtr _hookID = IntPtr.Zero;

        private List<string> WORD_ENDING_KEYS = new List<string> { "Return", "Space", "OemPeriod", "OemComma" };

        private const string KEY_BACKSPACE = "Back";

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
                var text = key.ToString();
                if (text.Length == 1)
                {
                    OnCharacterTyped(Char.Parse(text));
                }
                else if (WORD_ENDING_KEYS.Contains(text))
                {
                    OnWordEnded(EventArgs.Empty);
                }
                else if (text == KEY_BACKSPACE)
                {
                    OnBackSpaceDown(EventArgs.Empty);
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        protected virtual void OnCharacterTyped(Char c)
        {
            EventHandler<Char> handler = CharacterTyped;
            if (handler != null)
            {
                handler(this, c);
            }
        }

        protected virtual void OnWordEnded(EventArgs e)
        {
            EventHandler handler = WordEnded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnBackSpaceDown(EventArgs e)
        {
            EventHandler handler = BackSpaceDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<Char> CharacterTyped;
        public event EventHandler WordEnded;
        public event EventHandler BackSpaceDown;

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
