using System;
using JetBrains.Annotations;
using TinCan;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers
{
    public class xApiStatement
    {
        public class Actor
        {
            internal xAPI_Actor[] _members;
            internal xAPI_Actor _actor;
            internal bool IsGroup => false;
            internal xAPI_Actor? _instructor = null;
            internal xAPI_Actor? _team = null;

            public Actor()
            {
                
            }

            public Actor(xAPI_Actor group, xAPI_Actor[] members)
            {
                _actor = group;
                _members = members;
            }
            
            public xApiStatement Verb(xAPI_Verb verb)
                => new xApiStatement(this, verb);
            
            public xApiStatement Does(xAPI_Verb verb)
                => new xApiStatement(this, verb);
        }
        
        private static xApiStatementBuilder _builder = new xApiStatementBuilder();
        private xApiStatement.Actor _actor;
        private xAPI_Verb _verb;
        private xAPI_Activity _activity;

        private xAPI_Extensions_Activity _activityExtensions;
        private xAPI_Extensions_Context _contextExtensions;
        private xAPI_Extensions_Result _resultExtensions;

        [CanBeNull] private Score _score;
        private bool? success = null;
        private bool? completion = null;
        [CanBeNull] private string response = null;

        public xApiStatement Activity(xAPI_Activity activity, 
            xAPI_Extensions_Activity activityExtensions = null)
        {
            _activity = activity;
            
            if (activityExtensions != null)
                _activityExtensions = activityExtensions;

            return this;
        }
        // public void Object(xAPI_Activity obj) {}

        public xAPI_Verb GetVerb() => _verb;

        public xApiStatement WithSuccess(bool success)
        {
            this.success = success;
            return this;
        }

        public xApiStatement WithCompletion(bool completion)
        {
            this.completion = completion;
            return this;
        }

        public xApiStatement WithResponse(string response)
        {
            this.response = response;
            return this;
        }
        
        public xApiStatement WithContext(xAPI_Extensions_Context extensions)
        {
            _contextExtensions.AddRange(extensions);
            return this;
        }

        public xApiStatement WithResult(xAPI_Extensions_Result extensions)
        {
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
        
        public xApiStatement(xApiStatement.Actor actor, xAPI_Verb verb)
        {
            _actor = actor;
            _verb = verb;
            _contextExtensions = new xAPI_Extensions_Context();
            _resultExtensions = new xAPI_Extensions_Result();
        }
    }
}