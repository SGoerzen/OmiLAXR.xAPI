using OmiLAXR.Context;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI.Hooks
{
    [AddComponentMenu("OmiLAXR / 5) Hooks / Scenario Context Hook (xAPI)")]
    public class ScenarioContextHook : xApiHook
    {
        protected override xApiStatement AfterCompose(xApiStatement statement)
        {
            if (ScenarioLanguage.Instance)
            {
                var language = ScenarioLanguage.Instance.language;
                var langCode = language.ToString();
                // ensure it has not "_" prefix
                statement.WithLanguage(langCode[0] == '_' ? langCode.Substring(1) : langCode);
            }

            if (PlatformInformation.Instance)
            {
                // Add platform information
                var platform = PlatformInformation.Instance.GetPlatformString();
                statement.WithPlatform(platform);
            }

            // add application name and version
            statement.WithContext(xapi.generic.extensions.context
                .learningScenario(Application.productName)
                .version(Application.version));

            return statement;
        }
    }
}