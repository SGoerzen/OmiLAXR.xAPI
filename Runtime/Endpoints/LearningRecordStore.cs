using System.Collections.Generic;
using OmiLAXR.Composers;
using OmiLAXR.Endpoints;
using OmiLAXR.xAPI.Composers;
using OmiLAXR.xAPI.Extensions;
using TinCan;
using UnityEngine;

namespace OmiLAXR.xAPI.Endpoints
{
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Learning Record Store (LRS for xAPI)")]
    public class LearningRecordStore : BasicAuthEndpoint
    {
        [Tooltip("xAPI Base URL")]
        public string statementIdUri = "https://xapi.elearn.rwth-aachen.de/definitions/";
        
        private RemoteLRS _remoteLrs;
        
        protected override void OnEnable()
        {
            _remoteLrs = new RemoteLRS(credentials.endpoint, credentials.username, credentials.password);
            base.OnEnable();
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

        protected override TransferCode HandleSending(IStatement statement)
        {
            var stmt = statement as xApiStatement;

            var tinCanStatement = stmt.ToTinCanStatement(statementIdUri);

            // Transfer single statement to LRS
            var resp = _remoteLrs.SaveStatement(tinCanStatement);
            
            if (resp.success) 
                return TransferCode.Success;
            
            // Print error message to know more about on debug
            DebugLog.xAPI.Error($"Error Message: {resp.errMsg}, Http Error: {resp.httpException}");
            return TransferCode.Error;
        }
    }
}
