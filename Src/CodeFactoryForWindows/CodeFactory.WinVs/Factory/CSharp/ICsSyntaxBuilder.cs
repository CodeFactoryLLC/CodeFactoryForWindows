using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Defines a contract for building syntax based on a specified request.
    /// </summary>
    /// <typeparam name="R">The type of the syntax request, which must implement <see cref="ISyntaxRequest"/>.</typeparam>
    public interface ICsSyntaxBuilder<R> where R : ISyntaxRequest
    {
        /// <summary>
        /// Asynchronously builds a syntax representation of the specified request.
        /// </summary>
        /// <param name="request">The request object containing the data to be processed into a syntax representation. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the syntax representation as a
        /// string.</returns>
        Task<string> BuildSyntaxAsync(R request);
    }

}
