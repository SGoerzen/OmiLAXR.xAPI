using System.Collections.Generic;
using System.Linq;
using TinCan;
using UnityEngine;

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

        private static bool SendToLrs(ILRS lrs, Statement statement)
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

        private static bool SendToLrs(ILRS lrs, IEnumerable<Statement> statements)
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
        public static TransferCode SendStatement(RemoteLRS lrs, Statement statement)
        {
            if (lrs == null)
                return TransferCode.UnknownLrs;
            return SendToLrs(lrs, statement) ? TransferCode.Success : TransferCode.Error;
        }

        public static TransferCode SendStatements(RemoteLRS lrs, IEnumerable<Statement> statements)
        {
            if (lrs == null)
                return TransferCode.UnknownLrs;
            return SendToLrs(lrs, statements) ? TransferCode.Success : TransferCode.Error;
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
