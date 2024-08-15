using System;
using System.Collections.Generic;
using System.Linq;
using OmiLAXR.xAPI.Extensions;
using TinCan;
using xAPI.Registry;

namespace OmiLAXR.xAPI
{
    [Obsolete("Try to use composer instead.")]
    public static class xAPIv1
    {
        public static Agent CreateAgent(xAPI_Actor actor) => CreateAgent(actor.Name, actor.Email);
        /// <summary>
        /// Create the actor parameter of xAPI statement identified by email address.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mail"></param>
        /// <returns></returns>
        public static Agent CreateAgent(string name, string mail)
        {
            return new Agent
            {
                //account = new AgentAccount(new Uri("https://learninglocker.net"), name),
                name = name,
                mbox = "mailto:" + mail
            };
        }

        private static Verb CreateVerb(string uri, xAPI_Verb verb)
        {
            var v = new Verb
            {
               
                id = new Uri(verb.CreateValidId(uri)),
                display = new LanguageMap()
            };

            foreach (var name in verb.Names)
                v.display.Add(name.Key, name.Value);

            return v;
        }

        private static Activity CreateActivity(string uri, xAPI_Activity activity, xAPI_Extensions extensions = null)
        {            
            var a = new Activity
            {
                id = activity.CreateValidId(uri),
                definition = activity.ToTinCanActivityDefinition()
            };

            a.definition.extensions = extensions?.ToTinCanExtensions(uri);

            return a;
        }
        
        public static (xAPI_Extensions_Activity, xAPI_Extensions_Context, xAPI_Extensions_Result) SplitExtensions(
            xAPI_Extensions extensions)
        {
            if (extensions == null)
                return (null, null, null);

            var activityExtensions = new xAPI_Extensions_Activity();
            var contextExtensions = new xAPI_Extensions_Context();
            var resultExtensions = new xAPI_Extensions_Result();

            foreach (var ext in extensions)
            {
                if (activityExtensions.ExtensionType == ext.Key.extensionType)
                    activityExtensions.Add(ext);
                else if (contextExtensions.ExtensionType == ext.Key.extensionType)
                    contextExtensions.Add(ext);
                else if (resultExtensions.ExtensionType == ext.Key.extensionType)
                    resultExtensions.Add(ext);
            }

            return (activityExtensions, contextExtensions, resultExtensions);
        }

        public static Statement CreateStatement(
            string uri, 
            xAPI_Actor actor, 
            xAPI_Verb verb, 
            xAPI_Activity activity, 
            xAPI_Extensions_Activity activityExtensions = null,
            xAPI_Extensions_Context contextExtensions = null,
            xAPI_Extensions_Result resultExtensions = null,
            Score score = null,
            bool? completion = null,
            bool success = true,
            string response = null,
            xAPI_Actor? instructor = null,
            xAPI_Actor? team = null
        )
        {
            var s = new Statement
            {
                actor = CreateAgent(actor),
                verb = CreateVerb(uri, verb),
                target = CreateActivity(uri, activity, activityExtensions),
                context = CreateContext(uri, contextExtensions, instructor, team),
                result = CreateResult(uri, score, completion, success, response, resultExtensions),
                //add timestamp to statement
                timestamp = DateTime.Now,
                version = TCAPIVersion.latest()
            };

            return s;
        }
        
        public static Group CreateGroup(xAPI_Actor group, IEnumerable<xAPI_Actor> members)
            => CreateGroup(group.Name, group.Email, members);
        public static Group CreateGroup(string name, string mail, IEnumerable<xAPI_Actor> actors)
        {
            var members = actors.Select(a => CreateAgent(a.Name, a.Email));
            return new Group
            {
                name = name,
                mbox = "mailto:" + mail,
                member = members.ToList()
            };
        }
        
        public static Statement CreateStatement(
            string uri, 
            xAPI_Actor group,
            xAPI_Actor[] groupMembers, 
            xAPI_Verb verb, 
            xAPI_Activity activity, 
            xAPI_Extensions_Activity activityExtensions = null,
            xAPI_Extensions_Context contextExtensions = null,
            xAPI_Extensions_Result resultExtensions = null,
            Score score = null,
            bool? completion = null,
            bool success = true,
            string response = null,
            xAPI_Actor? instructor = null,
            xAPI_Actor? team = null
        )
        {
            var s = new Statement
            {
                actor = CreateGroup(group, groupMembers),
                verb = CreateVerb(uri, verb),
                target = CreateActivity(uri, activity, activityExtensions),
                context = CreateContext(uri, contextExtensions, instructor, team),
                result = CreateResult(uri, score, completion, success, response, resultExtensions),
                //add timestamp to statement
                timestamp = DateTime.Now,
                version = TCAPIVersion.latest()
            };

            return s;
        }

        /// <summary>
        /// https://learningpool.com/xapi-statement-101-context-element/
        /// https://xapi.com/blog/deep-dive-context/?utm_source=google&utm_medium=natural_search
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="extensions"></param>
        /// <param name="instructor"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public static Context CreateContext(string uri, xAPI_Extensions_Context extensions = null, xAPI_Actor? instructor = null, xAPI_Actor? team = null)
        {
            return new Context()
            {
                instructor = instructor.HasValue ? CreateAgent(instructor.Value) : null,
                extensions = extensions?.ToTinCanExtensions(uri),
                registration = null,
                team = team.HasValue ? CreateAgent(team.Value) : null,
            };
        }

        public static Result CreateResult(string uri, Score score = null, bool? completion = null, bool? success = null, string response = null, xAPI_Extensions_Result extensions = null)
        {
            return new Result
            {
                score = score,
                completion = completion,
                success = success,
                extensions = extensions?.ToTinCanExtensions(uri),
                response = response
            };
        }

        public static Statement CreateStatement(string uri, xAPI_Actor actor,
            xAPI_Verb verb, xAPI_Activity activity, xAPI_Extensions extensions = null,
            Score score = null, bool completion = true, bool success = true)
        {
            var (activityExtensions, contextExtensions, resultExtensions) = SplitExtensions(extensions);
            return CreateStatement(uri, actor, verb, activity, activityExtensions, contextExtensions, resultExtensions,
                score, completion, success);
        }
    }

}