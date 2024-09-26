using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Keyboard Statement Composer (xAPI)")]
    public class KeyboardComposer : XApiComposer<KeyboardTrackingBehaviour>
    {
        protected override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(KeyboardTrackingBehaviour tb)
        {
            tb.OnPressed.AddHandler((_, args) =>
            {
                var verb = args.isDown ? xapi.generic.verbs.pressed : xapi.generic.verbs.released;
                var stmt = actor.Does(verb)
                    .Activity(xapi.generic.activities.keyboard)
                    .WithExtension(xapi.generic.extensions.activity.keyboardButton(args.key));
                SendStatement(stmt);
            });
        }
    }
}