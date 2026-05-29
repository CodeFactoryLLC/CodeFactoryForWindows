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
        /// The timeout duration for regular expression operations. This is set to 2 seconds to prevent long-running regex operations from hanging the application, especially for complex patterns or large input strings. If a regex operation exceeds this timeout, it will throw a RegexMatchTimeoutException, which can be caught and handled appropriately by the caller.
        /// </summary>
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Cache for compiled regular expressions based on wildcard patterns to improve performance on repeated matches.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Regex> _regexCache = new ConcurrentDictionary<string, Regex>();

        /// <summary>
        /// Performs a wildcard match of the input string against the specified pattern, supporting **, *, and ? wildcards. Uses caching for compiled regular expressions to optimize performance on repeated patterns.
        /// </summary>
        /// <param name="input">The input string to match against the pattern.</param>
        /// <param name="pattern">The wildcard pattern to match. Supports **, *, and ? wildcards. If you want to use a raw regular expression, enclose it in slashes (/).</param>
        /// <param name="ignoreCase">Whether the match should be case-insensitive.</param>
        /// <remarks>
        /// When using ** in the pattern, it matches any sequence of characters, including dots. A single * matches any sequence of characters except path separators. A ? matches any single character. If the pattern does not contain any wildcard characters, a fast path is used for a simple case-insensitive equality check. For patterns with wildcards, compiled regular expressions are cached up to a specified limit to improve performance on repeated matches with the same pattern.
        /// </remarks>
        /// <returns>True if the input matches the pattern; otherwise, false.</returns>
        public static bool Matches(string input, string pattern, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(pattern)) return true;
            if (string.IsNullOrEmpty(input)) return false;

            string cacheKey = null;

            Regex regex = null;

            // Check if pattern is explicitly marked as a raw regex (enclosed in slashes)
            if (pattern.Length > 2 && pattern.StartsWith("/") && pattern.EndsWith("/"))
            {
                // Treat pattern as a raw regex if it is enclosed in slashes
                var regexPattern = pattern.Substring(1, pattern.Length - 2);

                if (string.IsNullOrWhiteSpace(regexPattern))
                    return true; // empty pattern matches everything, consistent with line 39

                // Wildcard path: use cached regex
                cacheKey = $"raw\0{pattern}\0{ignoreCase}";

                regex = _regexCache.Count >= MaxCacheSize
                    ? BuildCustomRegex(regexPattern, ignoreCase, compiled: false)
                    : _regexCache.GetOrAdd(cacheKey, _ => BuildCustomRegex(regexPattern, ignoreCase, compiled: true));

            }
            else
            {
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
                cacheKey = $"{pattern}\0{ignoreCase}";

                regex = _regexCache.Count >= MaxCacheSize
                    ? BuildRegex(pattern, ignoreCase, compiled: false)
                    : _regexCache.GetOrAdd(cacheKey, _ => BuildRegex(pattern, ignoreCase, compiled: true));
            }

            bool result = false;
            
            try
            { 
                result = regex.IsMatch(input);
            }
            catch (RegexMatchTimeoutException)
            {
                result = false;
            }

            return result;
        }


        /// <summary>
        /// Builds a regular expression from a glob-style pattern with wildcard support.    
        /// </summary>
        /// <param name="pattern">The glob-style pattern to convert. Supports ** (any characters), * (any characters except path separators), and ?
        /// (single character).</param>
        /// <param name="ignoreCase">Whether the regular expression should perform case-insensitive matching.</param>
        /// <param name="compiled">Whether to compile the regular expression for improved performance.</param>
        /// <returns>A regular expression that matches strings according to the specified glob pattern.</returns>
        private static Regex BuildRegex(string pattern, bool ignoreCase, bool compiled)
        {
            var regexPattern = "^" +
            Regex.Escape(pattern)
                 .Replace("\\*\\*", ".*")
                 .Replace("\\*", "[^/\\\\]*")   // exclude path separators, not dots
                 .Replace("\\?", ".") +
                 "$";

            var options = compiled
                ? RegexOptions.Compiled | (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None)
                : (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

            return new Regex(regexPattern, options,RegexTimeout);
        }


        /// <summary>
        /// Builds a regular expression directly from the provided pattern without any escaping, treating it as a raw regex pattern. 
        /// This is used when the pattern is explicitly marked as a regex by being enclosed in slashes (/pattern/). 
        /// The method applies the specified options for case sensitivity and compilation based on the parameters.
        /// </summary>
        /// <param name="pattern">The raw regex pattern to use.</param>
        /// <param name="ignoreCase">Whether the regex should ignore case.</param>
        /// <param name="compiled">Whether the regex should be compiled for performance.</param>
        /// <returns>A Regex object based on the provided pattern and options.</returns>
        private static Regex BuildCustomRegex(string pattern, bool ignoreCase, bool compiled)
        {
            var options = compiled
                ? RegexOptions.Compiled | (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None)
                : (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

            try
            {
                return new Regex(pattern, options, RegexTimeout);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid regular expression pattern: {pattern}", nameof(pattern), ex);
            }
        }

    }
}
