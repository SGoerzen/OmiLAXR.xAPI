using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OmiLAXR.xAPI.Actors;
using TinCan;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace OmiLAXR.xAPI
{
    public class LearningRecordStore : MonoBehaviour
    {
        private static LearningRecordStore _instance;
        /// <summary>
        /// Singleton instance of the OmiLAXR LRS Controller. Only one can exist at a time.
        /// </summary>
        public static LearningRecordStore Instance
            => _instance ??= FindObjectOfType<LearningRecordStore>();

        public Action<LrsCredentials> onStartedSending;
        
        private string tempFolder => System.IO.Path.Combine(System.IO.Path.GetTempPath(), "OmiLAXR.xAPI");
        
        [Tooltip("xAPI Base URL")]
        public string statementIdUri = "https://xapi.elearn.rwth-aachen.de/definitions/";
        
        public LrsCredentials credentials = new LrsCredentials("https://lrs.elearn.rwth-aachen.de/data/xAPI", "", "");
        private RemoteLRS _remoteLRS;
        
        public bool AreValidCredentials
            => !string.IsNullOrEmpty(credentials.endpoint);
        
        public Instructor instructor;

        #region Background Processes

        private BackgroundWorker _sendWorker;
        
        #endregion
        
        private void Start()
        {
            StartSending();
        }

        private void StartSending()
        {
            _remoteLRS = new RemoteLRS(credentials.endpoint, credentials.key, credentials.secret);
            onStartedSending?.Invoke(credentials);
        }
        
        public static bool SendTo(ILRS lrs, Statement statement)
        {
            // Transfer single statement to LRS
            var resp = lrs.SaveStatement(statement);

            if (!resp.success)
            {
                // Print error message to know more about on debug
                DebugLog.xAPI.Error($"Error Message: {resp.errMsg}, Http Error: {resp.httpException}");
            }

            return resp.success;
        }

        public static bool SendTo(ILRS lrs, IEnumerable<Statement> statements)
        {
            var resp = lrs.SaveStatements(statements.ToList());
            if (!resp.success)
            {
                // Print error message to know more about on debug
                Debug.LogError($"Error Message: {resp.errMsg}, Http Error: {resp.httpException}");
            }
            return resp.success;
        }
        /// <summary>
        /// Transfer a xAPI statement to the LRS. If transfer was not successful, save xAPI locally.
        /// </summary>
        /// <param name="lrs"></param>
        /// <param name="statement"></param>
        /// <param name="pathLocalStorage"></param>
        public TransferCode SendStatement(Statement statement)
        {
            if (AreValidCredentials)
                return TransferCode.UnknownLrs;
            return SendTo(_remoteLRS, statement) ? TransferCode.Success : TransferCode.Error;
        }

        public TransferCode SendStatements(IEnumerable<Statement> statements)
        {
            if (AreValidCredentials)
                return TransferCode.UnknownLrs;
            return SendTo(_remoteLRS, statements) ? TransferCode.Success : TransferCode.Error;
        }

        /// <summary>
        /// Transfer multiple statements to LRS.
        /// </summary>
        /// <param name="lrs"></param>
        /// <param name="statements"></param>
        public static Dictionary<Statement, bool> TransferStatements(ILRS lrs, IEnumerable<Statement> statements)
        {
            var resultTransferStatements = new Dictionary<Statement, bool>();
            foreach (var statement in statements)
            {
                //transfer single statement
                var response = lrs.SaveStatement(statement);
                //successful transfer: add statement with boolean true to returning dictionary
                resultTransferStatements.Add(statement, response.success);
                //no successful transfer: add statement with boolean false to returning dictionary
            }

            return resultTransferStatements;
        }
    }
}
