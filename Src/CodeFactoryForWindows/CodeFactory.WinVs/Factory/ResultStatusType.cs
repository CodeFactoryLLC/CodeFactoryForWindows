using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Factory
{
    /// <summary>
    /// Enumeration that defines the possible result status types for factory results.
    /// </summary>
    public enum ResultStatusType
    {
        /// <summary>
        /// The content was created by the factory.
        /// </summary>
        Created = 0,

        /// <summary>
        /// The content was updated by the factory.
        /// </summary>
        Updated = 1,

        /// <summary>
        /// The content was not created or updated by the factory, but was skipped due to the factory not requiring to make any changes.
        /// </summary>
        Skipped = 2
    }
}
