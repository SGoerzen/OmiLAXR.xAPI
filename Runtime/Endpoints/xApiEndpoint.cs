/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/

using System.Collections.Generic;
using OmiLAXR.Composers;
using OmiLAXR.Endpoints;
using OmiLAXR.xAPI.Composers;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Endpoints
{
    /// <summary>
    /// Abstract base class for all xAPI-specific endpoint implementations within the OmiLAXR analytics pipeline.
    /// Provides common xAPI functionality and ensures type safety by handling only xApiStatement instances.
    /// Serves as the foundation for concrete xAPI endpoints like Learning Record Store connections.
    /// </summary>
    public abstract class xApiEndpoint : Endpoint
    {
        /// <summary>
        /// Provides access to the xAPI Registry definitions for creating standardized xAPI statements.
        /// Acts as a static reference to the global xAPI contexts, activities, verbs, and extensions
        /// defined in the xAPI Registry for consistent statement generation across all xAPI endpoints.
        /// </summary>
        protected static xAPI_Contexts xapi => xApiRegistry.definitions;

        /// <summary>
        /// Handles the sending of generic IStatement instances by filtering for xAPI-specific statements.
        /// Ensures type safety by only processing xApiStatement instances and rejecting incompatible statement types.
        /// Delegates actual xAPI statement processing to the concrete implementation's HandleSending method.
        /// </summary>
        /// <param name="statement">The statement to be sent, which must be an xApiStatement instance</param>
        /// <returns>TransferCode indicating the result of the send operation or NoStatements for incompatible types</returns>
        protected override TransferCode HandleSending(IStatement statement)
        {
            // Reject non-xAPI statements to ensure type safety
            if (!(statement is xApiStatement))
                return TransferCode.NoStatements;
            
            // Cast to xApiStatement and delegate to concrete implementation
            var stmt = statement as xApiStatement;
            return HandleSending(stmt);
        }
        
        /// <summary>
        /// Abstract method that concrete xAPI endpoint implementations must override to handle xAPI statement transmission.
        /// Defines the contract for processing validated xApiStatement instances in endpoint-specific ways,
        /// such as sending to Learning Record Stores, local files, or other xAPI-compatible destinations.
        /// </summary>
        /// <param name="statement">The validated xApiStatement instance to be processed</param>
        /// <returns>TransferCode indicating the success or failure of the statement processing operation</returns>
        protected abstract TransferCode HandleSending(xApiStatement statement);
    }
}