using OmiLAXR.Pipelines.Learner;

namespace OmiLAXR.xAPI.Composers
{
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