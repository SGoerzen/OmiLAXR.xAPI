/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
using System;
using System.Collections.Generic;
using OmiLAXR.Composers;

namespace OmiLAXR.xAPI.Endpoints
{
    /// <summary>
    /// Interface defining the contract for Learning Record Store (LRS) client implementations.
    /// Provides a standardized abstraction for sending xAPI statements to various LRS endpoints,
    /// enabling platform-specific optimizations while maintaining consistent functionality across
    /// different deployment targets (WebGL, desktop, mobile, etc.).
    /// </summary>
    public interface ILRSClient
    {
        /// <summary>
        /// Asynchronously sends a batch of xAPI statements to the configured Learning Record Store.
        /// Implementations should handle the conversion of statements to appropriate formats,
        /// manage HTTP communication, authentication, and error handling while reporting
        /// success or failure through the provided callback mechanism.
        /// </summary>
        /// <param name="statements">Collection of IStatement instances to transmit to the LRS</param>
        /// <param name="callback">Action callback to invoke with transmission result - true for success, false for failure</param>
        void SendStatements(List<IStatement> statements, Action<bool> callback);
    }
}