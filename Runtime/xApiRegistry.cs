using System.ComponentModel;
using UnityEngine;
using xAPI.Registry;

namespace OmiLAXR.xAPI
{
    [AddComponentMenu("OmiLAXR / xAPI Registry")]
    [Description("This is an instance for managing xAPI Registry settings.")]
    public class xApiRegistry : MonoBehaviour
    {
        public static xAPI_Contexts definitions = new xAPI_Contexts();
        public string uri = "https://xapi.elearn.rwth-aachen.de/definitions";

        //private static TinCan.TCAPIVersion version = TinCan.TCAPIVersion.latest();
        
    }
}