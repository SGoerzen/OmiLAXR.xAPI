/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Components;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers.Environment
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from object transform changes.
    /// Generates environment-based statements when objects are moved, rotated, or scaled,
    /// capturing spatial manipulation patterns with comprehensive positional and change data.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / [xAPI] Transform Composer")]
    [Description("Creates statements:\n- actor moved/rotated/scaled vrObject with vrObjectName(String) and result position(Vector3), rotation(Vector3), scale(Vector3), startValue(Vector3), endValue(Vector3)")]
    public sealed class TransformComposer : xApiComposer<TransformTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under environment tracking for organizational purposes.
        /// Groups object transformation analytics with other environmental modification composers.
        /// </summary>
        /// <returns>ComposerGroup.Environment indicating this handles environmental changes</returns>
        public override ComposerGroup GetGroup() => ComposerGroup.Environment;

        /// <summary>
        /// Identifies the author of this composer implementation for attribution and support.
        /// </summary>
        /// <returns>Author information including name and contact details</returns>
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        /// <summary>
        /// Configures xAPI statement composition for transform change events.
        /// Binds event handlers for position, rotation, and scale changes with
        /// comprehensive spatial data including current state and change deltas.
        /// </summary>
        /// <param name="tb">TransformTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(TransformTrackingBehaviour tb)
        {
            // Handle object position changes with comprehensive transform data
            tb.OnChangedPosition.AddHandler((owner, tw, change) =>
            {
                var t = tw.transform;
                var stmt = actor.Does(xapi.virtualReality.verbs.moved)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .vrObjectName(tw.GetTrackingName()))
                    .WithResult(xapi.virtualReality.extensions.result
                        .position(t.position)      // Current position
                        .rotation(t.eulerAngles)   // Current rotation
                        .scale(t.localScale))      // Current scale
                    .WithResult(xapi.generic.extensions.result
                        .startValue(change.OldValue)  // Previous position
                        .endValue(change.NewValue));  // New position
                SendStatement(owner, stmt);
            });
            
            // Handle object rotation changes with comprehensive transform data
            tb.OnChangedRotation.AddHandler((owner, tw, change) =>
            {
                var t = tw.transform;
                var stmt = actor.Does(xapi.virtualReality.verbs.rotated)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .vrObjectName(tw.GetTrackingName()))
                    .WithResult(xapi.virtualReality.extensions.result
                        .position(t.position)      // Current position
                        .rotation(t.eulerAngles)   // Current rotation
                        .scale(t.localScale))      // Current scale
                    .WithResult(xapi.generic.extensions.result
                        .startValue(change.OldValue)  // Previous rotation
                        .endValue(change.NewValue));  // New rotation
                SendStatement(owner, stmt);
            });
            
            // Handle object scale changes with comprehensive transform data
            tb.OnChangedScale.AddHandler((owner, tw, change) =>
            {
                var t = tw.transform;
                var stmt = actor.Does(xapi.virtualReality.verbs.scaled)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .vrObjectName(tw.GetTrackingName()))
                    .WithResult(xapi.virtualReality.extensions.result
                        .position(t.position)      // Current position
                        .rotation(t.eulerAngles)   // Current rotation
                        .scale(t.localScale))      // Current scale
                    .WithResult(xapi.generic.extensions.result
                        .startValue(change.OldValue)  // Previous scale
                        .endValue(change.NewValue));  // New scale
                SendStatement(owner, stmt);
            });
        }
    }
}
#endif