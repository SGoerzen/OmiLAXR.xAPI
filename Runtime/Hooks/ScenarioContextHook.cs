using OmiLAXR.Context;
using OmiLAXR.xAPI.Composers;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            if (FpsMonitor.Instance)
            {
                statement.WithContext(xapi.systemControl.extensions.context.fps(FpsMonitor.Instance.fps));
            }

            // add application name and version
            var activeScene = SceneManager.GetActiveScene();
            // Get the name of the active scene
            var sceneName = activeScene.name;
            statement.WithContext(xapi.seriousGames.extensions.context.game(Application.productName));
            statement.WithContext(xapi.generic.extensions.context
                .learningScenario(sceneName)
                .version(Application.version));

            return statement;
        }
    }
}