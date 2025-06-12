#if XAPI_REGISTRY_EXISTS
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OmiLAXR.Composers;
using OmiLAXR.Endpoints;
using OmiLAXR.xAPI.Composers;
using OmiLAXR.xAPI.Extensions;
using TinCan;
using UnityEngine;

namespace OmiLAXR.xAPI.Endpoints
{
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Learning Record Store (LRS for xAPI)")]
    [Description("Sends all received statements a Learning Record Store asynchronously.")]
    public class LearningRecordStore : BasicAuthEndpoint
    {
        private RemoteLRS _remoteLrs;

        protected override int MaxBatchSize => 20;
        
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

        public override void StartSending(bool resetQueue = false)
        {
            var credentials = Credentials;
            _remoteLrs = new RemoteLRS(credentials.endpoint, credentials.username, credentials.password);
            base.StartSending(resetQueue);
        }

        protected override TransferCode HandleSending(List<IStatement> batch)
        {
            if (batch.Count == 0)
                return TransferCode.NoStatements;
            
            var stmts = batch.Select(s => (s as xApiStatement).ToTinCanStatement()).ToList();

            var resp = _remoteLrs.SaveStatements(stmts);

            if (!resp.success)
            {
                DebugLog.xAPI.Error($"Error Message: {resp.errMsg}, Http Error: {resp.httpException}");
                foreach (var statement in batch)
                {
                    TriggerFailedStatement(statement);
                    QueuedStatements.Enqueue(statement);
                }
                TriggerFailedBatch(batch);
                return TransferCode.Error;
            }
            
            foreach (var statement in batch)
                TriggerSentStatement(statement);

            return TransferCode.Success;
        }

        protected override TransferCode HandleSending(IStatement stmt)
        {
            var statement = stmt as xApiStatement;
            var tinCanStatement = statement.ToTinCanStatement();
            
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
#endif