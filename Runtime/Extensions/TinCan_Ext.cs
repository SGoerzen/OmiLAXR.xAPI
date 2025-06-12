using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using OmiLAXR.xAPI.Composers;
using xAPI.Registry;
using tc = TinCan;

namespace OmiLAXR.xAPI.Extensions
{
    public static class TinCan_Ext
    {
        public static void AssignTo(this xAPI_Extensions extensions, JObject jObject, string uri)
        {
            // Skip if there are no extensions
            if (jObject == null)
                return;
#if UNITY_2021_1_OR_NEWER 
            foreach (var (extension, value) in extensions)
            {
                var id = extension.CreateId(uri);
                jObject.Add(id, value == null ? "null" : value.ToString());
            }
#else
            foreach (var kvp in extensions)
            {
                var id = kvp.Key.CreateId(uri);
                jObject.Add(id, kvp.Value == null ? "null" : kvp.Value.ToString());
            }
#endif
        }

        public static tc.Statement ToTinCanStatement(this xApiStatement s)
        {
            var customTimestamp = s.GetTimestamp();
            var actor = s.GetActor();
            var stmt = new tc.Statement()
            {
                id = s.GetId(),
                // Statement Meta
                timestamp = customTimestamp ?? s.CreatedAt,
                version = tc.TCAPIVersion.latest(),
                authority = s.GetAuthority().ToTinCanAgent(),
                // Actor
                actor = s.IsInGroup ? actor.ToTinCanAgentGroup(s.GetGroupMembers()) : actor.ToTinCanAgent(),
                // Verb
                verb = s.GetVerb().ToTinCanVerb(s.GetUri()),
                // Activity + Activity Extension
                target = s.GetActivity().ToTinCanActivity(s.GetUri(), s.GetActivityExtensions(), s.GetInteractionType(), s.GetCorrectResponses()),
                // Context
                context = s.GetContextExtensions().ToTinCanContext(s.GetUri(), s.GetLanguage(), s.GetPlatform(), s.GetInstructor(), s.GetTeam(), s.GetTeamMembers(), s.GetRegistration()),
                // Result
                result = s.GetResultExtensions().ToTinCanResult(s.GetUri(), s.GetScore(), s.GetCompletion(), s.GetSuccess(), s.GetResponse()),
            };
            return stmt;
        }
        
        public static tc.Context ToTinCanContext(this xAPI_Extensions_Context extensions, string uri, string language, string platform, xAPI_Actor? instructor = null, xAPI_Actor? team = null, xAPI_Actor[] teamMembers = null, Guid? registration = null, Guid? refId = null)
        {
            var ctx = new tc.Context()
            {
                instructor = instructor?.ToTinCanAgent(),
                extensions = extensions?.ToTinCanExtensions(uri),
                registration = registration,
                team = team?.ToTinCanAgentTeam(teamMembers),
                language = language,
                platform = platform,
            };
            
            if (refId != null)
                ctx.statement = new tc.StatementRef(refId.Value);

            return ctx;
        }

        public static tc.Result ToTinCanResult(this xAPI_Extensions_Result extensions, string uri, tc.Score score = null, bool? completion = null, bool? success = null, string response = null, TimeSpan? duration = null)
        {
            return new tc.Result
            {
                score = score,
                completion = completion,
                success = success,
                extensions = extensions?.ToTinCanExtensions(uri),
                response = response,
                duration = duration
            };
        }

        private static Uri CreateValidUri(this xAPI_Definition definition, string uri)
        {
            var isRelative = string.IsNullOrEmpty(uri) || uri == "/";
            var defPath = definition.CreateId(uri);

            if (isRelative && defPath[0] == '/')
                defPath = defPath.Substring(1);
            
            return new Uri(defPath, isRelative ? UriKind.Relative : UriKind.RelativeOrAbsolute);
        } 
        
        public static tc.Verb ToTinCanVerb(this xAPI_Verb verb, string uri)
        {
            var v = new tc.Verb
            {
               
                id = CreateValidUri(verb, uri),
                display = new tc.LanguageMap()
            };

            foreach (var name in verb.Names)
                v.display.Add(name.Key, name.Value);

            return v;
        }

        #if XAPI_REGISTRY_EXISTS
        private static string CreateId(this xAPI_Definition definition, string uri)
            => string.IsNullOrEmpty(uri) ? definition.GetPath() : definition.CreateValidId(uri);
        #else
        private static string CreateId(this xAPI_Definition definition, string uri) => "";
        #endif
        
        public static tc.Activity ToTinCanActivity(this xAPI_Activity activity, string uri, xAPI_Extensions extensions = null, tc.InteractionType interactionType = null, List<string> correctResponsesPattern = null)
        {            
            var a = new tc.Activity
            {
                id = activity.CreateId(uri),
                definition = activity.ToTinCanActivityDefinition(),
            };
            
            a.definition.extensions = extensions?.ToTinCanExtensions(uri);
            a.definition.interactionType = interactionType;
            a.definition.correctResponsesPattern = correctResponsesPattern;

            return a;
        }
        
        public static tc.Agent ToTinCanAgentTeam(this xAPI_Actor actor, xAPI_Actor[] members)
        {
            return new tc.Group()
            {
                name = actor.Name,
                mbox = "mailto:" + actor.Email,
                member = members.Select(m => m.ToTinCanAgent()).ToList()
            };
        }
        
        public static tc.Agent ToTinCanAgent(this xAPI_Actor actor)
        {
            return new tc.Agent
            {
                name = actor.Name,
                mbox = "mailto:" + actor.Email
            };
        }
        
        public static tc.Group ToTinCanAgentGroup(this xAPI_Actor actor, xAPI_Actor[] members)
        {
            return new tc.Group()
            {
                name = actor.Name,
                mbox = "mailto:" + actor.Email,
                member = members.Select(m => m.ToTinCanAgent()).ToList()
            };
        }
        
        public static tc.Extensions ToTinCanExtensions(this xAPI_Extensions extensions, string uri)
        {
            var jObject = new JObject();
            extensions.AssignTo(jObject, uri);

            return new tc.Extensions(jObject);
        }
        public static tc.ActivityDefinition ToTinCanActivityDefinition(this xAPI_Definition definition)
        {
            return new tc.ActivityDefinition
            {
                // translate xAPI name definitions to LanguageMap
                name = new tc.LanguageMap(definition.Names),
                description = new tc.LanguageMap(definition.Descriptions)
            };
        }
    }
}