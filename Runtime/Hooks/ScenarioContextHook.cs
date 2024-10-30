using System;
using System.ComponentModel;
using OmiLAXR.Context;
using OmiLAXR.Context.HeartRate;
using OmiLAXR.xAPI.Composers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OmiLAXR.xAPI.Hooks
{
    [AddComponentMenu("OmiLAXR / 5) Hooks / Scenario Context Hook (xAPI)")]
    [Description("Adds language by <ScenarioLanguage>, platform by <PlatformInformation>, fps by <FpsMonitor>, registration by <Registration> and active scene, game and game version to the context of all statements.")]
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

            if (FpsMonitor.Instance && FpsMonitor.Instance.enabled)
            {
                statement.WithContext(xapi.systemControl.extensions.context.fps(FpsMonitor.Instance.fps));
            }

            if (HeartRateMonitor.Instance && HeartRateMonitor.Instance.enabled)
            {
                var provider = HeartRateMonitor.Instance.provider;
                if (!provider)
                {
                    DebugLog.xAPI.Error("Cannot provider heart rate as no heart rate provider is assigned to the Heart Rate Monitor.");
                }
                else if (provider.enabled)
                {
                    statement.WithContext(xapi.gestures.extensions.context.heartRate(provider.GetHeartRate()));
                }
            }

            if (Registration.Instance && Registration.Instance.enabled)
            {
                statement.WithRegistration(new Guid(Registration.Instance.uuid));
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