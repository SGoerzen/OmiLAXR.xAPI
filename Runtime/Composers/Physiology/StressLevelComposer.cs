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

namespace OmiLAXR.xAPI.Composers.Physiology
{
    /// <summary>
    /// Composer responsible for handling and logging stress level tracking events.
    /// Subscribes to various stress-related events and logs their occurrences.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / [xAPI] Stress Level Composer")]
    [Description("Creates statements:\n" +
                 "- actor stressed stress\n" +
                 "- actor relaxed stress\n" +
                 "- actor increased stress\n" +
                 "- actor decreased stress\n" +
                 "- actor updated stress")]
    public sealed class StressLevelComposer : xApiComposer<StressLevelTrackingBehaviour>
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
            // State transitions
            tb.OnStressDetected.AddHandler((owner, value) =>
            {
                var stmt = actor.Does(xapi.ppm.verbs.stressed)
                    .Activity(xapi.ppm.activities.stress)
                    .WithResult(xapi.ppm.extensions.result.stressScore(value));
                SendStatement(owner, stmt);
            });

            tb.OnRelaxationDetected.AddHandler((owner, value) =>
            {
                var stmt = actor.Does(xapi.ppm.verbs.relaxed)
                    .Activity(xapi.ppm.activities.stress)
                    .WithResult(xapi.ppm.extensions.result.stressScore(value));
                SendStatement(owner, stmt);
            });

            tb.OnStressLevelIncreased.AddHandler((owner, value, delta) =>
            {
                var stmt = actor.Does(xapi.ppm.verbs.increased)
                    .Activity(xapi.ppm.activities.stress)
                    .WithResult(xapi.ppm.extensions.result.stressScore(value).delta(delta));
                SendStatement(owner, stmt);
            });

            tb.OnStressLevelDecreased.AddHandler((owner, value, delta) =>
            {
                var stmt = actor.Does(xapi.ppm.verbs.decreased)
                    .Activity(xapi.ppm.activities.stress)
                    .WithResult(xapi.ppm.extensions.result.stressScore(value).delta(delta));
                SendStatement(owner, stmt);
            });

            tb.OnStressLevelUpdated.AddHandler((owner, value, delta) =>
            {
                var stmt = actor.Does(xapi.ppm.verbs.updated)
                    .Activity(xapi.ppm.activities.stress)
                    .WithResult(xapi.ppm.extensions.result.stressScore(value).delta(delta));
                SendStatement(owner, stmt);
            });

        }
    }
}
#endif