using CodeFactory.WinVs.Models.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.WinVs.Factory.CSharp
{
    /// <summary>
    /// Represents the result of processing a C# container, including its name, status, source, and the resulting container
    /// object.
    /// </summary>
    /// <remarks>This class is used to encapsulate the outcome of processing a C# container, such as whether it
    /// was created, updated, or skipped. It provides methods to update the result with new information and to validate
    /// the integrity of the result.</remarks>
    public class CsContainerResult : CsSourceResult<CsContainerResult>
    {
        /// <summary>
        /// The name of the container that was processed.
        /// </summary>
        public required string ContainerName { get; init; }

        /// <summary>
        /// The container that was created or updated.
        /// </summary>
        public required CsContainer Container { get; init; }

        /// <summary>
        /// Updates the current container result with an updated container result.  Use this to merge a 'created' result with an 'updated' result so it returns 'created' but with the most recent source code
        /// </summary>
        /// <param name="updatedResult">The updated container result</param>
        /// <returns></returns>
        public CsContainerResult UpdateWith(CsContainerResult updatedResult)
        {
            if (updatedResult == null)
                return this;

            ResultStatusType status;

            if (Status == ResultStatusType.Created)
            {
                status = ResultStatusType.Created;
            }
            else if (updatedResult.Status == ResultStatusType.Skipped)
            {
                status = updatedResult.Status;
            }
            else
            {
                status = updatedResult.Status;
            }

            return new CsContainerResult
            {
                ContainerName = updatedResult.ContainerName,
                Status = status,
                Source = updatedResult.Source,
                Container = updatedResult.Container
            };
        }

        /// <summary>
        /// Validates the result of the container result.  This checks to ensure that the source and container are not null and throws an exception if they are.
        /// </summary>
        /// <param name="callerName">The method name that called the validation.</param>
        /// <returns></returns>
        /// <exception cref="CodeFactoryException">Raised if the source or the container models are null.</exception>
        public override void ValidateResult([CallerMemberName] string callerName = null)
        {
            string containerDescriptor = string.Empty;
            if (Status == ResultStatusType.Created)
                containerDescriptor = "newly created ";
            else if (Status == ResultStatusType.Updated)
                containerDescriptor = "updated ";
            else if (Status == ResultStatusType.Skipped)
                containerDescriptor = "";

            if (Source == null)
                throw new CodeFactoryException($"Failed to load source for {containerDescriptor}'{ContainerName}', cannot complete {callerName}.");
            if (Container == null)
                throw new CodeFactoryException($"Failed to load container for {containerDescriptor}'{ContainerName}', cannot complete {callerName}.");

        }
    }
}
