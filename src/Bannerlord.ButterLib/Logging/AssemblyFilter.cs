using Newtonsoft.Json;

using Serilog.Core;
using Serilog.Events;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bannerlord.ButterLib.Logging
{
    internal class AssemblyFilter : ILogEventFilter
    {
        private readonly IReadOnlyList<string> _filter;

        public AssemblyFilter(IEnumerable<string> filter)
        {
            _filter = filter.ToList();
        }

        public bool IsEnabled(LogEvent logEvent)
        {
            if (logEvent.Properties.TryGetValue("Assemblies", out var property))
            {
                var assemblies = JsonConvert.DeserializeObject<List<string>>(property.ToString());
                return _filter.Any(x => assemblies.Any(y => MatchWildcardString(x, y)));
            }
            return true;
        }

        // https://www.codeproject.com/Tips/57304/Use-wildcard-Characters-and-to-Compare-Strings
        // Without Span this will create a huge memory pressure
        public static bool MatchWildcardString(string pattern, string input)
        {
            if (string.CompareOrdinal(pattern, input) == 0)
            {
                return true;
            }
            if (string.IsNullOrEmpty(input))
            {
                return string.IsNullOrEmpty(pattern.Trim('*'));
            }
            if (pattern.Length == 0)
            {
                return false;
            }
            if (pattern[0] == '?')
            {
                return MatchWildcardString(pattern.Substring(1), input.Substring(1));
            }
            if (pattern[pattern.Length - 1] == '?')
            {
                return MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input.Substring(0, input.Length - 1));
            }
            if (pattern[0] == '*')
            {
                return MatchWildcardString(pattern.Substring(1), input) || MatchWildcardString(pattern, input.Substring(1));
            }
            if (pattern[pattern.Length - 1] == '*')
            {
                return MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input) || MatchWildcardString(pattern, input.Substring(0, input.Length - 1));
            }
            if (pattern[0] == input[0])
            {
                return MatchWildcardString(pattern.Substring(1), input.Substring(1));
            }
            return false;
        }
        // Regex is slow in NET FX
        private static bool MatchWildcardStringRegex(string pattern, string input)
        {
            string regexPattern = pattern.Aggregate("^", (current, c) => current + c switch
            {
                '*' => ".*",
                '?' => ".",
                _ => $"[{c}]"
            });
            return new Regex($"{regexPattern}$").IsMatch(input);
        }
    }
}