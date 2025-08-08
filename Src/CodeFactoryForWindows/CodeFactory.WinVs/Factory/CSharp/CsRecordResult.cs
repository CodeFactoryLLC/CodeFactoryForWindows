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
    /// Represents the result of processing a C# record, including its name, status, source, and the resulting record
    /// object.
    /// </summary>
    /// <remarks>This class is used to encapsulate the outcome of processing a C# record, such as whether it
    /// was created, updated, or skipped. It provides methods to update the result with new information and to validate
    /// the integrity of the result.</remarks>
    public class CsRecordResult : CsSourceResult<CsRecordResult>
    {
        /// <summary>
        /// The name of the record that was processed.
        /// </summary>
        public required string RecordName { get; init; }

        /// <summary>
        /// The record that was created or updated.
        /// </summary>
        public required CsRecord Record { get; init; }


        /// <summary>
        /// Updates the current record result with an updated record result.  Use this to merge a 'created' result with an 'updated' result so it returns 'created' but with the most recent source code
        /// </summary>
        /// <param name="updatedResult">The updated record result</param>
        /// <returns></returns>
        public CsRecordResult UpdateWith(CsRecordResult updatedResult)
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

            return new CsRecordResult
            {
                RecordName = updatedResult.RecordName,
                Status = status,
                Source = updatedResult.Source,
                Record = updatedResult.Record
            };
        }

        /// <summary>
        /// Validates the result of the record result.  This checks to ensure that the source and record are not null and throws an exception if they are.
        /// </summary>
        /// <param name="callerName">The method name that called the validation.</param>
        /// <returns></returns>
        /// <exception cref="CodeFactoryException">Raised if the source or the record models are null.</exception>
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
                throw new CodeFactoryException($"Failed to load source for {classDescriptor}'{RecordName}', cannot complete {callerName}.");
            if (Record == null)
                throw new CodeFactoryException($"Failed to load record for {classDescriptor}'{RecordName}', cannot complete {callerName}.");

        }
    }
}
