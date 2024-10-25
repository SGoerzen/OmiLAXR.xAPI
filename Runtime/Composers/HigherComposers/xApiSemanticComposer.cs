using OmiLAXR.Composers.HigherComposers;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers.HigherComposers
{
    public abstract class xApiSemanticComposer<TActorOfPipeline> : HigherComposer<xApiStatement>
        where TActorOfPipeline : Pipeline
    {
        protected readonly xAPI_Contexts xapi = new xAPI_Contexts();

        protected xApiStatement.ActorRole actor;
        protected TActorOfPipeline targetPipeline;

        protected virtual void Awake()
        {
            targetPipeline = GetPipeline<TActorOfPipeline>();
            actor = new xApiStatement.ActorRole(targetPipeline.actor, GetAuthor(), targetPipeline.instructor);
        }
    }
}