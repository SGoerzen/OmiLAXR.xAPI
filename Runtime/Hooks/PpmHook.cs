/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Actors.HeartRate;
using OmiLAXR.StressLevel;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI.Hooks
{
    /// <summary>
    /// xAPI hook for enriching statements with Peripheral Physiological Measures (PPM) data.
    /// Automatically adds biometric and physiological context to learning analytics statements
    /// by integrating heart rate, stress level, and other physiological indicators that provide
    /// insights into learner engagement, cognitive load, and emotional states during learning activities.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 5) Hooks / Peripheral Physiological Measures (PPM) Hook (xAPI)")]
    [Description("Adds peripheral physiological measures values to each statement.")]
    public class PpmHook : xApiHook
    {
        /// <summary>
        /// Processes xAPI statements by enriching them with available physiological measurement data.
        /// Queries available physiological data providers (heart rate, stress level) and adds
        /// corresponding measurements as result extensions, providing comprehensive biometric
        /// context for learning analytics analysis and learner state assessment.
        /// </summary>
        /// <param name="statement">The xApiStatement to enrich with physiological data</param>
        /// <returns>xApiStatement enriched with available peripheral physiological measures</returns>
        protected override xApiStatement AfterCompose(xApiStatement statement)
        {
            // Attempt to retrieve and add heart rate data if provider is available
            var heartRateProvider = GetProvider<HeartRateProvider>(statement);
            
            if (heartRateProvider)
            {
                // Add current heart rate as a result extension using xAPI Registry PPM vocabulary
                statement.WithResult(xapi.ppm.extensions.result.heartRate(heartRateProvider.GetHeartRate()));
            }
            
            // Attempt to retrieve stress level data if provider is available
            var stressLevelProvider = GetProvider<StressLevelProvider>(statement);
            if (stressLevelProvider)
            {
                // TODO: Implement stress level extension once xAPI Registry vocabulary is available
                // statement.WithResult(xapi.ppm.extensions.result.stressLevel(stressLevelProvider.GetStressLevel()));
            }

            return statement;
        }
    }
}
#endif