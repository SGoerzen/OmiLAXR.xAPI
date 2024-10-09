using OmiLAXR.xAPI.Actors;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Extensions
{
    public static class xApiRegistry_Ext
    {
        public static xAPI_Actor ToXAPIActor(this Actor actor)
        {
            return new xAPI_Actor(actor.actorName, actor.actorEmail);
        }
        public static xAPI_Actor ToXAPIActor(this Instructor actor)
        {
            return new xAPI_Actor(actor.instructorName, actor.instructorEmail);
        }
        public static xAPI_Actor ToXAPIActor(this Team actor)
        {
            return new xAPI_Actor(actor.teamName, actor.teamEmail);
        }
        public static xAPI_Actor ToXAPIActor(this ActorGroup actor)
        {
            return new xAPI_Actor(actor.groupName, actor.groupEmail);
        }
    }
}