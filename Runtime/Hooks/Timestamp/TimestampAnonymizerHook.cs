/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI.Hooks
{
    /// <summary>
    /// xAPI hook for randomized timestamp anonymization to protect temporal privacy.
    /// Applies a randomly generated hour offset within configurable bounds to all statement
    /// timestamps, providing strong temporal privacy protection by making it impossible to
    /// predict the anonymization pattern while preserving relative timing relationships.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 5) Hooks / Timestamp Anonymizer Hook (xAPI)"),
     Description("Adds to the timestamp a random hour offset. This way the timestamps will be anonymized.")]   
    public class TimestampAnonymizerHook : xApiHook
    {
        /// <summary>
        /// Unity Start method that generates a random hour offset within the configured range.
        /// Called once at component initialization to establish a consistent but unpredictable
        /// offset that will be applied to all statements processed by this hook instance,
        /// ensuring temporal privacy while maintaining session consistency.
        /// </summary>
        private void Start()
        {
            hourOffset = Random.Range(hourOffsetMin, hourOffsetMax);
        }

        /// <summary>
        /// The randomly generated hour offset applied to all statement timestamps.
        /// Read-only field that displays the actual offset value used for anonymization,
        /// determined randomly within the configured min/max range during component initialization.
        /// </summary>
        [ReadOnly]
        public int hourOffset = 0;
        
        /// <summary>
        /// Minimum hour offset value for random anonymization range.
        /// Default of -720 hours (30 days in the past) provides strong temporal obfuscation
        /// while allowing for reasonable anonymization bounds and analytical utility preservation.
        /// </summary>
        public int hourOffsetMin = -24 * 30; // -30 days
        
        /// <summary>
        /// Maximum hour offset value for random anonymization range.
        /// Default of +720 hours (30 days in the future) creates a 60-day anonymization window
        /// that provides excellent temporal privacy protection while maintaining relative timing.
        /// </summary>
        public int hourOffsetMax = 24 * 30; // +30 days
        
        /// <summary>
        /// Processes xAPI statements by applying randomized timestamp anonymization.
        /// Uses the pre-generated random offset to modify statement timestamps consistently,
        /// providing robust temporal privacy protection while preserving the analytical value
        /// of relative timing relationships between learning events and session durations.
        /// </summary>
        /// <param name="statement">The xApiStatement to process for timestamp anonymization</param>
        /// <returns>xApiStatement with anonymized timestamp, or unchanged if no timestamp exists</returns>
        protected override xApiStatement AfterCompose(xApiStatement statement)
        {
            // Skip processing if statement has no timestamp to avoid null reference errors
            if (!statement.GetTimestamp().HasValue)
                return statement;
                
            // Apply the randomly generated hour offset for consistent anonymization
            var newTimestamp = statement.GetTimestamp()!.Value.AddHours(hourOffset);
            statement.WithTimestamp(newTimestamp);
            return statement;
        }
    }
}
#endif