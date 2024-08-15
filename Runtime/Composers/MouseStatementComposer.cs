using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 6) Composers / Mouse Statement Composer (xAPI)")]
    public class MouseStatementComposer : xApiStatementComposer<MouseTrackingBehaviour>
    {
        protected override void Awake()
        {
            base.Awake();
            var tb = trackingBehaviour;
            tb.OnClicked += name => { };
            tb.OnPressedDown += name => { };
            tb.OnScrolledWheel += y => { };
        }
    }
}