/*
 * SPDX-License-Identifier: AGPL-3.0-or-later
 * Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
 * This file is part of OmiLAXR.xAPI.
 */
#if XAPI_REGISTRY_EXISTS
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;

namespace OmiLAXR.xAPI.Composers
{
    /// <summary>
    /// Composer responsible for handling and logging stress level tracking events.
    /// Subscribes to various stress-related events and logs their occurrences.
    /// </summary>
    public class StressLevelComposer : xApiComposer<StressLevelTrackingBehaviour>
    {
        /// <summary>
        /// Specifies the composer group as Physiology.
        /// </summary>
        /// <returns>ComposerGroup.Physiology</returns>
        public override ComposerGroup GetGroup() => ComposerGroup.Physiology;

        /// <summary>
        /// Provides author information for the stress level tracking.
        /// </summary>
        /// <returns>Author details</returns>
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        /// <summary>
        /// Composes the stress level tracking behavior by attaching event handlers.
        /// Logs various stress-related events for debugging and tracking purposes.
        /// </summary>
        /// <param name="tb">The stress level tracking behavior to compose</param>
        protected override void Compose(StressLevelTrackingBehaviour tb)
        {
            // Log when relaxation is detected
            tb.OnRelaxationDetected.AddHandler((owner, value) =>
            {
                OmiLAXR.DebugLog.OmiLAXR.Print($"OnRelaxationDetected: {value}");
            });

            // Log when stress is detected
            tb.OnStressDetected.AddHandler((owner, value) =>
            {
                OmiLAXR.DebugLog.OmiLAXR.Print($"OnStressDetected: {value}");
            });

            // Log when stress level decreases
            tb.OnStressLevelDecreased.AddHandler((owner, value) =>
            {
                OmiLAXR.DebugLog.OmiLAXR.Print($"OnStressLevelDecreased: {value}");
            });

            // Log when stress level increases
            tb.OnStressLevelIncreased.AddHandler((owner, value) =>
            {
                OmiLAXR.DebugLog.OmiLAXR.Print($"OnStressLevelIncreased: {value}");
            });

            // Log general stress level updates
            tb.OnStressLevelUpdated.AddHandler((owner, value) =>
            {
                OmiLAXR.DebugLog.OmiLAXR.Print($"OnStressLevelUpdated: {value}");
            });
        }
    }
}
#endif