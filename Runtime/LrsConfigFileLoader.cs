using System.IO;
using UnityEngine;

namespace OmiLAXR.xAPI.Tracking
{
    [RequireComponent(typeof(LearningRecordStore))]
    public class LrsCredentialsLoader : MonoBehaviour
    {
        private LearningRecordStore _lrs;
        
        private void Awake()
        {
            _lrs = GetComponent<LearningRecordStore>();
            LoadConfig();
        }

        private void LoadConfig()
        {
            string filePath;
#if UNITY_EDITOR
            filePath = Path.Combine(Application.dataPath, "lrs.credentials.json");
#else
            filePath = Path.Combine(Application.dataPath, "../lrs.config.json");
#endif

            if (File.Exists(filePath))
            {
                var jsonContent = File.ReadAllText(filePath);
                var credentials = JsonUtility.FromJson<LrsCredentials>(jsonContent);

                _lrs.credentials = credentials;

                Debug.Log("Loaded 'lrs.config.json' successfully");
            }
            else
            {
                Debug.LogError($"Cannot find 'lrs.config.json' in path '{filePath}'.");
            }
        }
    }

   
}