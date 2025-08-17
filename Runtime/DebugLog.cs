/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
namespace OmiLAXR.xAPI
{
    /// <summary>
    /// Static utility class providing centralized debug logging functionality for xAPI operations.
    /// Creates a dedicated logging channel specifically for xAPI-related activities within the
    /// OmiLAXR analytics framework, enabling filtered debugging and monitoring of xAPI statement
    /// processing, transmission, and error handling throughout the analytics pipeline.
    /// </summary>
    public static class DebugLog
    {
        /// <summary>
        /// Dedicated debug logger instance for all xAPI-related operations and error reporting.
        /// Provides categorized logging with "OmiLAXR.xAPI" prefix for easy identification
        /// and filtering of xAPI-specific debug information, errors, and status messages
        /// during development, testing, and production monitoring.
        /// </summary>
        public static readonly OmiLAXR.DebugLog xAPI = new OmiLAXR.DebugLog("OmiLAXR.xAPI");
    }
}