/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using OmiLAXR.Composers;
using OmiLAXR.xAPI.Composers;
using OmiLAXR.xAPI.Extensions;
using TinCan;
using UnityEngine;
using UnityEngine.Networking;

namespace OmiLAXR.xAPI.Endpoints
{
    #if UNITY_WEBGL

    /// <summary>
    /// WebGL-specific implementation of the LRS client wrapper for sending xAPI statements.
    /// Handles the unique requirements of WebGL builds by using Unity's UnityWebRequest
    /// instead of standard HTTP libraries, ensuring compatibility with browser security constraints.
    /// </summary>
    public class WebGLLRSClientWrapper : ILRSClient
    {
        /// <summary>
        /// The base endpoint URL for the Learning Record Store, with trailing slashes removed for consistency.
        /// </summary>
        private readonly string endpoint;
        
        /// <summary>
        /// Base64-encoded HTTP Basic Authentication header for LRS authentication.
        /// </summary>
        private readonly string authHeader;
        
        /// <summary>
        /// MonoBehaviour instance used to run coroutines for asynchronous HTTP requests.
        /// Required because static methods cannot start coroutines in Unity.
        /// </summary>
        private readonly MonoBehaviour coroutineRunner;
        
        /// <summary>
        /// The xAPI specification version to use for statement formatting and communication.
        /// </summary>
        private readonly TCAPIVersion version = TCAPIVersion.V103;

        /// <summary>
        /// Initializes a new WebGLLRSClientWrapper with LRS connection parameters.
        /// Configures authentication and endpoint settings for WebGL-compatible HTTP communication.
        /// </summary>
        /// <param name="endpoint">The base URL of the Learning Record Store endpoint</param>
        /// <param name="username">Username for HTTP Basic Authentication</param>
        /// <param name="password">Password for HTTP Basic Authentication</param>
        /// <param name="runner">MonoBehaviour instance to handle coroutine execution</param>
        public WebGLLRSClientWrapper(string endpoint, string username, string password, MonoBehaviour runner)
        {
            this.endpoint = endpoint.TrimEnd('/'); // Ensure clean endpoint URL
            // Create Base64-encoded authentication header
            authHeader = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            coroutineRunner = runner;
        }

        /// <summary>
        /// Sends a batch of xAPI statements to the LRS using WebGL-compatible HTTP requests.
        /// Starts a coroutine to handle the asynchronous operation due to WebGL threading limitations.
        /// </summary>
        /// <param name="statements">List of IStatement instances to send (expected to be xApiStatement)</param>
        /// <param name="callback">Action to invoke with the result of the operation</param>
        public void SendStatements(List<IStatement> statements, Action<bool> callback)
        {
            coroutineRunner.StartCoroutine(SendStatementsCoroutine(statements, callback));
        }

        /// <summary>
        /// Coroutine that handles the actual HTTP communication with the LRS for WebGL builds.
        /// Converts statements to JSON, sends via UnityWebRequest, and processes the response
        /// including updating statement IDs with those returned by the LRS.
        /// </summary>
        /// <param name="statements">List of statements to send</param>
        /// <param name="callback">Callback to invoke with the operation result</param>
        /// <returns>IEnumerator for Unity coroutine system</returns>
        private IEnumerator SendStatementsCoroutine(List<IStatement> statements, Action<bool> callback)
        {
            // Convert OmiLAXR xAPI statements to TinCan format
            var tinCanStatements = statements.Select(s => (s as xApiStatement).ToTinCanStatement()).ToList();

            // Build JSON array of statements for HTTP transmission
            var jArray = new JArray();
            foreach (var statement in tinCanStatements)
            {
                jArray.Add(statement.ToJObject(version));
            }

            // Prepare HTTP POST request with JSON payload
            var jsonPayload = jArray.ToString();
            var request = new UnityWebRequest(endpoint + "/statements", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            // Set required HTTP headers for xAPI communication
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("X-Experience-API-Version", version.ToString());
            request.SetRequestHeader("Authorization", authHeader);
            
            // Send the request and wait for response
            yield return request.SendWebRequest();

            // Handle HTTP request failures
            if (request.result != UnityWebRequest.Result.Success)
            {
                DebugLog.xAPI.Error($"WebGL LRS Transfer Failed: {request.error}");
                callback(false);
                yield break;
            }

            try
            {
                // Parse the LRS response to extract statement IDs
                var responseArray = JArray.Parse(request.downloadHandler.text);
                for (var i = 0; i < responseArray.Count; i++)
                {
                    // Update statement IDs with those assigned by the LRS
                    tinCanStatements[i].id = new Guid((string)responseArray[i]);
                }

                callback(true); // Report successful transmission
            }
            catch (Exception ex)
            {
                // Handle JSON parsing or other processing errors
                DebugLog.xAPI.Error($"Failed to parse LRS response: {ex.Message}");
                callback(false);
            }
        }
    }

    #endif
}