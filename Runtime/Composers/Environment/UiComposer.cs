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
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from UI element interactions.
    /// Generates comprehensive environment-based statements for various UI controls including
    /// buttons, sliders, dropdowns, toggles, input fields, and scrollbars with element-specific context.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / UI Composer (xAPI)")]
    [Description("Creates statements:\n- actor clicked uiElement with uiElementType('button'), vrObjectName(String), uiElementValue(text)" +
                 "\n- actor changed uiElement with uiElementType('slider'), vrObjectName(String), uiElementValue(String), uiElementMinValue(Float), uiElementMaxValue(Float)" +
                 "\n- actor changed uiElement with uiElementType('dropdown'), vrObjectName(String), uiElementValue(String), uiElementMinValue(0), uiElementMaxValue(Int), uiElementOptions(String[])" +
                 "\n- actor changed uiElement with uiElementType('toggle'), vrObjectName(String), uiElementValue(Boolean), uiElementMinValue(false), uiElementMaxValue(true)" +
                 "\n- actor changed uiElement with uiElementType('inputField'), vrObjectName(String), uiElementValue(String)" +
                 "\n- actor changed uiElement with uiElementType('scrollbar'), vrObjectName(String), uiElementValue(String)")]
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
        /// Configures xAPI statement composition for various UI element interaction events.
        /// Binds event handlers for different UI controls, each with appropriate
        /// element-specific context and value information for comprehensive UI analytics.
        /// </summary>
        /// <param name="tb">UiTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(UiTrackingBehaviour tb)
        {
            // Handle button click events with text content
            tb.OnClickedButton.AddHandler((owner, button) =>
            {
                var buttonName = button.gameObject.GetTrackingName();
                var text = button.GetTextOrDefault(); // Get button text or default value
                
                var stmt = actor.Does(xapi.generic.verbs.clicked)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("button")
                        .vrObjectName(buttonName)
                        .uiElementValue(text))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(owner, stmt);
            });
            
            // Handle slider value changes with min/max range context
            tb.OnChangedSlider.AddHandler((owner, slider, newValue) =>
            {
                var maxValue = slider.maxValue;
                var minValue = slider.minValue;
                var sliderName = slider.GetTrackingName();
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("slider")
                        .vrObjectName(sliderName)
                        .uiElementValue(newValue)
                        .uiElementMinValue(minValue)
                        .uiElementMaxValue(maxValue))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(owner, stmt);
            });
            
            // Handle dropdown selection changes with available options context
            tb.OnChangedDropdown.AddHandler((owner, dropdown, newValue, options) =>
            {
                var value = options[newValue]; // Get selected option text
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("dropdown")
                        .vrObjectName(dropdown.GetTrackingName())
                        .uiElementValue(value)
                        .uiElementMinValue(0)
                        .uiElementMaxValue(options.Length)
                        .uiElementOptions(options)) // Include all available options
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(owner, stmt);
            });
            
            // Handle toggle state changes with boolean value context
            tb.OnChangedToggle.AddHandler((owner, toggle, isChecked) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("toggle")
                        .vrObjectName(toggle.GetTrackingName())
                        .uiElementValue(isChecked)
                        .uiElementMinValue(false)  // Boolean min value
                        .uiElementMaxValue(true))  // Boolean max value
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(owner, stmt);
            });
            
            // Handle input field text changes (supports both InputField and TMP_InputField)
            tb.OnChangedInputField.AddHandler((owner, inputFieldSelectable, value) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("inputField")
                        .vrObjectName(inputFieldSelectable.GetTrackingName())
                        .uiElementValue(value))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(owner, stmt);
            });
            
            // Handle scrollbar value changes with current value context
            tb.OnChangedScrollbar.AddHandler((owner, scrollbar, value) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("scrollbar")
                        .vrObjectName(scrollbar.GetTrackingName())
                        .uiElementValue(value)
                    )
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(owner, stmt);
            });
        }
    }
}
#endif