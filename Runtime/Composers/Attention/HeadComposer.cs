/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers.Attention
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from head gesture tracking.
    /// Generates attention and engagement-related statements when head nods and shakes are detected,
    /// capturing non-verbal learner feedback and engagement indicators.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / [xAPI] Head Composer")]
    [Description("Creates statements:\n- actor nodded/shaked head with timeSpan(TimeSpan), numberOfGestures(Int)")]
    public sealed class HeadComposer : xApiComposer<HeadTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under attention tracking for organizational purposes.
        /// Groups head gestures with other attention and engagement measurement analytics.
        /// </summary>
        /// <returns>ComposerGroup.Attention indicating this handles attention-related data</returns>
        public override ComposerGroup GetGroup() => ComposerGroup.Attention;

        /// <summary>
        /// Identifies the author of this composer implementation for attribution and support.
        /// </summary>
        /// <returns>Author information including name and contact details</returns>
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        /// <summary>
        /// Configures xAPI statement composition for head gesture events.
        /// Binds event handlers for nodding and shaking gestures that include
        /// temporal data and gesture frequency for comprehensive gesture analysis.
        /// </summary>
        /// <param name="tb">HeadTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(HeadTrackingBehaviour tb)
        {
            // Handle head nodding gestures with timing and frequency data
            tb.OnNodded.AddHandler((owner, args) =>
            {
                var stmt = actor.Does(xapi.gestures.verbs.nodded)
                    .Activity(xapi.gestures.activities.head)
                    .WithResult(xapi.generic.extensions.result.timeSpan(args.TimeSpan))
                    .WithResult(xapi.gestures.extensions.result.numberOfGestures(args.NumberOfGesture));
                SendStatement(owner, stmt);
            });
            
            // Handle head shaking gestures with timing and frequency data
            tb.OnShook.AddHandler((owner, args) =>
            {
                var stmt = actor.Does(xapi.gestures.verbs.shaked)
                    .Activity(xapi.gestures.activities.head)
                    .WithResult(xapi.generic.extensions.result.timeSpan(args.TimeSpan))
                    .WithResult(xapi.gestures.extensions.result.numberOfGestures(args.NumberOfGesture));
                SendStatement(owner, stmt);
            });
        }
    }
}
#endif