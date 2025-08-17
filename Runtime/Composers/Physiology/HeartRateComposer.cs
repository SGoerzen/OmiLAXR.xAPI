/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from heart rate tracking data.
    /// Generates physiological measurement statements when heart rate events are detected.
    /// Currently marked as not implemented - serves as a template for future heart rate analytics integration.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / Heart Rate Composer (xAPI)")]
    [Description("Creates statements:\n- actor measured heart rate with heartRate(Int)")]
    public class HeartRateComposer : xApiComposer<HeartRateTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under physiological tracking for organizational purposes.
        /// Groups related composers together in the analytics pipeline for easier management.
        /// </summary>
        /// <returns>ComposerGroup.Physiology indicating this handles physiological data</returns>
        public override ComposerGroup GetGroup() => ComposerGroup.Physiology;

        /// <summary>
        /// Identifies the author of this composer implementation for attribution and support.
        /// </summary>
        /// <returns>Author information including name and contact details</returns>
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        /// <summary>
        /// Configures the xAPI statement composition logic for heart rate tracking events.
        /// Currently throws NotImplementedException - intended for future implementation
        /// when heart rate tracking capabilities are fully developed.
        /// </summary>
        /// <param name="tb">HeartRateTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(HeartRateTrackingBehaviour tb)
        {
            // Disable the component to prevent unnecessary processing
            enabled = false;
            throw new System.NotImplementedException();
        }
    }
}
#endif