using UnityEngine;

namespace OmiLAXR.xAPI
{
    [AddComponentMenu("OmiLAXR / 0) Pipelines / xAPI Data Provider")]
    public class xApiDataProvider : DataProvider
    {
        [Tooltip("xAPI Base URL")]
        public string statementIdUri = "https://xapi.elearn.rwth-aachen.de/definitions/";
    }
}