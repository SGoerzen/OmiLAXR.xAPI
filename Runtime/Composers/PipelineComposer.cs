#if XAPI_REGISTRY_EXISTS
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.System;

namespace OmiLAXR.xAPI.Composers
{
    public class PipelineComposer : xApiComposer<PipelinesTrackingBehaviour>
    {
        public override Author GetAuthor()
        {
            throw new System.NotImplementedException();
        }

        protected override void Compose(PipelinesTrackingBehaviour tb)
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif