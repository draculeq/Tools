using Deadbit.Events;
using Deadbit.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Deadbit.Tools
{
    public class OpenUrl : SerializedMonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private IGenericValue<string> url;
        [SerializeField] private StringEvent urlOpened;
#pragma warning restore 649

        public void Open()
        {
            Open(url.Value);
        }

        public void Open(string url)
        {
            Application.OpenURL(url);
            urlOpened?.Invoke(url);
        }
    }
}
