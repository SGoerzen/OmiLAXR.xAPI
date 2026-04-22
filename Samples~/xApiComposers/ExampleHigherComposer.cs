using System.Collections.Generic;
using System.Linq;
using OmiLAXR.Composers;
using OmiLAXR.Pipelines;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace OmiLAXR.xAPI.Composers.HigherComposers
{
    public sealed class ExampleSemanticComposer : xApiSemanticComposer<LearnerPipeline>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        // Conditions that need to be met to trigger this composer
        protected override Dictionary<string, MatchCondition<xApiStatement>> Conditions()
            => new Dictionary<string, MatchCondition<xApiStatement>>
            {
                // actor clicked mouse
                { "clicked", new MatchCondition<xApiStatement>(1, stmt => stmt.MatchKeys("clicked", "mouse")) },
                // actor moved mouse
                { "moved", new MatchCondition<xApiStatement>(10, stmt => stmt.MatchKeys("moved", "mouse")) }
            };
        
        protected override void OnMatchAllConditions(Dictionary<string, IEnumerable<xApiStatement>> matchingStatements)
        {
            // Get clicked statement
            var clickedStatement = matchingStatements["clicked"].First();
            // Get first and last move statement
            var movedMatch = matchingStatements["moved"];
            if (movedMatch == null)
                return;
            var firstMoveStatement = movedMatch.First();
            var lastMoveStatement = movedMatch.Last();
            var startPosition = firstMoveStatement.GetExtensionValue<Vector3>("mousePosition");
            var endPosition = lastMoveStatement.GetExtensionValue<Vector3>("mousePosition");
            // Compose new statement
            var stmt = actor.Does(xapi.generic.verbs.achieved)
                .Activity(xapi.generic.activities.goal)
                .WithResult(xapi.generic.extensions.result.startValue(startPosition).endValue(endPosition))
                .WithExtension(xapi.generic.extensions.activity.name("dragged"));
            SendStatement(stmt);    
        }
    }
}
