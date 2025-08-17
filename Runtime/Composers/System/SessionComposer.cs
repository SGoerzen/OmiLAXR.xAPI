/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.Context;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from session lifecycle events.
    /// Generates system-level statements when learning sessions start and end, providing
    /// temporal boundaries for analytics analysis and session identification via UUID.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / Session Composer (xAPI)"),
    Description("Creates statements: \n- actor started session with timestamp(DateTime), name(UUID) " +
                "\n- actor ended session with timestamp(DateTime), name(UUID)")]
    public class SessionComposer : xApiComposer<SessionTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under system tracking for organizational purposes.
        /// Groups session lifecycle events with other system-level analytics components.
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
        /// Configures xAPI statement composition for session lifecycle events.
        /// Binds event handlers for session start and stop events, including UUID-based
        /// session identification when Registration component is available.
        /// </summary>
        /// <param name="tb">SessionTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(SessionTrackingBehaviour tb)
        {
            // Handle session start events with UUID tracking if available
            tb.OnSessionStarted.AddHandler((owner, startDate) =>
            {
                if (Registration.Instance && Registration.Instance.enabled)
                {
                    // Create session start statement with UUID identification
                    DebugLog.xAPI.Print($"Started session with UUID: {Registration.Instance.uuid}.");
                    var stmt = actor.Does(xapi.systemControl.verbs.started)
                        .Activity(xapi.systemControl.activities.session)
                        .WithTimestamp(startDate)
                        .WithExtension(xapi.systemControl.extensions.activity.name(Registration.Instance.uuid));  
                    SendStatement(owner, stmt);
                }
                else
                {
                    // Handle session start without UUID when Registration is unavailable
                    DebugLog.xAPI.Print("Started session.");
                }
            });
            
            // Handle session stop events with UUID tracking if available
            tb.OnSessionStopped.AddHandler((owner, stopDate) =>
            {
                if (Registration.Instance && Registration.Instance.enabled)
                {
                    // Create session end statement with UUID identification
                    DebugLog.xAPI.Print($"Stopped session with UUID: {Registration.Instance.uuid}.");
                    var stmt = actor.Does(xapi.systemControl.verbs.ended)
                        .Activity(xapi.systemControl.activities.session)
                        .WithTimestamp(stopDate)
                        .WithExtension(xapi.systemControl.extensions.activity.name(Registration.Instance.uuid));
                    SendStatement(owner, stmt);
                }
                else
                {
                    // Handle session stop without UUID when Registration is unavailable
                    DebugLog.xAPI.Print("Stopped session.");
                }
            });
        }
    }
}
#endif