using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typist
{
    class Program
    {        
        static void Main(string[] args)
        {
            KeyboardHook hook = new KeyboardHook();

            hook.CharacterTyped += Hook_CharacterTyped;        
            hook.WordEnded += Hook_WordEnded;
            hook.BackSpaceDown += Hook_BackSpaceDown;

            hook.enable();
            Console.ReadLine();
            hook.disable();
        }

        private static void Hook_BackSpaceDown(object sender, EventArgs e)
        {
            Console.Write("\b \b");         
        }

        private static void Hook_CharacterTyped(object sender, char e)
        {
            Console.Write(e);
        }

        private static void Hook_WordEnded(object sender, EventArgs e)
        {
            Console.WriteLine("");
        }
    }
}
