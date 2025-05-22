#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Mouse Statement Composer (xAPI)")]
    [Description("Creates statements:\n- actor clicked mouse with mouseButton(String), mousePosition(Vector3)" +
                 "\n- actor pressed mouse with mouseButton(String), mousePosition(Vector3)" +
                 "\n- actor scrolled mouse with mouseButton(String), mousePosition(Vector3), scrollValue(Float)" +
                 "\n- actor moved mouse with mouseButton(String), mousePosition(Vector3) and result startValue(Vector3), endValue(Vector3), value(Vector3)")]
    public sealed class MouseComposer : xApiComposer<MouseTrackingBehaviour>
    {
        public override Author GetAuthor()
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
            tb.OnMousePositionChanged.AddHandler((_, pos, oldPos) =>
            {
                var change = pos - oldPos;
                var statement = actor.Does(xapi.generic.verbs.moved)
                    .Activity(xapi.generic.activities.mouse)
                    .WithExtension(xapi.generic.extensions.activity
                        .mousePosition(pos))
                    .WithResult(xapi.generic.extensions.result
                        .startValue(oldPos)
                        .endValue(pos)
                        .value(change)); // todo add delta value, add from to value
                SendStatement(statement);
            });
        }
    }
}
#endif