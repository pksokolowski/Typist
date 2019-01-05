using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typist
{
    /// <summary>
    /// Intercepts keyboard input and builds words from it, ignoring characters that aren't letters, and 
    /// firing an event upon word completion, as judged by use of a return, comma or perior characters.
    /// It handles errors correction (backspace) as well as captures additional statistics regarding 
    /// writing performance.
    /// </summary>
    class WordsInterceptor
    {
        private KeyboardHook hook;

        private List<string> WORD_ENDING_KEYS = new List<string> { "Return", "Space", "OemPeriod", "Oemcomma" };

        private const string KEY_BACKSPACE = "Back";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hook">A keyboard hook object to use for input, 
        /// make sure it's enabled when you want to intercept words written</param>
        public WordsInterceptor(KeyboardHook hook)
        {
            this.hook = hook;
            hook.KeyPressed += Hook_KeyPressed;           
        }

        private void Hook_KeyPressed(object sender, string key)
        {
            if (key.Length == 1)
            {
                addChar(Char.Parse(key));
            }
            else if (WORD_ENDING_KEYS.Contains(key))
            {
                endWord();
            }
            else if (key == KEY_BACKSPACE)
            {
                removeChar();
            }
        }

        private String currentWord = "";
        private double startTimeStamp = 0;

        private void addChar(char c)
        {
            if (currentWord.Length == 0)
            {
                setupForNewWord();
            }
            currentWord += c;
        }

        private void endWord()
        {            
            if (currentWord.Length > 1)
            {
                OnWordTyped(currentWord);
            }

            reset();
        }

        private void removeChar()
        {
            if (currentWord.Length == 0)
            {
                // specjal case when there is noting to delete as the entire word
                // is gone already
                reset();
                return;
            }
            currentWord = currentWord.Substring(0, currentWord.Length - 1);
        }

        private void reset()
        {
            currentWord = "";
        }

        private void setupForNewWord()
        {
            // prepare values anew
            startTimeStamp = DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
        }

        protected virtual void OnWordTyped(String word)
        {
            EventHandler<String> handler = WordTyped;
            if (handler != null)
            {
                handler(this, word);
            }
        }

        public event EventHandler<String> WordTyped;
    }
}
