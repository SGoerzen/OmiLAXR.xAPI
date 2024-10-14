using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.System;

namespace OmiLAXR.xAPI
{
    public class TransformComposer : Composer<TransformTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        protected override void Compose(TransformTrackingBehaviour tb)
        {
            
        }
    }
}
