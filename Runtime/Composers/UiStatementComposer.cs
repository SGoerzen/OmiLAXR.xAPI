using OmiLAXR.Extensions;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / UI Statement Composer (xAPI)")]
    public class UiStatementComposer : xApiStatementComposer<UiTrackingBehaviour>
    {
        protected override void Awake()
        {
            base.Awake();

            trackingBehaviour.OnClickedButton += button =>
            {
                var name = button.gameObject.GetTrackingName();
                var text = button.GetTextOrDefault();
                
                // Actor.Clicked(UIElementsEntryPoint.name().uiElementType()).Context().Result()
                
                DebugLog.xAPI.Print("Clicked " + name);
                
                /*LearningRecordStore.Instance.SendStatement(
                    verb: xAPI_Definitions.generic.verbs.clicked,
                    activity: xAPI_Definitions.virtualReality.activities.uiElement,
                    extensions: new xAPI_Extensions
                    {
                        xAPI_Definitions.systemControl.extensions.activity.name(text),
                        xAPI_Definitions.virtualReality.extensions.activity
                            .vrObjectName(name).uiElementValue(text).uiElementType("button"),
                    }
                );*/

            };
            trackingBehaviour.OnChangedSlider += (slider, value) => { };
        }
    }
}