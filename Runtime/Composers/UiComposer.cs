using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.Extensions;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / UI Statement Composer (xAPI)")]
    [Description("Creates statements:\n- actor clicked uiElement with uiElementType('button'), vrObjectName(name), uiElementValue(text)" +
                 "\n- actor changed uiElement with uiElementType('slider'), vrObjectName(name), uiElementValue(value), uiElementMinValue(minValue), uiElementMaxValue(maxValue)" +
                 "\n- actor changed uiElement with uiElementType('dropdown'), vrObjectName(name), uiElementValue(value), uiElementMinValue(0), uiElementMaxValue(options.Length), uiElementOptions(options)" +
                 "\n- actor changed uiElement with uiElementType('toggle'), vrObjectName(name), uiElementValue(checked), uiElementMinValue(false), uiElementMaxValue(true)" +
                 "\n- actor changed uiElement with uiElementType('inputField'), vrObjectName(name), uiElementValue(value)" +
                 "\n- actor changed uiElement with uiElementType('scrollbar'), vrObjectName(name), uiElementValue(value)")]
    public sealed class UiComposer : xApiComposer<UiTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(UiTrackingBehaviour tb)
        {
            tb.OnClickedButton.AddHandler((_, button) =>
            {
                var buttonName = button.gameObject.GetTrackingName();
                var text = button.GetTextOrDefault();
                
                var stmt = actor.Does(xapi.generic.verbs.clicked)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("button")
                        .vrObjectName(buttonName)
                        .uiElementValue(text))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(stmt);
            });
            tb.OnChangedSlider.AddHandler((_, slider, newValue) =>
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
                SendStatement(stmt);
            });
            tb.OnChangedDropdown.AddHandler((_, dropdown, newValue, options) =>
            {
                var value = options[newValue];
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("dropdown")
                        .vrObjectName(dropdown.GetTrackingName())
                        .uiElementValue(value)
                        .uiElementMinValue(0)
                        .uiElementMaxValue(options.Length)
                        .uiElementOptions(options))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(stmt);
            });
            tb.OnChangedToggle.AddHandler((_, toggle, isChecked) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("toggle")
                        .vrObjectName(toggle.GetTrackingName())
                        .uiElementValue(isChecked)
                        .uiElementMinValue(false)
                        .uiElementMaxValue(true))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(stmt);
            });
            tb.OnChangedInputField.AddHandler((_, inputFieldSelectable, value) =>
            {
                // inputFieldSelectable may be InputField or TMP_InputField
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("inputField")
                        .vrObjectName(inputFieldSelectable.GetTrackingName())
                        .uiElementValue(value))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(stmt);
            });
            tb.OnChangedScrollbar.AddHandler((_, scrollbar, value) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementType("scrollbar")
                        .vrObjectName(scrollbar.GetTrackingName())
                        .uiElementValue(value)
                    )
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(stmt);
            });
        }
    }
}