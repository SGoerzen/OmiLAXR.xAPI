using UnityEngine;

namespace OmiLAXR.xAPI
{
    [System.Serializable]
    public struct LrsCredentials
    {
        [InspectorName("xAPI Endpoint")]
        public string endpoint;
        public string key;
        public string secret;

        public LrsCredentials(string endpoint, string key, string secret)
        {
            this.endpoint = endpoint;
            this.key = key;
            this.secret = secret;
        }
    }
}