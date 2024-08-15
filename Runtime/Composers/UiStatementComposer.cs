using OmiLAXR.Extensions;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / UI Statement Composer (xAPI)")]
    public class UiStatementComposer : xApiStatementComposer<UiTrackingBehaviour>
    {
        protected override void Compose(UiTrackingBehaviour tb)
        {
            trackingBehaviour.OnClickedButton += (_, button) =>
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
            };
            trackingBehaviour.OnChangedSlider += (_, slider, newValue) =>
            {
                var maxValue = slider.maxValue;
                var minValue = slider.minValue;
                var sliderName = slider.name;
                
                var statement = actor.Does(xapi.generic.verbs.clicked)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .uiElementValue(newValue)
                        .uiElementMinValue(minValue)
                        .uiElementMaxValue(maxValue)
                        .uiElementType("slider")
                        .vrObjectName(sliderName))
                    .Activity(xapi.virtualReality.activities.uiElement);
                
                SendStatement(statement);
            };
        }
    }
}