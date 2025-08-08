using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory
{
    /// <summary>
    /// Provides functionality to transform object names by removing specified prefixes and suffixes and optionally
    /// adding new ones.
    /// </summary>
    /// <remarks>The <see cref="ObjectNameTransformer"/> class is designed to modify object names based on
    /// configurable rules. It allows removing specific prefixes and suffixes from object names and adding new ones.
    /// This can be useful in scenarios where object names need to be normalized or adjusted to meet specific naming
    /// conventions.  Instances of this class are immutable and thread-safe. Use the static <see
    /// cref="Init(IEnumerable{string}, IEnumerable{string}, string, string)"/> or <see cref="Init(string, string,
    /// string, string)"/> methods to create configured instances.</remarks>
    public class ObjectNameTransformer
    {
        #region Backing fields

        /// <summary>
        /// Backing field for the property <see cref="RemovePrefixes"/>
        /// </summary>
        private readonly ImmutableList<string> _removePrefixes;

        /// <summary>
        /// Backing field for the property <see cref="RemoveSuffixes"/>
        /// </summary>
        private readonly ImmutableList<string> _removeSuffixes;

        /// <summary>
        /// Backing field for the property <see cref="AddPrefix"/>
        /// </summary>
        private readonly string _addPrefix;

        /// <summary>
        /// Backing field for the property <see cref="AddSuffix"/>
        /// </summary>
        private readonly string _addSuffix;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectNameTransformer"/> class with specified prefixes and
        /// suffixes to modify object names.
        /// </summary>
        /// <remarks>This constructor allows customization of how object names are transformed by
        /// specifying prefixes and suffixes to remove or add. The provided collections for prefixes and suffixes are
        /// converted to immutable lists for thread safety.</remarks>
        /// <param name="removePrefixes">A collection of prefixes to remove from object names. If null, no prefixes will be removed.</param>
        /// <param name="removeSuffixes">A collection of suffixes to remove from object names. If null, no suffixes will be removed.</param>
        /// <param name="addPrefix">A prefix to add to object names. Can be null or empty if no prefix should be added.</param>
        /// <param name="addSuffix">A suffix to add to object names. Can be null or empty if no suffix should be added.</param>
        protected ObjectNameTransformer(IEnumerable<string> removePrefixes, IEnumerable<string> removeSuffixes, string addPrefix, string addSuffix)
        {
            _removePrefixes = removePrefixes != null ? ImmutableList<string>.Empty.AddRange(removePrefixes) : ImmutableList<string>.Empty;
            _removeSuffixes = removeSuffixes != null ? ImmutableList<string>.Empty.AddRange(removeSuffixes) : ImmutableList<string>.Empty;
            _addPrefix = addPrefix;
            _addSuffix = addSuffix;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectNameTransformer"/> class with the specified prefixes and
        /// suffixes to remove or add.
        /// </summary>
        /// <param name="removePrefixes">A collection of prefixes to remove from object names. Cannot be null.</param>
        /// <param name="removeSuffixes">A collection of suffixes to remove from object names. Cannot be null.</param>
        /// <param name="addPrefix">The prefix to add to object names. Can be null or empty if no prefix should be added.</param>
        /// <param name="addSuffix">The suffix to add to object names. Can be null or empty if no suffix should be added.</param>
        /// <returns>An instance of the <see cref="ObjectNameTransformer"/> class configured with the specified parameters.</returns>
        public static ObjectNameTransformer Init(IEnumerable<string> removePrefixes, IEnumerable<string> removeSuffixes, string addPrefix, string addSuffix)
        {
            return new ObjectNameTransformer(removePrefixes, removeSuffixes, addPrefix, addSuffix);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectNameTransformer"/> class with specified prefixes and
        /// suffixes to modify object names.
        /// </summary>
        /// <param name="removePrefixes">A comma-separated list of prefixes to remove from object names. Can be <see langword="null"/> or empty.</param>
        /// <param name="removeSuffixes">A comma-separated list of suffixes to remove from object names. Can be <see langword="null"/> or empty.</param>
        /// <param name="addPrefix">A prefix to add to object names. Can be <see langword="null"/> or empty.</param>
        /// <param name="addSuffix">A suffix to add to object names. Can be <see langword="null"/> or empty.</param>
        /// <returns>An instance of <see cref="ObjectNameTransformer"/> configured with the specified prefixes and suffixes.</returns>
        public static ObjectNameTransformer Init(string removePrefixes, string removeSuffixes, string addPrefix, string addSuffix)
        {
            return new ObjectNameTransformer(removePrefixes != null ? removePrefixes.Split(',') : null, removeSuffixes != null ? removeSuffixes.Split(',') : null, addPrefix, addSuffix);
        }


        /// <summary>
        /// List of prefixes to remove from the beginning of the objects name.
        /// </summary>
        public IReadOnlyList<string> RemovePrefixes => _removePrefixes;

        /// <summary>
        /// List of suffixes to remove from the end of the objects name.
        /// </summary>
        public IReadOnlyList<string> RemoveSuffixes => _removeSuffixes;

        /// <summary>
        /// The prefix to append to the beginning of the objects name;
        /// </summary>
        public string AddPrefix => _addPrefix;

        /// <summary>
        /// The suffix to append to the end of the objects name.
        /// </summary>
        public string AddSuffix => _addSuffix;


        /// <summary>
        /// Formats the specified object name by applying optional prefix and suffix modifications.
        /// </summary>
        /// <remarks>The method performs the following transformations in order: <list type="bullet">
        /// <item><description>Trims leading and trailing whitespace from the input name.</description></item>
        /// <item><description>Removes any prefixes specified in the <c>RemovePrefixes</c> collection if they match the
        /// start of the name.</description></item> <item><description>Removes any suffixes specified in the
        /// <c>RemoveSuffixes</c> collection if they match the end of the name.</description></item>
        /// <item><description>Adds the <c>AddPrefix</c> value to the start of the name, if
        /// specified.</description></item> <item><description>Adds the <c>AddSuffix</c> value to the end of the name,
        /// if specified.</description></item> </list> If <paramref name="defaultPrefix"/> is provided, it will be
        /// prepended to the final result unless it was already removed earlier.</remarks>
        /// <param name="name">The original name to format. Cannot be null or empty.</param>
        /// <param name="defaultPrefix">An optional default prefix to prepend to the formatted name. If the name already starts with this prefix, 
        /// it will be removed before further processing.</param>
        /// <returns>A formatted string that includes any applied prefixes or suffixes. If <paramref name="defaultPrefix"/> is
        /// provided,  it will be prepended to the final result.</returns>
        public string FormatObjectName(string name, string defaultPrefix = null)
        {

            if (string.IsNullOrEmpty(name)) return name;

            string formattedName = name.Trim();

            if (defaultPrefix != null)
            {
                if (formattedName.StartsWith(defaultPrefix))
                {
                    formattedName = formattedName.Substring(defaultPrefix.Length);
                }
            }

            if (RemovePrefixes.Any())
            {
                string removePrefix = RemovePrefixes.FirstOrDefault(p => formattedName.StartsWith(p));

                if (removePrefix != null) formattedName = formattedName.Substring(removePrefix.Length);
            }

            if (RemoveSuffixes.Any())
            {
                string removeSuffix = RemoveSuffixes.FirstOrDefault(p => formattedName.EndsWith(p));

                if (removeSuffix != null) formattedName = formattedName.Substring(0, formattedName.Length - (removeSuffix.Length));
            }

            if (!string.IsNullOrEmpty(AddPrefix)) formattedName = $"{AddPrefix}{formattedName}";

            if (!string.IsNullOrEmpty(AddSuffix)) formattedName = $"{formattedName}{AddSuffix}";

            return defaultPrefix != null
                ? $"{defaultPrefix}{formattedName}"
                : formattedName;
        }
    }
}
