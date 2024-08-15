using System.IO;
using OmiLAXR.Endpoints;
using UnityEngine;

namespace OmiLAXR.xAPI.Endpoints
{
    [AddComponentMenu("OmiLAXR / 6) Endpoints / LRS Credentials Loader (xAPI)")]
    [RequireComponent(typeof(LearningRecordStore))]
    public class LrsCredentialsLoader : MonoBehaviour
    {
        private LearningRecordStore _lrs;
        [Header("Name of file that is located in Assets folder (data path).")]
        public string filename = "lrs.credentials.json";
        
        private void Awake()
        {
            _lrs = GetComponent<LearningRecordStore>();
            LoadConfig();
        }

        private void LoadConfig()
        {
            string filePath;
#if UNITY_EDITOR
            filePath = Path.Combine(Application.dataPath, filename);
#else
            filePath = Path.Combine(Application.dataPath, "../" + filename);
#endif

            if (File.Exists(filePath))
            {
                var jsonContent = File.ReadAllText(filePath);
                var credentials = JsonUtility.FromJson<BasicAuthCredentials>(jsonContent);

                _lrs.credentials = credentials;

                Debug.Log($"Loaded '{filename}' successfully.");
            }
            else
            {
                Debug.LogError($"Cannot find '{filename}' in path '{filePath}'.");
            }
        }
    }

   
}