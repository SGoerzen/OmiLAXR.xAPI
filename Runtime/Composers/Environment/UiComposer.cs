/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Components;
using OmiLAXR.Composers;
using OmiLAXR.Extensions;
using OmiLAXR.TrackingBehaviours.Learner;
using OmiLAXR.Types;
using UnityEngine;
using UnityEngine.UI;

namespace OmiLAXR.xAPI.Composers.Environment
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from UI element interactions.
    /// Generates comprehensive environment-based statements for various UI controls including
    /// buttons, sliders, dropdowns, toggles, input fields, and scrollbars with element-specific context.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / [xAPI] UI Composer")]
    [Description(
        "Creates statements:" +
        "\n- actor hovered uiElement (verb: hovered) with activity: uiElement; values: uiElementType, vrObjectName, uiElementContent; result: hoverDuration, pressDuration, pressesInHover, totalPresses" +
        "\n- actor exited uiElement (verb: exited) with activity: uiElement; values: uiElementType, vrObjectName, uiElementContent; result: hoverDuration, pressDuration, pressesInHover, totalPresses; statement may reference prior hovered" +
        "\n- actor clicked uiElement (verb: clicked) with activity: uiElement; values: uiElementType, vrObjectName, uiElementContent; result: hover/press metrics; statement may reference prior hovered" +
        "\n- actor clicked button (verb: clicked) with activity: uiElement; extension: uiElementType('button'), vrObjectName, uiElementValue(text)" +
        "\n- actor changed slider/dropdown/toggle/inputField/scrollbar (verb: changed) with activity: uiElement; extension includes uiElementType, vrObjectName, uiElementValue and element-specific fields (min/max/options as applicable); statement may reference prior hovered"
    )]
    public sealed class UiComposer : xApiComposer<UiTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under environment tracking for organizational purposes.
        /// Groups UI interaction analytics with other environmental interaction composers.
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
        /// Adds pointer / device context and pointer-event derived metrics to the statement.
        /// Populates context.deviceName and result fields: hoverDuration, pressDuration, pressesInHover, totalPresses.
        /// Called for hover/click events where InteractionEventHandler provides timing/count metrics.
        /// </summary>
        /// <param name="stmt">Statement to augment</param>
        /// <param name="args">Pointer event args containing device and timing/count metrics</param>
        private void MapPointerEventsData(xApiStatement stmt, InteractionEventHandler.InteractionEventArgs args)
        {
            stmt
                .WithContext(xapi.generic.extensions.context.deviceName(args.Device))
                .WithValue(xapi.generic.extensions.result
                    .hoverDuration(Duration.FromSeconds(args.HoverDuration))
                    .pressDuration(Duration.FromSeconds(args.PressDuration))
                    .pressesInHover(args.PressesInHover)
                    .totalPresses(args.TotalPresses)
                );
        }

        /// <summary>
        /// Attempts to find and attach a previously stored 'hovered' statement as a reference.
        /// If a reference is found, sets the duration between the new statement's creation time and
        /// the referenced statement's creation time, and attaches the reference. Optionally erases the stored ref.
        /// </summary>
        /// <param name="selectable">UI selectable used as key for stored statements</param>
        /// <param name="stmt">Statement to update with reference and duration (if found)</param>
        /// <param name="erase">If true, remove the stored reference after restoring it</param>
        private void DetectRefStmt(Selectable selectable, xApiStatement stmt, bool erase = false)
        {
            var refStmt = RestoreStatement(selectable.GetHashCode(), erase);
            if (refStmt != null)
            {
                var duration = stmt.CreatedAt - refStmt.CreatedAt;
                stmt.WithDuration(duration)
                    .WithRef(refStmt);
            }
        }
        
        /// <summary>
        /// Configures xAPI statement composition for various UI element interaction events.
        /// Binds event handlers for different UI controls, each with appropriate
        /// element-specific context and value information for comprehensive UI analytics.
        /// </summary>
        /// <param name="tb">UiTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(UiTrackingBehaviour tb)
        {
            tb.OnHoverStarted.AddHandler((owner, selectable, args) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.hovered)
                    .Activity(xapi.virtualReality.activities.uiElement)
                    .WithValue(xapi.virtualReality.extensions.activity
                        .uiElementType(selectable.GetUiElementType())
                        .vrObjectName(selectable.GetTrackingName())
                        .uiElementContent(selectable.GetTextOrDefault())
                    );
                MapPointerEventsData(stmt, args);
                StoreStatement(selectable.GetHashCode(), stmt);
                SendStatement(owner, stmt);
            });
            tb.OnHoverEnded.AddHandler((owner, selectable, args) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.exited)
                    .Activity(xapi.virtualReality.activities.uiElement)
                    .WithValue(xapi.virtualReality.extensions.activity
                        .uiElementType(selectable.GetUiElementType())
                        .vrObjectName(selectable.GetTrackingName())
                        .uiElementContent(selectable.GetTextOrDefault())
                    );
                DetectRefStmt(selectable, stmt);
                MapPointerEventsData(stmt, args);
                
                SendStatement(owner, stmt);
            });
            tb.OnClicked.AddHandler((owner, selectable, args) =>
            {
                var refStmt = RestoreStatement(selectable.GetHashCode());
                var stmt = actor.Does(xapi.generic.verbs.clicked)
                    .Activity(xapi.virtualReality.activities.uiElement)
                    .WithRef(refStmt)
                    .WithValue(xapi.virtualReality.extensions.activity
                        .uiElementType(selectable.GetUiElementType())
                        .vrObjectName(selectable.GetTrackingName())
                        .uiElementContent(selectable.GetTextOrDefault())
                    );
                DetectRefStmt(selectable, stmt);
                MapPointerEventsData(stmt, args);
                SendStatement(owner, stmt);
            });
            // Handle button click events with text content
            tb.OnClickedButton.AddHandler((owner, button) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.clicked)
                    .Activity(xapi.virtualReality.activities.uiElement)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType(button.GetUiElementType())
                        .vrObjectName(button.GetTrackingName())
                        .uiElementValue(button.GetTextOrDefault()));
                DetectRefStmt(button, stmt);
                SendStatement(owner, stmt);
            });
            
            // Handle slider value changes with min/max range context
            tb.OnChangedSlider.AddHandler((owner, slider, newValue) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .Activity(xapi.virtualReality.activities.uiElement)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType(slider.GetUiElementType())
                        .vrObjectName(slider.GetTrackingName())
                        .uiElementValue(newValue)
                        .uiElementMinValue(slider.minValue)
                        .uiElementMaxValue(slider.maxValue));
                DetectRefStmt(slider, stmt);
                SendStatement(owner, stmt);
            });
            
            // Handle dropdown selection changes with available options context
            tb.OnChangedDropdown.AddHandler((owner, dropdown, newValue, options) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .Activity(xapi.virtualReality.activities.uiElement)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType(dropdown.GetUiElementType())
                        .vrObjectName(dropdown.GetTrackingName())
                        .uiElementValue(options[newValue])
                        .uiElementMinValue(0)
                        .uiElementMaxValue(options.Length)
                        .uiElementOptions(options));
                DetectRefStmt(dropdown, stmt);
                SendStatement(owner, stmt);
            });
            
            // Handle toggle state changes with boolean value context
            tb.OnChangedToggle.AddHandler((owner, toggle, isChecked) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                        .Activity(xapi.virtualReality.activities.uiElement)
                        .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType(toggle.GetUiElementType())
                        .vrObjectName(toggle.GetTrackingName())
                        .uiElementValue(isChecked)
                        .uiElementMinValue(false)  // Boolean min value
                        .uiElementMaxValue(true))  // Boolean max value
                    ;
                DetectRefStmt(toggle, stmt);
                SendStatement(owner, stmt);
            });
            
            // Handle input field text changes (supports both InputField and TMP_InputField)
            tb.OnChangedInputField.AddHandler((owner, inputFieldSelectable, value) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                        .Activity(xapi.virtualReality.activities.uiElement)
                        .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType(inputFieldSelectable.GetUiElementType())
                        .vrObjectName(inputFieldSelectable.GetTrackingName())
                        .uiElementValue(value))
                    ;
                DetectRefStmt(inputFieldSelectable, stmt);
                SendStatement(owner, stmt);
            });
            
            // Handle scrollbar value changes with current value context
            tb.OnChangedScrollbar.AddHandler((owner, scrollbar, value) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                        .Activity(xapi.virtualReality.activities.uiElement)
                        .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType(scrollbar.GetUiElementType())
                        .vrObjectName(scrollbar.GetTrackingName())
                        .uiElementValue(value)
                    );
                DetectRefStmt(scrollbar, stmt);
                SendStatement(owner, stmt);
            });
        }
    }
}
#endif