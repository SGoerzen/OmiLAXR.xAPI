#if !XAPI_REGISTRY_EXISTS
using System;
using System.Collections;
using System.Collections.Generic;
using OmiLAXR;

namespace xAPI.Registry
{
    internal static class xApiRegistry
    {
        public const string CatchMessage = "Use 'xAPI4Unity to create a valid xAPI Registry.";
    }
    public static class OmiLAXR_Ext {
        public static xAPI_Actor ToXAPIActor(this Actor actor) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public static xAPI_Actor[] ToXAPIActors(this Actor[] actor) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public static xAPI_Actor ToXAPIActor(this Team team) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public static xAPI_Actor ToXAPIActor(this Instructor team) => throw new NotImplementedException(xApiRegistry.CatchMessage);
    }
    public struct xAPI_Actor
    {
        public string Name => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public string Email => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public xAPI_Actor(string name = "", string email = "")
            => throw new NotImplementedException(xApiRegistry.CatchMessage);
    }
    public class xAPI_Contexts {}

    public abstract class xAPI_Definition
    {
        public string Key => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public Dictionary<string, string> Names => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public Dictionary<string, string> Descriptions => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public string CreateValidId(string uri) =>
            throw new NotImplementedException(xApiRegistry.CatchMessage);
        public bool MatchesPath(string path, char sep = '/') => throw new NotImplementedException(xApiRegistry.CatchMessage);
    }
    public class xAPI_Activity : xAPI_Definition {}

    public class xAPI_Verb : xAPI_Definition
    {
        
    }

    public class xAPI_Extension : xAPI_Definition
    {
        
    }

    public abstract class xAPI_Extensions : Dictionary<xAPI_Extension, object>
    {
        public bool ContainsKey(string key) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public bool ContainsPath(string key) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public bool AddRange(xAPI_Extensions ext) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public bool Remove(string path, char sep = '/') => throw new NotImplementedException(xApiRegistry.CatchMessage);
        public bool RemoveByKey(string key) => throw new NotImplementedException(xApiRegistry.CatchMessage);
    }
    public class xAPI_Extensions_Context : xAPI_Extensions {}
    public class xAPI_Extensions_Result : xAPI_Extensions {}
    public class xAPI_Extensions_Activity : xAPI_Extensions {}
}
#endif