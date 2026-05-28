using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace CodeFactory.WinVs
{
    /// <summary>
    /// Global helper for matching strings against wildcard patterns with support for **, *, ? and caching compiled regexes for performance.
    /// </summary>
    public static class WildcardSearch
    {
        /// <summary>
        /// The maximum number of compiled regular expressions to cache. Once this limit is reached, new patterns will be processed without caching to prevent unbounded memory growth.
        /// </summary>
        private const int MaxCacheSize = 512; // Limit cache size to prevent memory bloat

        /// <summary>
        /// Cache for compiled regular expressions based on wildcard patterns to improve performance on repeated matches.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Regex> _regexCache = new ConcurrentDictionary<string, Regex>();

        /// <summary>
        /// Performs a wildcard match of the input string against the specified pattern, supporting **, *, and ? wildcards. Uses caching for compiled regular expressions to optimize performance on repeated patterns.
        /// </summary>
        /// <param name="input">The input string to match against the pattern.</param>
        /// <param name="pattern">The wildcard pattern to match. Supports **, *, and ? wildcards.</param>
        /// <param name="ignoreCase">Whether the match should be case-insensitive.</param>
        /// <remarks>
        /// When using ** in the pattern, it matches any sequence of characters, including dots. A single * matches any sequence of characters except dots. A ? matches any single character. If the pattern does not contain any wildcard characters, a fast path is used for a simple case-insensitive equality check. For patterns with wildcards, compiled regular expressions are cached up to a specified limit to improve performance on repeated matches with the same pattern.
        /// </remarks>
        /// <returns>True if the input matches the pattern; otherwise, false.</returns>
        public static bool Matches(string input, string pattern, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(pattern)) return true;
            if (string.IsNullOrEmpty(input)) return false;

            // Check if pattern contains any wildcard characters
            bool hasWildcard = pattern.Contains("*") || pattern.Contains("?");

            if (!hasWildcard)
            {
                // Fast path: standard case-insensitive exact match
                return input.Equals(pattern, ignoreCase
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal);
            }

            // Wildcard path: use cached regex
            var cacheKey = $"{pattern}\0{ignoreCase}";

            Regex regex = _regexCache.Count >= MaxCacheSize
                ? BuildRegex(pattern, ignoreCase, compiled: false)
                : _regexCache.GetOrAdd(cacheKey, _ => BuildRegex(pattern, ignoreCase, compiled: true));

            return regex.IsMatch(input);
        }


        /// <summary>
        /// Builds a regular expression from a glob-style pattern with wildcard support.    
        /// </summary>
        /// <param name="pattern">The glob-style pattern to convert. Supports ** (any characters), * (any characters except dot), and ?
        /// (single character).</param>
        /// <param name="ignoreCase">Whether the regular expression should perform case-insensitive matching.</param>
        /// <param name="compiled">Whether to compile the regular expression for improved performance.</param>
        /// <returns>A regular expression that matches strings according to the specified glob pattern.</returns>
        private static Regex BuildRegex(string pattern, bool ignoreCase, bool compiled)
        {
            var regexPattern = "^" +
                Regex.Escape(pattern)
                     .Replace("\\*\\*", ".*")
                     .Replace("\\*", "[^.]*")
                     .Replace("\\?", ".") +
                "$";

            var options = compiled
                ? RegexOptions.Compiled | (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None)
                : (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

            return new Regex(regexPattern, options);
        }

    }
}
