using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Mouse Statement Composer (xAPI)")]
    public class MouseStatementComposer : xApiStatementComposer<MouseTrackingBehaviour>
    {
        protected override void Compose(MouseTrackingBehaviour tb)
        {
            tb.OnClicked += (_, args) =>
            {
                var statement = actor.Does(xapi.generic.verbs.clicked)
                    .Activity(xapi.generic.activities.mouse, 
                        xapi.generic.extensions.activity
                            .mouseButton(args.mouseButton)
                            .mousePosition(args.mousePosition));
                SendStatement(statement);
            };
            tb.OnPressedDown += (_, args) =>
            {
                var statement = actor.Does(xapi.generic.verbs.pressed)
                    .Activity(xapi.generic.activities.mouse, 
                        xapi.generic.extensions.activity
                            .mouseButton(args.mouseButton)
                            .mousePosition(args.mousePosition));
                SendStatement(statement);
            };
            tb.OnScrolledWheel += (_, args, value) =>
            {
                var statement = actor.Does(xapi.generic.verbs.scrolled)
                    .Activity(xapi.generic.activities.mouse, 
                        xapi.generic.extensions.activity
                            .mouseButton(args.mouseButton)
                            .mousePosition(args.mousePosition)
                            .scrollValue(value));
                SendStatement(statement);
            };
            tb.OnMousePositionChanged += (_, pos) =>
            {
                var statement = actor.Does(xapi.generic.verbs.moved)
                    .Activity(xapi.generic.activities.mouse)
                    .WithExtension(xapi.generic.extensions.activity.mousePosition(pos));
                SendStatement(statement);
            };
        }
    }
}