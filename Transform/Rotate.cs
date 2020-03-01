using Deadbit.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Deadbit.Tools
{
    public class Rotate : SerializedMonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private IGenericValue<Vector3> speed;
        [SerializeField] private IGenericValue<bool> useInterval;
        [SerializeField, ShowIf("isUsingInterval")] private IGenericValue<float> interval;
#pragma warning restore 649

        private bool IsUsingInterval => useInterval?.Value ?? false;

        private Quaternion initialValue;
        private float elapsed;

        void Awake()
        {
            initialValue = transform.rotation;
            if (IsUsingInterval) elapsed = 0;
        }

        void Update()
        {
            if (IsUsingInterval)
            {
                elapsed += Time.deltaTime;
                if (elapsed > interval.Value)
                {
                    transform.Rotate(speed.Value);
                    elapsed -= interval.Value;
                }
            }
            else
            {
                transform.Rotate(speed.Value * Time.deltaTime);
            }
        }

        public void Reset()
        {
            transform.rotation = initialValue;
        }
    }
}