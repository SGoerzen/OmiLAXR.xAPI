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

namespace OmiLAXR.xAPI.Composers.Input
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from Unity Input System interactions.
    /// Generates input-based statements when button press and release events are detected,
    /// supporting various input devices and providing device-specific context information.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / [xAPI] Input System Composer"),
     Description("Creates statements:\n- actor pressed action with actionName(String)")]
    public sealed class InputSystemComposer : xApiComposer<InputSystemTrackingBehaviour>
    {
        /// <summary>
        /// Key used for storing and retrieving button press statements for press-release correlation.
        /// Enables linking press and release events in the analytics timeline.
        /// </summary>
        private const int ButtonKey = 0;
        
        /// <summary>
        /// Categorizes this composer under input tracking for organizational purposes.
        /// Groups Unity Input System analytics with other input device composers.
        /// </summary>
        /// <returns>ComposerGroup.Input indicating this handles input device interactions</returns>
        public override ComposerGroup GetGroup() => ComposerGroup.Input;

        /// <summary>
        /// Identifies the author of this composer implementation for attribution and support.
        /// </summary>
        /// <returns>Author information including name and contact details</returns>
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        /// <summary>
        /// Configures xAPI statement composition for Unity Input System events.
        /// Binds event handlers for button press and release actions, including
        /// device identification and statement correlation for complete interaction tracking.
        /// </summary>
        /// <param name="tb">InputSystemTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(InputSystemTrackingBehaviour tb)
        {
            // Handle button press events with device context and statement storage
            tb.OnPressedAnyButton.AddHandler((owner, args) =>
            {
                var stmt = actor.Does(xapi.virtualReality.verbs.pressed)
                    .Activity(xapi.virtualReality.activities.action)
                    .WithContext(xapi.generic.extensions.context.deviceId(args.DeviceId).deviceName(args.DeviceName))
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .actionName(args.ButtonName));
                
                // Store statement for correlation with future release event
                StoreStatement(ButtonKey, stmt);
                SendStatement(owner, stmt);
            });
            
            // Handle button release events with reference to corresponding press event
            tb.OnReleasedAnyButton.AddHandler((owner, args) =>
            {
                // Retrieve the stored press statement for correlation
                var refStmt = RestoreStatement(ButtonKey);
                
                var stmt = actor.Does(xapi.virtualReality.verbs.released)
                    .Activity(xapi.virtualReality.activities.action)
                    .WithContext(xapi.generic.extensions.context.deviceId(args.DeviceId).deviceName(args.DeviceName))
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .actionName(args.ButtonName))
                    .WithRef(refStmt); // Reference the original press statement
                
                SendStatement(owner, stmt);
            });
        }
    }
}
#endif