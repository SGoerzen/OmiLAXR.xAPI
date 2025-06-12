#if XAPI_REGISTRY_EXISTS
using System.Linq;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers
{
    public abstract class xApiComposer<T> : Composer<T>
        where T : ActorPipelineComponent, ITrackingBehaviour
    {
        /// <summary>
        /// Who is responsible for the correctness of the statement.
        /// </summary>
        protected xApiStatement.Builder actor;
        protected static xAPI_Contexts xapi => xApiRegistry.definitions;

        protected string Uri = "";
        protected virtual string GetUri() => Uri;

        protected override void OnEnable()
        {
            var globalUri = GetComponentInParent<xApiRegistry>();
            if (globalUri && globalUri.enabled)
                Uri = globalUri.uri;
            
            actor = new xApiStatement.Builder(GetUri(), GetAuthor());
            
            base.OnEnable();
        }

        
    }
}
#endif