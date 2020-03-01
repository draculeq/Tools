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
        private string lastUrl;
        private Texture2D lastTexture;
#pragma warning restore 649
        public int Id { get; private set; }

        DeadbitCache.TaskRequest request;

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
            if (lastUrl == url)
            {
                OnSucceed(lastTexture);
                return lastTexture;
            }

            Id = Guid.NewGuid().GetHashCode();
            if (downloadStarted != null) downloadStarted.Invoke();
            request = await DeadbitCache.GetTexture(url, this);
            if (request != null && request.Listeners != null && request.Listeners.Any(a => a == Id))
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

            return null;
        }

        void OnDisable()
        {
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
