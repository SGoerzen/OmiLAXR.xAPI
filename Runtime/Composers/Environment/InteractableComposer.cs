/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Components;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from object interaction events.
    /// Generates environment-based statements when learners interact with, grab, touch, or release
    /// virtual objects, providing detailed interaction pattern analysis with hand-specific context.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / Interactable Composer (xAPI)"),
     Description("Creates statements:\n- actor interacted/grabbed/touched/released vrObject with vrObjectName(String), hand(Hand)")]
    public class InteractableComposer : xApiComposer<InteractableTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under environment tracking for organizational purposes.
        /// Groups object interaction analytics with other environmental modification composers.
        /// </summary>
        /// <returns>ComposerGroup.Environment indicating this handles environmental interactions</returns>
        public override ComposerGroup GetGroup() => ComposerGroup.Environment;

        /// <summary>
        /// Identifies the author of this composer implementation for attribution and support.
        /// </summary>
        /// <returns>Author information including name and contact details</returns>
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        /// <summary>
        /// Configures xAPI statement composition for various object interaction events.
        /// Binds event handlers for interaction, grabbing, touching, and releasing actions,
        /// each including object identification and hand-specific context information.
        /// </summary>
        /// <param name="tb">InteractableTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(InteractableTrackingBehaviour tb)
        {
            // Handle general object interaction events
            tb.OnInteracted.AddHandler((owner, e) =>
            {
                var go = e.Target;
                var stmt = actor.Does(xapi.virtualReality.verbs.interacted)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.GetTrackingName()))
                    .WithExtension(xapi.gestures.extensions.activity.hand(e.Hand));
                SendStatement(owner, stmt);
            });
            
            // Handle object grabbing events with hand context
            tb.OnGrabbed.AddHandler((owner, e) =>
            {
                var go = e.Target;
                var stmt = actor.Does(xapi.virtualReality.verbs.grabbed)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.GetTrackingName()))
                    .WithExtension(xapi.gestures.extensions.activity.hand(e.Hand));
                SendStatement(owner, stmt);
            });
            
            // Handle object touching events with hand context
            tb.OnTouched.AddHandler((owner, e) =>
            {
                var go = e.Target;
                var stmt = actor.Does(xapi.virtualReality.verbs.touched)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.GetTrackingName()))
                    .WithExtension(xapi.gestures.extensions.activity.hand(e.Hand));
                SendStatement(owner, stmt);
            });
            
            // Handle object release events with hand context
            tb.OnReleased.AddHandler((owner, e) =>
            {
                var go = e.Target;
                var stmt = actor.Does(xapi.virtualReality.verbs.released)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.GetTrackingName()))
                    .WithExtension(xapi.gestures.extensions.activity.hand(e.Hand));
                SendStatement(owner, stmt);
            });
        }
    }
}
#endif