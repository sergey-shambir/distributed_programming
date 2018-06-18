using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using StackExchange.Redis;

namespace TextLib
{
    public class TextMetrics
    {
        const string RuVowels = "оиаыюяэёуе";
        const string EnVowels = "aeiouy";

        const string RuConsonants = "бвгджзйклмнпрстфхцчшщ";
        const string EnConsonants = "bcdfghjklmnpqrstvwxz";

        public float CalculateScore(string text)
        {
            float vowelCount = (float)GetVowelCount(text);
            float consonantsCount = (float)GetConsonantsCount(text);

            return vowelCount / consonantsCount;
        }

        public int GetVowelCount(string text)
        {
            return text.Count(c => RuVowels.Contains(c) || EnVowels.Contains(c));
        }

        public int GetConsonantsCount(string text)
        {
            return text.Count(c => RuConsonants.Contains(c) || EnConsonants.Contains(c));
        }
    }
}


