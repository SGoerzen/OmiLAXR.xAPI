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

namespace OmiLAXR.xAPI.Composers
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from keyboard input interactions.
    /// Generates input-based statements when keyboard press and release events are detected,
    /// tracking learner interaction patterns with keyboard-based interfaces.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / Keyboard Composer (xAPI)")]
    [Description("Creates statements:\n- actor pressed/released keyboard with keyboardButton(String)")]
    public class KeyboardComposer : xApiComposer<KeyboardTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under input tracking for organizational purposes.
        /// Groups keyboard-related analytics with other input device composers.
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
        /// Configures xAPI statement composition for keyboard press and release events.
        /// Binds event handlers to the tracking behavior that create contextually appropriate
        /// xAPI statements with keyboard-specific extensions and activity definitions.
        /// </summary>
        /// <param name="tb">KeyboardTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(KeyboardTrackingBehaviour tb)
        {
            // Handle both key press and key release events
            tb.OnPressed.AddHandler((owner, args) =>
            {
                // Choose verb based on key state - pressed or released
                var verb = args.IsDown ? xapi.generic.verbs.pressed : xapi.generic.verbs.released;
                
                // Create xAPI statement with keyboard activity and key information
                var stmt = actor.Does(verb)
                    .Activity(xapi.generic.activities.keyboard)
                    .WithExtension(xapi.generic.extensions.activity.keyboardButton(args.Key));
                
                SendStatement(owner, stmt);
            });
        }
    }
}
#endif