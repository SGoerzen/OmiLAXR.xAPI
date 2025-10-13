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

namespace OmiLAXR.xAPI.Composers.System
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from Unity application lifecycle events.
    /// Generates system-level statements when the game/application starts, ends, is paused/resumed, or gains/loses focus,
    /// providing comprehensive application usage analytics with Unity-specific context information.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / [xAPI] System Composer")]
    [Description("Creates statements:\n- actor started game with name(String)" +
                 "\n- actor ended game with name(String)" +
                 "\n- actor paused/resumed game with name(String)" +
                 "\n- actor focused/unfocused game with name(String)")]
    public sealed class SystemComposer : xApiComposer<SystemTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under system tracking for organizational purposes.
        /// Groups application lifecycle events with other system-level analytics components.
        /// </summary>
        /// <returns>ComposerGroup.System indicating this handles system-level events</returns>
        public override ComposerGroup GetGroup() => ComposerGroup.System;

        /// <summary>
        /// Identifies the author of this composer implementation for attribution and support.
        /// </summary>
        /// <returns>Author information including name and contact details</returns>
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        
        /// <summary>
        /// Configures xAPI statement composition for Unity application lifecycle events.
        /// Binds event handlers for game start, quit, pause/resume, and focus changes,
        /// each including timestamp information and Unity application context.
        /// </summary>
        /// <param name="tb">SystemTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(SystemTrackingBehaviour tb)
        {
            // Handle game/application start events with timestamp and product name
            tb.OnStartedGame.AddHandler((owner, timestamp) =>
            {
                var stmt = actor.Does(xapi.systemControl.verbs.started)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(owner, stmt);
            });

            // Handle game/application quit events with timestamp and product name
            tb.OnQuitGame.AddHandler((owner, timestamp) =>
            {
                var stmt = actor.Does(xapi.systemControl.verbs.ended)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(owner, stmt);
            });
            
            // Handle game/application pause and resume state changes
            tb.OnPausedGame.AddHandler((owner, timestamp, isPaused) =>
            {
                // Select appropriate verb based on pause state
                var verb = isPaused ? xapi.systemControl.verbs.paused : xapi.systemControl.verbs.resumed;
                var stmt = actor.Does(verb)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(owner, stmt);
            });
            
            // Handle game/application focus state changes (window focus/unfocus)
            tb.OnFocusedGame.AddHandler((owner, timestamp, isFocused) =>
            {
                // Select appropriate verb based on focus state
                var verb = isFocused ? xapi.systemControl.verbs.focused : xapi.systemControl.verbs.unfocused;
                var stmt = actor.Does(verb)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(owner, stmt);
            });
        }
    }
}
#endif