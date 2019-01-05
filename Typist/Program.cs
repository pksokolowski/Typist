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
            WordsInterceptor interceptor = new WordsInterceptor(hook);

            interceptor.WordTyped += Interceptor_WordTyped;       

            hook.enable();
            Console.ReadLine();
            hook.disable();
        }

        private static void Interceptor_WordTyped(object sender, string e)
        {
            Console.WriteLine(e);
        }
     
    }
}
