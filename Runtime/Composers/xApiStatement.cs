using System;
using OmiLAXR.Composers;
using OmiLAXR.xAPI.Actors;
using OmiLAXR.xAPI.Extensions;
using TinCan;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers
{
    public sealed class xApiStatement : IStatement
    {
        public class ActorRole
        {
            internal readonly Actor _actor;
            internal readonly Author _authority;

            public ActorRole(Actor actor, Author author)
            {
                _actor = actor;
                _authority = author;
            }
            
            public ActorRole(ActorGroup group)
            {
                throw new NotImplementedException();
            }
            
            public xApiStatement Verb(xAPI_Verb verb)
                => new xApiStatement(this, verb);

            public xApiStatement Does(xAPI_Verb verb)
                => new xApiStatement(this, verb);
        }

        private xAPI_Verb _verb;
        private xAPI_Activity _activity;
        private xAPI_Actor _actor;
        private xAPI_Actor? _instructor;
        private xAPI_Actor _authority;
        private xAPI_Actor? _team;

        private readonly xAPI_Extensions_Activity _activityExtensions = new xAPI_Extensions_Activity();
        private readonly xAPI_Extensions_Context _contextExtensions = new xAPI_Extensions_Context();
        private readonly xAPI_Extensions_Result _resultExtensions = new xAPI_Extensions_Result();

        public xAPI_Extensions_Activity GetActivityExtensions() => _activityExtensions;
        public xAPI_Extensions_Context GetContextExtensions() => _contextExtensions;
        public xAPI_Extensions_Result GetResultExtensions() => _resultExtensions;
        public xAPI_Activity GetActivity() => _activity;
        public xAPI_Verb GetVerb() => _verb;
        public xAPI_Actor GetActor() => _actor;
        public xAPI_Actor? GetTeam() => _team;
        public Score GetScore() => _score;
        public bool? GetSuccess() => _success;
        public bool? GetCompletion() => _completion;
        public string GetResponse() => _response;
        public xAPI_Actor? GetInstructor() => _instructor;
        public xAPI_Actor GetAuthority() => _authority;
        public readonly DateTime CreatedAt = DateTime.Now;
        
        private Score _score = null;
        private bool? _success = null;
        private bool? _completion = null;
        private string _response = null;

        private bool _isDiscarded;
        public bool IsDiscarded() => _isDiscarded;
        
        #region Matching Methods
        public bool MatchPaths(string verbPath, string activityPath, char pathSeperator = '.')
            => _verb.MatchesPath(verbPath, pathSeperator) && _activity.MatchesPath(activityPath, pathSeperator);
        public bool MatchKeys(string verbKey, string activityKey)
            => _verb.Key == verbKey && _activity.Key == activityKey;
        public bool HasVerb(string verbPath)
            => GetVerb().MatchesPath(verbPath);
        public bool HasVerbKey(string verbKey)
            => GetVerb().Key == verbKey;
        public bool HasActivity(string activityPath)
            => GetActivity().MatchesPath(activityPath);
        public bool HasActivityKey(string activityKey)
            => GetActivity().Key == activityKey;
        public bool HasContextKey(string contextKey)
            => _contextExtensions.ContainsKey(contextKey);
        public bool HasContext(string contextPath)
            => _contextExtensions.ContainsPath(contextPath);
        public bool HasExtensionKey(string extensionKey)
            => _activityExtensions.ContainsKey(extensionKey);
        public bool HasExtension(string extensionPath)
            => _activityExtensions.ContainsPath(extensionPath);
        public bool HasResultKey(string resultKey)
            => _resultExtensions.ContainsKey(resultKey);
        public bool HasResult(string resultPath)
            => _resultExtensions.ContainsPath(resultPath);
        #endregion

        public xApiStatement Activity(xAPI_Activity activity,
            xAPI_Extensions_Activity activityExtensions = null)
        {
            _activity = activity;

            if (activityExtensions != null)
                _activityExtensions.AddRange(activityExtensions);

            return this;
        }
        
        public xApiStatement Verb(xAPI_Verb verb)
        {
            _verb = verb;
            return this;
        }
        // public void Object(xAPI_Activity obj) {}

        public xApiStatement WithExtension(xAPI_Extensions_Activity activityExtensions)
        {
            _activityExtensions.AddRange(activityExtensions);
            return this;
        }

        public void Discard()
        {
            _isDiscarded = true;
        }
        
        /// <summary>
        /// Use only if you know what you do!
        /// </summary>
        public xApiStatement ChangeActor(xAPI_Actor actor)
        {
            _actor = actor;
            return this;
        }
        /// <summary>
        /// Use only if you know what you do!
        /// </summary>
        public xApiStatement ChangeActor(Actor actor)
        {
            _actor = actor.ToXAPIActor();
            return this;
        }
        public xApiStatement WithTeam(xAPI_Actor team)
        {
            _team = team;
            return this;
        }
        public xApiStatement WithTeam(Team team)
        {
            _team = team.ToXAPIActor();
            return this;
        }
        public xApiStatement WithInstructor(xAPI_Actor instructor)
        {
            _instructor = instructor;
            return this;
        }
        public xApiStatement WithInstructor(Instructor instructor)
        {
            _instructor = instructor.ToXAPIActor();
            return this;
        }

        public xApiStatement ChangedBy(string name, string email)
        {
            _authority = new xAPI_Actor(name, email);
            return this;
        }
        
        public xApiStatement DropExtension(string path, char pathSeparator = '.')
        {
            _activityExtensions.Remove(path, pathSeparator);
            return this;
        }
        
        public xApiStatement DropExtensionByKey(string key)
        {
            _activityExtensions.RemoveByKey(key);
            return this;
        }

        public xApiStatement DropExtensions()
        {
            _activityExtensions.Clear();
            return this;
        }


        public xApiStatement WithSuccess(bool success)
        {
            this._success = success;
            return this;
        }

        public xApiStatement DropSuccess()
        {
            _success = null;
            return this;
        }

        public xApiStatement WithCompletion(bool completion)
        {
            this._completion = completion;
            return this;
        }

        public xApiStatement DropCompletion()
        {
            _completion = null;
            return this;
        }

        public xApiStatement WithResponse(string response)
        {
            this._response = response;
            return this;
        }

        public xApiStatement DropResponse()
        {
            _response = null;
            return this;
        }

        public xApiStatement WithContext(xAPI_Extensions_Context extensions)
        {
            _contextExtensions.AddRange(extensions);
            return this;
        }

        public xApiStatement DropContext(string path, char pathSeparator = '.')
        {
            _contextExtensions.Remove(path, pathSeparator);
            return this;
        }
        
        public xApiStatement DropContextKey(string key)
        {
            _contextExtensions.RemoveByKey(key);
            return this;
        }

        public xApiStatement DropContexts()
        {
            _contextExtensions.Clear();
            return this;
        }

        public xApiStatement WithResult(xAPI_Extensions_Result extensions)
        {
            _resultExtensions.AddRange(extensions);
            return this;
        }

        public xApiStatement DropResultKey(string key)
        {
            _resultExtensions.RemoveByKey(key);
            return this;
        }
        
        public xApiStatement DropResult(string path, char pathSeparator = '.')
        {
            _resultExtensions.Remove(path, pathSeparator);
            return this;
        }

        public xApiStatement DropResults()
        {
            _resultExtensions.Clear();
            return this;
        }

        public xApiStatement WithScore(double raw, double? min, double? max)
        {
            if (max == 0)
                throw new ArgumentException("The parameter 'max' is not allowed to be 0.");

            _score = new Score()
            {
                raw = raw,
                min = min,
                max = max,
                scaled = min / max
            };

            return this;
        }

        public xApiStatement DropScore()
        {
            _score = null;
            return this;
        }

        public xApiStatement(ActorRole actor, xAPI_Verb verb)
        {
            _actor = actor._actor.ToXAPIActor();
            _authority = new xAPI_Actor(actor._authority.Name, actor._authority.Email);
            _verb = verb;
            _contextExtensions = new xAPI_Extensions_Context();
            _resultExtensions = new xAPI_Extensions_Result();
        }

        public override string ToString()
            =>
                $"[xApiStatement verb: {_verb}, activity: {_activity}, ext: {_activityExtensions}, ctx: {_contextExtensions}, result: {_resultExtensions}, score: {_score}, success: {_success}, completion: {_completion}]";
    }
}