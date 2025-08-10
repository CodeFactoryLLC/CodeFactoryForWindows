using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Base class for building syntax representations based on a specified request.
    /// </summary>
    /// <typeparam name="R">Type of the syntax request that is passed in when building the syntax.</typeparam>
    public abstract class CsSyntaxBuilder<R> : ICsSyntaxBuilder<R> where R : ISyntaxRequest
    {
        /// <summary>
        /// Asynchronously builds a syntax representation of the specified request.
        /// </summary>
        /// <param name="request">The request object containing the data to be processed into a syntax representation. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the syntax representation as a
        /// string.</returns>
        public abstract Task<string> BuildSyntaxAsync(R request);
        
    }
}
