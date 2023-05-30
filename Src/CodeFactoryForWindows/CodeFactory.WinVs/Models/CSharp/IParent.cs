using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Defines information how to find the parent model for the model that implements this C# model.
    /// </summary>
    public interface IParent
    {
        /// <summary>
        /// The parent to the current model. This will return null if there is no parent for this model, or the parent could not be located. 
        /// </summary>
        CsModel Parent { get; }
    }
}
