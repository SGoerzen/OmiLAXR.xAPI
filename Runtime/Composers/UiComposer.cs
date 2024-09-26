using OmiLAXR.Composers;
using OmiLAXR.Extensions;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;
using UnityEngine.UI;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / UI Statement Composer (xAPI)")]
    public sealed class UiComposer : XApiComposer<UiTrackingBehaviour>
    {
        protected override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(UiTrackingBehaviour tb)
        {
            tb.OnClickedButton.AddHandler((_, button) =>
            {
                var buttonName = button.gameObject.GetTrackingName();
                var text = button.GetTextOrDefault();
                
                var statement = actor.Does(xapi.generic.verbs.clicked)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementValue(text)
                        .uiElementType("button")
                        .vrObjectName(buttonName))
                    .Activity(xapi.virtualReality.activities.uiElement);
                
                SendStatement(statement);
            });
            tb.OnChangedSlider.AddHandler((_, slider, newValue) =>
            {
                var maxValue = slider.maxValue;
                var minValue = slider.minValue;
                var sliderName = slider.GetTrackingName();
                
                var statement = actor.Does(xapi.generic.verbs.clicked)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementValue(newValue)
                        .uiElementMinValue(minValue)
                        .uiElementMaxValue(maxValue)
                        .uiElementType("slider")
                        .vrObjectName(sliderName))
                    .Activity(xapi.virtualReality.activities.uiElement);
                
                SendStatement(statement);
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
                
                SendStatement(stmt);
            });
        }
    }
}