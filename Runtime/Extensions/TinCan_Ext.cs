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
            foreach (var (extension, value) in extensions)
            {
                var id = extension.CreateValidId(uri);
                jObject.Add(id, value == null ? "null" : value.ToString());
            }
        }

        public static tc.Statement ToTinCanStatement(this xApiStatement s, string statementUri)
        {
            var customTimestamp = s.GetTimestamp();
            var actor = s.GetActor();
            var stmt = new tc.Statement()
            {
                // Statement Meta
                timestamp = customTimestamp ?? s.CreatedAt,
                version = tc.TCAPIVersion.latest(),
                authority = s.GetAuthority().ToTinCanAgent(),
                // Actor
                actor = s.IsInGroup ? actor.ToTinCanAgentGroup(s.GetGroupMembers()) : actor.ToTinCanAgent(),
                // Verb
                verb = s.GetVerb().ToTinCanVerb(statementUri),
                // Activity + Activity Extension
                target = s.GetActivity().ToTinCanActivity(statementUri, s.GetActivityExtensions()),
                // Context
                context = s.GetContextExtensions().ToTinCanContext(statementUri, s.GetLanguage(), s.GetPlatform(), s.GetInstructor(), s.GetTeam(), s.GetTeamMembers(), s.GetRegistration()),
                // Result
                result = s.GetResultExtensions().ToTinCanResult(statementUri, s.GetScore(), s.GetCompletion(), s.GetSuccess(), s.GetResponse()),
            };
            return stmt;
        }
        
        public static tc.Context ToTinCanContext(this xAPI_Extensions_Context extensions, string uri, string language, string platform, xAPI_Actor? instructor = null, xAPI_Actor? team = null, xAPI_Actor[] teamMembers = null, Guid? registration = null)
        {
            return new tc.Context()
            {
                instructor = instructor?.ToTinCanAgent(),
                extensions = extensions?.ToTinCanExtensions(uri),
                registration = registration,
                team = team?.ToTinCanAgentTeam(teamMembers),
                language = language,
                platform = platform
            };
        }

        public static tc.Result ToTinCanResult(this xAPI_Extensions_Result extensions, string uri, tc.Score score = null, bool? completion = null, bool? success = null, string response = null)
        {
            return new tc.Result
            {
                score = score,
                completion = completion,
                success = success,
                extensions = extensions?.ToTinCanExtensions(uri),
                response = response
            };
        }
        
        
        private static tc.Verb ToTinCanVerb(this xAPI_Verb verb, string uri)
        {
            var v = new tc.Verb
            {
               
                id = new Uri(verb.CreateValidId(uri)),
                display = new tc.LanguageMap()
            };

            foreach (var name in verb.Names)
                v.display.Add(name.Key, name.Value);

            return v;
        }

        private static tc.Activity ToTinCanActivity(this xAPI_Activity activity, string uri, xAPI_Extensions extensions = null)
        {            
            var a = new tc.Activity
            {
                id = activity.CreateValidId(uri), // todo: combine with type
                definition = activity.ToTinCanActivityDefinition(),
            };

            a.definition.extensions = extensions?.ToTinCanExtensions(uri);

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
        
        public static tc.Extensions ToTinCanExtensions(this xAPI_Extensions extensions, System.Uri uri)
            => ToTinCanExtensions(extensions, uri.ToString());
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
