
using UnityEngine;

namespace OmiLAXR.xAPI.Actors
{
    /// <summary>
    /// Agents or Groups can also be included in the ‘context’ of a statement as an ‘instructor,’ leading to statements of the form “Brian (actor) learned xAPI from Ben (instructor).” 
    /// </summary>
    [AddComponentMenu("OmiLAXR / 0) Pipelines / Instructor")]
    public class Instructor : PipelineComponent
    {
        public string instructorName = "Instructor";
        public string instructorEmail = "instructor@omilaxr.dev";
    }
}