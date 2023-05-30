using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp.Builder
{
    /// <summary>
    /// Base contract defintion all builders must inherit from.
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// The type of builder that has been implemented.
        /// </summary>
        BuilderType BuilderType { get; }
    }
}
