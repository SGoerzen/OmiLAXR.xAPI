using xAPI.Registry;

namespace OmiLAXR.xAPI.Extensions
{
    public static class xApiRegistry_Ext
    {
        public static xAPI_Actor ToXAPIActor(this Actor actor)
        {
            return new xAPI_Actor(actor.actorName, actor.actorEmail);
        }
    }
}