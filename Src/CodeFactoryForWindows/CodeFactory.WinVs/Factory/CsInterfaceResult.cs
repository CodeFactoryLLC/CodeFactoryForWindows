//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2025 CodeFactory, LLC
//*****************************************************************************

using CodeFactory.WinVs.Models.CSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CodeFactory.WinVs.Factory
{
    /// <summary>
    /// Represents the result of processing a C# interface, including its name, status, source, and the resulting
    /// interface object.
    /// </summary>
    /// <remarks>This class provides information about the outcome of processing a C# interface, such as
    /// whether it was created, updated, or skipped. It also includes methods for updating the result with new data and
    /// validating the result to ensure it is complete and ready for further processing.</remarks>
    public class CsInterfaceResult : ISourceResult<CsInterfaceResult>
    {
        /// <summary>
        /// The name of the interface that was processed.
        /// </summary>
        public required string InterfaceName { get; init; }

        /// <summary>
        /// The status of the interface result, this indicates if the interface was created, updated or skipped.
        /// </summary>
        public ResultStatusType Status { get; init; }

        /// <summary>
        /// The source that was used to create the interface
        /// </summary>
        public CsSource Source { get; init; }

        /// <summary>
        /// The interface that was created or updated.
        /// </summary>
        public CsInterface Interface { get; init; }

        /// <summary>
        /// Updates the current interface result with an updated interface result.  Use this to merge a 'created' result with an 'updated' result so it returns 'created' but with the most recent source code
        /// </summary>
        /// <param name="updatedResult">The updated interface result</param>
        public CsInterfaceResult UpdateWith(CsInterfaceResult updatedResult)
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

            return new CsInterfaceResult
            {
                InterfaceName = updatedResult.InterfaceName,
                Status = status,
                Source = updatedResult.Source,
                Interface = updatedResult.Interface
            };
        }


        /// <summary>
        /// Validates the current interface result to ensure that the source and interface are not null. This is used to ensure that the result is valid before further processing.
        /// </summary>
        /// <param name="callerName">The name of the method that called the validation.</param>
        /// <exception cref="CodeFactoryException">Raised if the source or the interface is null.</exception>
        public void ValidateResult([CallerMemberName] string callerName = null)
        {
            string interfaceDescriptor = string.Empty;
            if (Status == ResultStatusType.Created)
                interfaceDescriptor = "newly created ";
            else if (Status == ResultStatusType.Updated)
                interfaceDescriptor = "updated ";
            else if (Status == ResultStatusType.Skipped)
                interfaceDescriptor = "";

            if (Source == null)
                throw new CodeFactoryException($"Failed to load source for {interfaceDescriptor}'{InterfaceName}', cannot complete {callerName}.");
            if (Interface == null)
                throw new CodeFactoryException($"Failed to load class for {interfaceDescriptor}'{InterfaceName}', cannot complete {callerName}.");
        }
    }
}
