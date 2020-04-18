using System;
using System.Linq;
using System.Threading.Tasks;
using Deadbit.Events;
using Deadbit.Variables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Deadbit.Tools
{
    public class TextureDownloader : SerializedMonoBehaviour, DeadbitCache.ITextureResultListener
    {
#pragma warning disable 649
        [SerializeField] private IGenericValue<string> source;
        [SerializeField] private UnityEvent downloadStarted;
        [SerializeField] public StringEvent downloadFailed;
        [SerializeField] public TextureEvent downloadSuceed;
        [SerializeField] public string lastUrl;
        [SerializeField] public Texture2D lastTexture;
#pragma warning restore 649
        public int Id => listenerId;

        DeadbitCache.TaskRequest request;
        private int listenerId;

        [Button]
        public async void Download()
        {
            await DownloadTexture(source.Value);
        }

        [Button]
        public async void Download(string url)
        {
            await DownloadTexture(url);
        }

        public async Task<Texture2D> DownloadTexture(string url)
        {
            if (lastUrl == url && lastTexture != null)
            {
                OnSucceed(lastTexture);
                return lastTexture;
            }
            if (lastTexture != null)
            {
                Destroy(lastTexture);
            }
            listenerId = Guid.NewGuid().GetHashCode();
            if (downloadStarted != null) downloadStarted.Invoke();
            request = await DeadbitCache.GetTexture(url, this);
            if (request != null)
            {
                if (request.Listeners == null || request.Listeners.Count == 0)
                {
                    if (request.Task != null && request.Task.Result != null && request.Task.Result.Texture != null)
                    {
                        Destroy(request.Task.Result.Texture);
                    }
                }
                else if (request.Listeners.Any(a => a == Id))
                {
                    if (request.Task.Result.IsSuccess)
                    {
                        lastTexture = request.Task.Result.Texture;
                        lastUrl = url;
                        OnSucceed(request.Task.Result.Texture);
                    }
                    else
                        OnFailed(request.Task.Result.Error);
                    return request.Task.Result.Texture;
                }
            }
            return null;
        }

        void OnDisable()
        {
            if (lastTexture != null)
            {
                Destroy(lastTexture);
            }
            if (request != null) request.RemoveListener(this);
        }

        private void OnSucceed(Texture2D tex)
        {
            if (downloadSuceed != null) downloadSuceed.Invoke(tex);
        }

        private void OnFailed(string obj)
        {
            if (downloadFailed != null) downloadFailed.Invoke(obj);
        }

    }
}