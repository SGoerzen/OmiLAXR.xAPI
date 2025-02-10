using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Input System Composer (OmiLAXR.xAPI)"),
     Description("Creates statements:\n- actor pressed action with actionName(String)")]
    public class InputSystemComposer : xApiComposer<InputSystemTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        protected override void Compose(InputSystemTrackingBehaviour tb)
        {
            tb.OnPressedAnyButton.AddHandler((_, args) =>
            {
                var stmt = actor.Does(xapi.virtualReality.verbs.pressed)
                    .Activity(xapi.virtualReality.activities.action)
                    .WithResult(xapi.generic.extensions.result.deviceId(args.DeviceId).deviceName(args.DeviceName))
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .actionName(args.ButtonName));
                SendStatement(stmt);
            });
        }
    }
}