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

        private List<string> WORD_ENDING_KEYS = new List<string> { "Return", "Space", "OemPeriod", "Oemcomma", "OemQuestion", "D1" };

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

        private StringBuilder currentWord = new StringBuilder();
        private double startTimeStamp = 0;

        private void addChar(char c)
        {
            if (currentWord.Length == 0)
            {
                setupForNewWord();
            }
            currentWord.Append(c);
        }

        private void endWord()
        {            
            if (currentWord.Length > 1)
            {
                var timeNow = getTimeNow();
                var typingDuration = timeNow - startTimeStamp;
                var eventArgs = new WordTypedEventArgs() { word = currentWord.ToString(), typingTime = typingDuration };
                OnWordTyped(eventArgs);
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
            currentWord = currentWord.Remove(currentWord.Length - 1, 1);
        }

        private void reset()
        {
            currentWord.Clear();
        }

        private void setupForNewWord()
        {
            // prepare values anew
            startTimeStamp = getTimeNow();
        }

        private Double getTimeNow()
        {
            return DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
        }

        protected virtual void OnWordTyped(WordTypedEventArgs e)
        {
            EventHandler<WordTypedEventArgs> handler = WordTyped;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<WordTypedEventArgs> WordTyped;

        public class WordTypedEventArgs : EventArgs
        {
            public string word { get; set; }
            public double typingTime { get; set; }
        }
    }
}
