using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Mouse Statement Composer (xAPI)")]
    public class MouseStatementComposer : xApiStatementComposer<MouseTrackingBehaviour>
    {
        protected override void Awake()
        {
            base.Awake();
            var tb = trackingBehaviour;

            tb.OnClicked += (_, args) =>
            {
                actor.Does(xapi.generic.verbs.clicked)
                    .Activity(xapi.generic.activities.mouse, 
                        xapi.generic.extensions.activity
                            .mouseButton(args.mouseButton)
                            .mousePosition(args.mousePosition));
            };
            tb.OnPressedDown += (_, args) =>
            {
                actor.Does(xapi.generic.verbs.pressed)
                    .Activity(xapi.generic.activities.mouse, 
                        xapi.generic.extensions.activity
                            .mouseButton(args.mouseButton)
                            .mousePosition(args.mousePosition));
            };
            tb.OnScrolledWheel += (_, args, value) =>
            {
                actor.Does(xapi.generic.verbs.scrolled)
                    .Activity(xapi.generic.activities.mouse, 
                        xapi.generic.extensions.activity
                            .mouseButton(args.mouseButton)
                            .mousePosition(args.mousePosition)
                            .scrollValue(value));
            };
        }
    }
}