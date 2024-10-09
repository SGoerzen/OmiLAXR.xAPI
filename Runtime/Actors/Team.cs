using UnityEngine;

namespace OmiLAXR.xAPI.Actors
{
    /// <summary>
    /// Agents or Groups can also be included in the ‘context’ of a statement as an ‘instructor,’ leading to statements of the form “Brian (actor) learned xAPI from Ben (instructor).” Context can also include a ‘team’ property but it must be a Group.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 0) Pipelines / Team")]
    public class Team : PipelineComponent
    {
        public string teamName = "Team";
        public string teamEmail = "team@omilaxr.dev";
    }
}