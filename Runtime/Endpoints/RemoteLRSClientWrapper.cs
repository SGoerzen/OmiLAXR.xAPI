/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using OmiLAXR.Composers;
using OmiLAXR.xAPI.Composers;
using OmiLAXR.xAPI.Extensions;
using TinCan;

namespace OmiLAXR.xAPI.Endpoints
{
    /// <summary>
    /// Wrapper implementation for Remote Learning Record Store (LRS) client functionality.
    /// Provides a standardized interface for sending xAPI statements to remote LRS endpoints
    /// using the TinCan library's RemoteLRS implementation for HTTP-based xAPI communication.
    /// </summary>
    public class RemoteLRSClientWrapper : ILRSClient
    {
        /// <summary>
        /// The underlying TinCan RemoteLRS instance responsible for HTTP communication with the LRS endpoint.
        /// Handles authentication, request formatting, and response processing according to xAPI specifications.
        /// </summary>
        private readonly RemoteLRS _remoteLrs;

        /// <summary>
        /// Initializes a new RemoteLRSClientWrapper instance with the specified RemoteLRS configuration.
        /// The provided RemoteLRS should be pre-configured with endpoint URL and authentication credentials.
        /// </summary>
        /// <param name="remoteLrs">Configured RemoteLRS instance for handling HTTP communication with the LRS</param>
        public RemoteLRSClientWrapper(RemoteLRS remoteLrs)
        {
            _remoteLrs = remoteLrs;
        }

        /// <summary>
        /// Asynchronously sends a batch of xAPI statements to the remote Learning Record Store.
        /// Converts OmiLAXR xAPI statements to TinCan format, transmits them via HTTP,
        /// and reports success or failure through the provided callback mechanism.
        /// </summary>
        /// <param name="statements">Collection of IStatement instances (expected to be xApiStatement) to transmit</param>
        /// <param name="callback">Action callback to invoke with transmission result - true for success, false for failure</param>
        public void SendStatements(List<IStatement> statements, Action<bool> callback)
        {
            // Convert OmiLAXR xAPI statements to TinCan Statement format for transmission
            var stmts = statements.Select(s => (s as xApiStatement).ToTinCanStatement()).ToList();
            
            // Attempt to save the converted statements to the remote LRS via HTTP
            var resp = _remoteLrs.SaveStatements(stmts);

            // Handle transmission failure scenarios
            if (!resp.success)
            {
                // Log comprehensive error information for debugging and monitoring
                DebugLog.xAPI.Error($"Error Message: {resp.errMsg}, Http Error: {resp.httpException}");
                callback(false);
                return;
            }

            // Report successful transmission to the caller
            callback(true);
        }
    }
}