using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Memoriae.BAL.PostgreSQL.Extensions
{
    public static class StringExtensions
    {
        public static string WrapWordsInTag(this string originalText, List<string> searchWords)
        {
            var pattern = string.Join("|", searchWords.Select(Regex.Escape));
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var matches = regex.Matches(originalText);
            return matches.Count <= 0
                       ? originalText
                       : matches.GroupBy(m => m.Value).Select(ind => ind.First()).Aggregate(
                           originalText,
                           (current, match) => current.Replace(match.Value, $"<em>{match}</em>"));
        }      

        public static bool CaseInsensitiveContains(
            this string text,
            string value,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase) =>
            text.IndexOf(value, stringComparison) >= 0;
    }
}
