using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Mouse Statement Composer (xAPI)")]
    public sealed class MouseComposer : XApiComposer<MouseTrackingBehaviour>
    {
        protected override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(MouseTrackingBehaviour tb)
        {
            tb.OnClicked.AddHandler((_, args) =>
            {
                var statement = actor.Does(xapi.generic.verbs.clicked)
                    .Activity(xapi.generic.activities.mouse, 
                        xapi.generic.extensions.activity
                            .mouseButton(args.mouseButton)
                            .mousePosition(args.mousePosition));
                SendStatement(statement);
            });
            tb.OnPressedDown.AddHandler((_, args) =>
            {
                var statement = actor.Does(xapi.generic.verbs.pressed)
                    .Activity(xapi.generic.activities.mouse, 
                        xapi.generic.extensions.activity
                            .mouseButton(args.mouseButton)
                            .mousePosition(args.mousePosition));
                SendStatement(statement);
            });
            tb.OnScrolledWheel.AddHandler((_, args, value) =>
            {
                var statement = actor.Does(xapi.generic.verbs.scrolled)
                    .Activity(xapi.generic.activities.mouse, 
                        xapi.generic.extensions.activity
                            .mouseButton(args.mouseButton)
                            .mousePosition(args.mousePosition)
                            .scrollValue(value));
                SendStatement(statement);
            });
            tb.OnMousePositionChanged.AddHandler((_, pos) =>
            {
                var statement = actor.Does(xapi.generic.verbs.moved)
                    .Activity(xapi.generic.activities.mouse)
                    .WithExtension(xapi.generic.extensions.activity.mousePosition(pos));
                SendStatement(statement);
            });
        }
    }
}