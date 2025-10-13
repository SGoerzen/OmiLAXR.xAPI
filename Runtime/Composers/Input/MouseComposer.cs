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
    /// xAPI composer for creating learning analytics statements from mouse input interactions.
    /// Generates comprehensive input-based statements for mouse clicks, presses, scrolling, and movement,
    /// capturing detailed user interaction patterns with mouse position and button-specific context.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / [xAPI] Mouse Composer")]
    [Description("Creates statements:\n- actor clicked mouse with mouseButton(String), mousePosition(Vector3)" +
                 "\n- actor pressed mouse with mouseButton(String), mousePosition(Vector3)" +
                 "\n- actor scrolled mouse with mouseButton(String), mousePosition(Vector3), scrollValue(Float)" +
                 "\n- actor moved mouse with mouseButton(String), mousePosition(Vector3) and result startValue(Vector3), endValue(Vector3), value(Vector3)")]
    public sealed class MouseComposer : xApiComposer<MouseTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under input tracking for organizational purposes.
        /// Groups mouse interaction analytics with other input device composers.
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
        /// Configures xAPI statement composition for various mouse interaction events.
        /// Binds event handlers for clicks, presses, scrolling, and movement with
        /// comprehensive context including button information and positional data.
        /// </summary>
        /// <param name="tb">MouseTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(MouseTrackingBehaviour tb)
        {
            // Handle mouse click events with button and position context
            tb.OnClicked.AddHandler((owner, args) =>
            {
                var statement = actor.Does(xapi.generic.verbs.clicked)
                    .Activity(xapi.generic.activities.mouse)
                    .WithExtension(xapi.generic.extensions.activity
                        .mouseButton(args.MouseButton)
                        .mousePosition(args.MousePosition));
                SendStatement(owner, statement);
            });
            
            // Handle mouse button press events with button and position context
            tb.OnPressedDown.AddHandler((owner, args) =>
            {
                var statement = actor.Does(xapi.generic.verbs.pressed)
                    .Activity(xapi.generic.activities.mouse)
                    .WithExtension(xapi.generic.extensions.activity
                        .mouseButton(args.MouseButton)
                        .mousePosition(args.MousePosition));
                SendStatement(owner, statement);
            });
            
            // Handle mouse wheel scrolling events with scroll value and position context
            tb.OnScrolledWheel.AddHandler((owner, args, value) =>
            {
                var statement = actor.Does(xapi.generic.verbs.scrolled)
                    .Activity(xapi.generic.activities.mouse)
                    .WithExtension(xapi.generic.extensions.activity
                        .mouseButton(args.MouseButton)
                        .mousePosition(args.MousePosition)
                        .scrollValue(value));
                SendStatement(owner, statement);
            });
            
            // Handle mouse movement events with positional change analysis
            tb.OnMousePositionChanged.AddHandler((owner, pos, oldPos) =>
            {
                var change = pos - oldPos; // Calculate movement delta
                var statement = actor.Does(xapi.generic.verbs.moved)
                    .Activity(xapi.generic.activities.mouse)
                    .WithExtension(xapi.generic.extensions.activity
                        .mousePosition(pos))
                    .WithResult(xapi.generic.extensions.result
                        .startValue(oldPos)  // Starting position
                        .endValue(pos)       // Ending position
                        .value(change));     // Movement delta vector
                SendStatement(owner, statement);
            });
        }
    }
}
#endif