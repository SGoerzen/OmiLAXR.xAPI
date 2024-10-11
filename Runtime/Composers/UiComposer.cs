using OmiLAXR.Composers;
using OmiLAXR.Extensions;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / UI Statement Composer (xAPI)")]
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
                        .uiElementValue(text)
                        .uiElementType("button")
                        .vrObjectName(buttonName))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(stmt);
            });
            tb.OnChangedSlider.AddHandler((_, slider, newValue) =>
            {
                var maxValue = slider.maxValue;
                var minValue = slider.minValue;
                var sliderName = slider.GetTrackingName();
                
                var stmt = actor.Does(xapi.generic.verbs.clicked)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementValue(newValue)
                        .uiElementMinValue(minValue)
                        .uiElementMaxValue(maxValue)
                        .uiElementType("slider")
                        .vrObjectName(sliderName))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(stmt);
            });
            tb.OnChangedDropdown.AddHandler((_, dropdown, newValue) =>
            {
                var value = dropdown.options[newValue].text;
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementValue(value)
                        .uiElementMinValue(0)
                        .uiElementMaxValue(dropdown.options.Count)
                        // todo: add .uiElementOptions([...])
                        .uiElementType("dropdown")
                        .vrObjectName(dropdown.GetTrackingName()))
                    .Activity(xapi.virtualReality.activities.uiElement);
                print(stmt);
                SendStatement(stmt);
            });
            tb.OnChangedToggle.AddHandler((_, toggle, isChecked) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementValue(isChecked)
                        .uiElementMinValue(false)
                        .uiElementMaxValue(true)
                        .uiElementType("toggle")
                        .vrObjectName(toggle.GetTrackingName()))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(stmt);
            });
            tb.OnChangedInputField.AddHandler((_, inputFieldSelectable, value) =>
            {
                // inputFieldSelectable may be InputField or TMP_InputField
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementValue(value)
                        .uiElementType("inputField")
                        .vrObjectName(inputFieldSelectable.GetTrackingName()))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(stmt);
            });
            tb.OnChangedScrollbar.AddHandler((_, scrollbar, value) =>
            {
                var stmt = actor.Does(xapi.generic.verbs.changed)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementValue(value)
                        .uiElementType("scrollbar")
                        .vrObjectName(scrollbar))
                    .Activity(xapi.virtualReality.activities.uiElement);
                SendStatement(stmt);
            });
        }
    }
}