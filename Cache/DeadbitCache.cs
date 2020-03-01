using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Deadbit.Utils.Extensions;
using UnityEngine;
using UnityEngine.Networking;

namespace Deadbit.Tools
{
    public class DeadbitCache
    {
        public interface ITextureResultListener
        {
            int Id { get; }
        }
        public class GetTextureResult
        {
            public bool IsCompleted { internal set; get; } = false;
            public bool IsSuccess { internal set; get; } = false;
            public string Error { internal set; get; } = string.Empty;
            public Texture2D Texture { internal set; get; } = null;

        }

        public class TaskRequest
        {
            public Task<GetTextureResult> Task { get; private set; }

            public List<int> Listeners { get; } = new List<int>();

            public TaskRequest(Task<GetTextureResult> task, ITextureResultListener listener)
            {
                Task = task;
                AddListener(listener);
            }

            public void AddListener(ITextureResultListener listener)
            {
                Listeners.Add(listener.Id);
            }

            public void RemoveListener(ITextureResultListener textureDownloader)
            {
                Listeners.Remove(textureDownloader.Id);
            }
        }
        private static string path = UnityEngine.Application.temporaryCachePath + "/Cache";

        private static Dictionary<string, TaskRequest> pendingRequests = new Dictionary<string, TaskRequest>();

        public static async Task<TaskRequest> GetTexture(string url, ITextureResultListener listener)
        {
            var filePath = path + "/" + url.GetHashCode();
            if (pendingRequests.ContainsKey(filePath)) //Return pending task if any in progress
            {
                var rq = pendingRequests[filePath];
                rq.AddListener(listener);
                await rq.Task;
                return rq;
            }

            var request = new TaskRequest(GetTextureTask(url, filePath), listener);
            if (pendingRequests.ContainsKey(filePath))
                return null;
            pendingRequests.Add(filePath, request);
            await request.Task;
            pendingRequests.Remove(filePath);
            return request;
        }

        private static async Task<GetTextureResult> GetTextureTask(string url, string filePath)
        {
            var tex = new Texture2D(16, 16, TextureFormat.ARGB32, false);
            var result = new GetTextureResult();

            if (Directory.Exists(path) && File.Exists(filePath))
            {
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + filePath))
                {
                    await www.SendWebRequest();
                    if (www.isNetworkError)
                    {
                        result.IsCompleted = true;
                        result.IsSuccess = false;
                        Debug.LogError(www.error);
                    }
                    else
                    {
                        tex = DownloadHandlerTexture.GetContent(www);
                        result.IsCompleted = true;
                        result.IsSuccess = true;
                        result.Texture = tex;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    Debug.LogError("Empty Url");
                    result.IsCompleted = true;
                    result.IsSuccess = false;
                    result.Error = "Empty Url";
                    return result;
                }

                var webRequest = UnityWebRequestTexture.GetTexture(new Uri(url));
                await webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.LogError($"www.error: {webRequest.error}, www.isNetError: {webRequest.isNetworkError}, www.isHttpError: {webRequest.isHttpError}");
                    result.IsCompleted = true;
                    result.IsSuccess = false;
                    result.Error = webRequest.error;
                }
                else if (tex == null) // Unity play mode stopped
                {
                    result.IsCompleted = true;
                    result.IsSuccess = false;
                    result.Error = "Unity stopped working";
                }
                else
                {
                    AddToCache(url, webRequest.downloadHandler.data);
                    tex.LoadImage(webRequest.downloadHandler.data);
                    tex.Apply();
                    result.IsCompleted = true;
                    result.IsSuccess = true;
                    result.Texture = tex;
                }
            }
            return result;
        }

        private static void AddToCache(string url, byte[] data)
        {
            var filePath = path + "/" + url.GetHashCode();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllBytes(filePath, data);
        }
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Deadbit/ClearCache")]
#endif
        public static void ClearCache()
        {
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                    File.Delete(file);
            }
        }
    }
}
