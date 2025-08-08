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
    /// Represents the result of processing a C# class, including its name, status, source, and the resulting class
    /// object.
    /// </summary>
    /// <remarks>This class is used to encapsulate the outcome of processing a C# class, such as whether it
    /// was created, updated, or skipped. It provides methods to update the result with new information and to validate
    /// the integrity of the result.</remarks>
    public class CsClassResult : CsSourceResult<CsClassResult>
    {
        /// <summary>
        /// The name of the class that was processed.
        /// </summary>
        public required string ClassName { get; init; }

        /// <summary>
        /// The class that was created or updated.
        /// </summary>
        public required CsClass Class { get; init; }

        /// <summary>
        /// Updates the current class result with an updated class result.  Use this to merge a 'created' result with an 'updated' result so it returns 'created' but with the most recent source code
        /// </summary>
        /// <param name="updatedResult">The updated class result</param>
        /// <returns></returns>
        public CsClassResult UpdateWith(CsClassResult updatedResult)
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

            return new CsClassResult
            {
                ClassName = updatedResult.ClassName,
                Status = status,
                Source = updatedResult.Source,
                Class = updatedResult.Class
            };
        }

        /// <summary>
        /// Validates the result of the class result.  This checks to ensure that the source and class are not null and throws an exception if they are.
        /// </summary>
        /// <param name="callerName">The method name that called the validation.</param>
        /// <returns></returns>
        /// <exception cref="CodeFactoryException">Raised if the source or the class models are null.</exception>
        public override void ValidateResult([CallerMemberName] string callerName = null)
        {
            string classDescriptor = string.Empty;
            if (Status == ResultStatusType.Created)
                classDescriptor = "newly created ";
            else if (Status == ResultStatusType.Updated)
                classDescriptor = "updated ";
            else if (Status == ResultStatusType.Skipped)
                classDescriptor = "";

            if (Source == null)
                throw new CodeFactoryException($"Failed to load source for {classDescriptor}'{ClassName}', cannot complete {callerName}.");
            if (Class == null)
                throw new CodeFactoryException($"Failed to load class for {classDescriptor}'{ClassName}', cannot complete {callerName}.");

        }
    }
}
