using System.Collections.Generic;
using System.Linq;
using static Typist.WordsInterceptor;

namespace Typist
{
    class WritingAnalyst
    {
        private Dictionary<string, List<double>> typingTimes = new Dictionary<string, List<double>>();

        private WordsInterceptor interceptor;

        public WritingAnalyst(WordsInterceptor interceptor)
        {
            this.interceptor = interceptor;
            interceptor.WordTyped += Interceptor_WordTyped;
        }

        private void Interceptor_WordTyped(object sender, WordTypedEventArgs e)
        {
            // add entry in dictionary if missing
            if (!typingTimes.ContainsKey(e.word))
            {
                typingTimes.Add(e.word, new List<double>());
            }

            // add the event to the collection for later analysis
            typingTimes[e.word].Add(e.typingTime);
        }

        public List<WordTypingStats> analyze()
        {           
            var keys = typingTimes.Keys;
            var results = new List<WordTypingStats>();

            foreach(string key in keys)
            {
                var data = typingTimes[key];
                var median = MedianCalculator.median(data);
                var medianTimePerChar = median / key.Length;

                var result = new WordTypingStats() { word = key, medianMillisPerChar = (int)medianTimePerChar, sampleSize = data.Count };
                results.Add(result);
            }

            // order by typing speed, from the slowest to the fastest
            var sortedResults = results.OrderByDescending(o => o.medianMillisPerChar).ToList();

            return sortedResults;
        }

        public class WordTypingStats
        {
            public string word { get; set; }
            public int medianMillisPerChar { get; set; }
            public int sampleSize { get; set; }

        }
    }
}
