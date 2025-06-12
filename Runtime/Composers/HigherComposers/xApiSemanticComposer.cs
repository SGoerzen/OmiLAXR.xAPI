#if XAPI_REGISTRY_EXISTS
using OmiLAXR.Composers.HigherComposers;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers.HigherComposers
{
    public abstract class xApiSemanticComposer<TActorOfPipeline> : HigherComposer<xApiStatement>
        where TActorOfPipeline : Pipeline
    {
        protected readonly xAPI_Contexts xapi = new xAPI_Contexts();

        protected xApiStatement.Builder actor;
        protected TActorOfPipeline targetPipeline;
        protected string Uri = "";
        protected virtual string GetUri() => Uri;

        protected virtual void Awake()
        {
            var globalUri = GetComponentInParent<xApiRegistry>();
            if (globalUri && globalUri.enabled)
                Uri = globalUri.uri;

            targetPipeline = GetPipeline<TActorOfPipeline>();
            actor = new xApiStatement.Builder(GetUri(), GetAuthor());
        }
    }
}
#endif