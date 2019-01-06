using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Typist.WritingAnalyst;

namespace Typist
{
    class Program
    {
        private static KeyboardHook hook = new KeyboardHook();
        private static WordsInterceptor interceptor = new WordsInterceptor(hook);
        private static WritingAnalyst analyst = new WritingAnalyst(interceptor);

        static void Main(string[] args)
        {


            hook.KeyPressed += Hook_KeyPressed;
            interceptor.WordTyped += Interceptor_WordTyped;

            Console.WriteLine("Intercepting keyboard input... press F12 to stop and see the results.");

            hook.enable();
            Console.ReadLine();
        }

        private static void Interceptor_WordTyped(object sender, WordsInterceptor.WordTypedEventArgs e)
        {
            Console.WriteLine("{0}  ({1:N0} ms)", e.word, e.typingTime);
        }

        private static void Hook_KeyPressed(object sender, string key)
        {
            switch (key)
            {
                case "F12":
                    hook.disable();
                    showAnalysisResults();

                    break;
            }
        }

        private static void showAnalysisResults()
        {
            var results = analyst.analyze();

            // limit to top 20
            if (results.Count > 20) results = results.GetRange(0, 20);

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Word                 Median millis per char      Sample size\n");
            foreach (WordTypingStats wordData in results)
            {
                Console.WriteLine("{0,-20} {1,-27:D} {2, -10}{3}", wordData.word, wordData.medianMillisPerChar, wordData.sampleSize, "*");
            }
            Console.WriteLine("------------------------------------------------------------");
        }
    }
}
