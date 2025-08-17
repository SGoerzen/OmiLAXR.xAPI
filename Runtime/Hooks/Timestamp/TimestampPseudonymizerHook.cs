/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
using System.ComponentModel;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI.Hooks
{
    /// <summary>
    /// xAPI hook for pseudonymizing statement timestamps to protect temporal privacy.
    /// Applies a configurable hour offset to all statement timestamps, making it difficult
    /// to correlate learning analytics data with specific real-world dates while preserving
    /// relative temporal relationships between learning events for analysis purposes.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 5) Hooks / Timestamp Pseudonymizer Hook (xAPI)"),
     Description("Adds to the timestamp an hour offset (default: 10 days ago). This way the timestamps will be pseudonymized.")]   
    public class TimestampPseudonymizerHook : xApiHook
    {
        /// <summary>
        /// Hour offset to apply to all statement timestamps for pseudonymization.
        /// Default value of -240 hours (10 days) shifts all timestamps into the past,
        /// making it difficult to identify the actual collection date while maintaining
        /// temporal sequence and duration information for learning analytics.
        /// </summary>
        public int hourOffset = -24 * 10; // -10 days default offset
        
        /// <summary>
        /// Processes xAPI statements by applying timestamp pseudonymization if a timestamp exists.
        /// Modifies the statement's timestamp by adding the configured hour offset,
        /// providing temporal privacy protection while preserving relative timing relationships
        /// essential for learning pattern analysis and session duration calculations.
        /// </summary>
        /// <param name="statement">The xApiStatement to process for timestamp pseudonymization</param>
        /// <returns>xApiStatement with pseudonymized timestamp, or unchanged if no timestamp exists</returns>
        protected override xApiStatement AfterCompose(xApiStatement statement)
        {
            // Skip processing if statement has no timestamp to avoid null reference errors
            if (!statement.GetTimestamp().HasValue)
                return statement;
                
            // Apply hour offset to create pseudonymized timestamp
            var newTimestamp = statement.GetTimestamp()!.Value.AddHours(hourOffset);
            statement.WithTimestamp(newTimestamp);
            return statement;
        }
    }
}