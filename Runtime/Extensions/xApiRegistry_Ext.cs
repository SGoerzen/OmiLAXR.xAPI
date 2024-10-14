using System.Collections.Generic;
using System.Linq;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Extensions
{
    public static class xApiRegistry_Ext
    {
        public static xAPI_Actor ToXAPIActor(this Actor actor)
            => new xAPI_Actor(actor.actorName, actor.actorEmail);
        public static IEnumerable<xAPI_Actor> ToXAPIActors(this IEnumerable<Actor> actors)
            => actors.Select(a => a.ToXAPIActor());
        
        public static xAPI_Actor ToXAPIActor(this Instructor actor)
            => new xAPI_Actor(actor.instructorName, actor.instructorEmail);
        
        public static xAPI_Actor ToXAPIActor(this Team team)
            => new xAPI_Actor(team.teamName, team.teamEmail);
    }
}