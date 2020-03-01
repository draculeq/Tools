using UnityEngine;
using UnityEngine.Events;

namespace Deadbit.Tools
{
    public class MonoBehaviourEvents : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] bool active = true;

        [SerializeField] private UnityEvent onAwoken;
        [SerializeField] private UnityEvent onEnabled;
        [SerializeField] private UnityEvent onStarted;
        [SerializeField] private UnityEvent onDisabled;
        [SerializeField] private UnityEvent onDestroyed;
#pragma warning restore 649

        private void Awake()
        {
            if (active) onAwoken.Invoke();
        }

        private void OnEnable()
        {
            if (active) onEnabled.Invoke();
        }

        private void Start()
        {
            if (active) onStarted.Invoke();
        }

        private void OnDisable()
        {
            if (active) onDisabled.Invoke();
        }

        private void OnDestroy()
        {
            if (active) onDestroyed.Invoke();
        }
    }
}