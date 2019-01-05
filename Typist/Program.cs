using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Typist
{
    class Program
    {        
        static void Main(string[] args)
        {
            KeyboardHook hook = new KeyboardHook();
            WordsInterceptor interceptor = new WordsInterceptor(hook);

            hook.KeyPressed += Hook_KeyPressed;          
            interceptor.WordTyped += Interceptor_WordTyped;

            Console.WriteLine("Intercepting keyboard input... press F12 to stop and see the results.");

            hook.enable();           
            Console.ReadLine();           
        }

        private static void Hook_KeyPressed(object sender, string key)
        {
            KeyboardHook hook = sender as KeyboardHook;
            switch (key)
            {
                case "F12":                    
                    hook.disable();
                    Console.WriteLine("Hook disabled.");                    
                    break;             
            }
        }

        private static void Interceptor_WordTyped(object sender, string e)
        {
            Console.WriteLine(e);
        }
     
    }
}
