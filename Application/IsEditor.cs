using UnityEngine;
using UnityEngine.Events;

namespace Deadbit.Tools
{
    public class IsEditor : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private UnityEvent onAwoken;
        [SerializeField] private UnityEvent onEnabled;
        [SerializeField] private UnityEvent onStarted;
        [SerializeField] private UnityEvent onDisabled;
        [SerializeField] private UnityEvent onDestroyed;
#pragma warning restore 649

        public bool Check()
        {
            return Application.isEditor;
        }

        private void Awake()
        {
            if (Check()) onAwoken.Invoke();
        }

        private void OnEnable()
        {
            if (Check()) onEnabled.Invoke();
        }

        private void Start()
        {
            if (Check()) onStarted.Invoke();
        }

        private void OnDisable()
        {
            if (Check()) onDisabled.Invoke();
        }

        private void OnDestroy()
        {
            if (Check()) onDestroyed.Invoke();
        }
    }
}
