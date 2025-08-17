/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System;
using System.ComponentModel;
using OmiLAXR.Types;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI.Hooks
{
    /// <summary>
    /// xAPI hook for advanced timestamp pseudonymization using a fixed reference date.
    /// Provides sophisticated temporal privacy protection by mapping all statement timestamps
    /// to a configurable reference date while preserving relative timing relationships
    /// and session durations essential for learning analytics analysis.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 5) Hooks / Reference Timestamp Pseudonmizer (xAPI)"),
     Description("Sets a fixed Date to the timestamp, thus the day of the collected data is pseudonymized.")]
    public class ReferenceTimestampPseudonmizerHook : xApiHook
    {
        /// <summary>
        /// Stores the first encountered timestamp for calculating elapsed time offsets.
        /// Used as the baseline for mapping all subsequent timestamps to maintain
        /// relative temporal relationships while anonymizing absolute dates.
        /// </summary>
        private DateTime? _firstDateTime;
        
        /// <summary>
        /// Determines whether to preserve original time-of-day information during anonymization.
        /// When true, maps timestamps to the reference date while keeping hours/minutes/seconds.
        /// When false, maps the first statement to midnight and calculates elapsed times from there.
        /// </summary>
        [Tooltip("The time of the first statement will be by default 00:00:00.000, except you set Keep original time true.")]
        public bool keepOriginalTime = false;
        
        /// <summary>
        /// Reference date to use for timestamp anonymization mapping.
        /// All statement timestamps will be mapped to dates relative to this reference,
        /// providing consistent pseudonymization while preserving analytical value.
        /// </summary>
        public SerializableDateTime referenceDateTime = SerializableDateTime.Now;
        
        /// <summary>
        /// Anonymizes a timestamp by mapping it to the reference date with configurable time preservation.
        /// Implements two anonymization strategies: preserving original time-of-day or calculating
        /// elapsed time from the first statement, both maintaining temporal relationships for analysis.
        /// </summary>
        /// <param name="dateTime">Original timestamp to anonymize</param>
        /// <returns>Anonymized timestamp mapped to reference date</returns>
        private DateTime AnonymizeTimestamp(DateTime dateTime)
        {
            // Store first timestamp for elapsed time calculations if not preserving original time
            if (!keepOriginalTime && !_firstDateTime.HasValue)
            {
                _firstDateTime = dateTime;
            }
    
            var rdt = referenceDateTime;
    
            if (keepOriginalTime)
            {
                // Preserve time-of-day by mapping to reference date with original hours/minutes/seconds
                return new DateTime(
                    rdt.year,
                    rdt.month,
                    rdt.day,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second,
                    dateTime.Millisecond);
            }
            else
            {
                // Calculate elapsed time since first timestamp to preserve relative timing
                var elapsed = dateTime - _firstDateTime!.Value;
        
                // Create anonymized timestamp using reference date plus elapsed time offset
                return new DateTime(rdt.year, rdt.month, rdt.day)
                    .Add(elapsed);
            }
        }
        
        /// <summary>
        /// Processes xAPI statements by applying reference-date-based timestamp anonymization.
        /// Transforms statement timestamps using the configured reference date and time preservation
        /// strategy, providing robust temporal privacy protection while maintaining analytical utility
        /// for session analysis, duration calculations, and learning pattern recognition.
        /// </summary>
        /// <param name="statement">The xApiStatement to process for timestamp anonymization</param>
        /// <returns>xApiStatement with anonymized timestamp, or unchanged if no timestamp exists</returns>
        protected override xApiStatement AfterCompose(xApiStatement statement)
        {
            // Skip processing if statement has no timestamp to avoid null reference errors
            if (!statement.GetTimestamp().HasValue)
                return statement;
                
            // Apply reference-date anonymization to the statement timestamp
            statement.WithTimestamp(AnonymizeTimestamp(statement.GetTimestamp()!.Value));
            return statement;
        }
    }
}
#endif