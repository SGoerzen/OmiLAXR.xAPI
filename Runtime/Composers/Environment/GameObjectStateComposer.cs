/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.System;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers.Environment
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from GameObject state changes.
    /// Tracks environmental modifications like object creation, destruction, and state changes
    /// within the virtual learning environment for comprehensive activity analysis.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / [xAPI] Game Object State Composer")]
    [Description("Creates statements:\n- actor deleted vrObject with vrObjectName(String)")]
    public sealed class GameObjectStateComposer : xApiComposer<GameObjectStateTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under environment tracking for organizational purposes.
        /// Groups object state changes with other environmental modification analytics.
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
        /// Configures xAPI statement composition for GameObject state change events.
        /// Binds event handlers for destruction, enabling, and disabling events,
        /// though only destruction is currently fully implemented.
        /// </summary>
        /// <param name="tb">GameObjectStateTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(GameObjectStateTrackingBehaviour tb)
        {
            // Handle GameObject destruction events with xAPI statement generation
            tb.OnDestroyedGameObject.AddHandler((owner, _sender, go) =>
            {
                var statement = actor.Does(xapi.generic.verbs.deleted)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.name));
                SendStatement(owner, statement);
            });
            
            // GameObject enabled events - placeholder for future implementation
            tb.OnEnabledGameObject.AddHandler((owner, _sender, go) =>
            {
                // TODO: Implement xAPI statement generation for GameObject enabling
            });
            
            // GameObject disabled events - placeholder for future implementation
            tb.OnDisabledGameObject.AddHandler((owner, _sender, go) =>
            {
                // TODO: Implement xAPI statement generation for GameObject disabling
            });
        }
    }
}
#endif