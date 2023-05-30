using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base class implementation for generation of bounds checking logic.
    /// </summary>
    public abstract class BaseBoundsCheckBlock:IBoundsCheckBlock
    {
        private readonly string _name;
        private readonly bool _ignoreWhenDefaultValueIsSet;
        private readonly ILoggerBlock _loggerBlock;

        /// <summary>
        /// Initializes the base class for the bounds check.
        /// </summary>
        /// <param name="name">The unique name that identifies the type of bounds check being implemented.</param>
        /// <param name="ignoreWhenDefaultValueIsSet">Flag that determines if the bounds checking should be ignored if a default value is set.</param>
        /// <param name="loggerBlock">Logger block used with bounds check logic.</param>
        protected BaseBoundsCheckBlock(string name, bool ignoreWhenDefaultValueIsSet, ILoggerBlock loggerBlock)
        {
            _name = name;
            _ignoreWhenDefaultValueIsSet = ignoreWhenDefaultValueIsSet;
            _loggerBlock = loggerBlock;
        }


        /// <summary>
        /// The type of code block that has been implemented.
        /// </summary>
        public CodeBlockType BlockType => CodeBlockType.BoundsCheck;

        /// <summary>
        /// Unique name assigned to identify the type of bounds check being performed.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Flag that determines if a bounds check should be ignored if the parameter has a default value set.
        /// </summary>
        public bool IgnoreWhenDefaultValueIsSet => _ignoreWhenDefaultValueIsSet;

        /// <summary>
        /// The logger block assigned to this bounds check.
        /// </summary>
        public ILoggerBlock LoggerBlock => _loggerBlock;

        /// <summary>
        /// Generates the bounds check syntax if the parameter meets the criteria for a bounds check.
        /// </summary>
        /// <param name="sourceMethod">The target method the parameter belongs to.</param>
        /// <param name="checkParameter">The parameter to build the bounds check for.</param>
        /// <returns>Returns a tuple that contains a boolean that determines if the bounds check syntax was created for the parameter.</returns>
        public abstract (bool hasBoundsCheck, string boundsCheckSyntax) GenerateBoundsCheck(CsMethod sourceMethod,
            CsParameter checkParameter);
    }
}
