using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Contract all bounds checking code blocks must implement.
    /// </summary>
    public interface IBoundsCheckBlock:IBlock
    {
        /// <summary>
        /// Unique name assigned to identify the type of bounds check being performed.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Flag that determines if a bounds check should be ignored if the parameter has a default value set.
        /// </summary>
        bool IgnoreWhenDefaultValueIsSet { get; }

        /// <summary>
        /// The logger block assigned to this bounds check.
        /// </summary>
        ILoggerBlock LoggerBlock { get; }


        /// <summary>
        /// Generates the bounds check syntax if the parameter meets the criteria for a bounds check.
        /// </summary>
        /// <param name="sourceMethod">The target method the parameter belongs to.</param>
        /// <param name="checkParameter">The parameter to build the bounds check for.</param>
        /// <returns>Returns a tuple that contains a boolean that determines if the bounds check syntax was created for the parameter.</returns>
        (bool hasBoundsCheck, string boundsCheckSyntax) GenerateBoundsCheck(CsMethod sourceMethod,
            CsParameter checkParameter);
    }
}
