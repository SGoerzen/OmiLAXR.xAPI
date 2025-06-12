#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Heart Rate Composer (xAPI)")]
    [Description("Creates statements:\n- actor measured heart rate with heartRate(Int)")]
    public class HeartRateComposer : xApiComposer<HeartRateTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        protected override void Compose(HeartRateTrackingBehaviour tb)
        {
            enabled = false;
            throw new System.NotImplementedException();
        }
    }
}
#endif