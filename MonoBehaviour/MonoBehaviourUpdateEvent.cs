﻿using Deadbit.Variables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Deadbit.Tools
{
    public class MonoBehaviourUpdateEvent : SerializedMonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private UnityEvent onUpdated;
        [SerializeField] private IGenericValue<float> interval;
#pragma warning disable 649

        private float elapsed;

        void Update()
        {
            elapsed += Time.deltaTime;
            if (elapsed > interval.Value)
            {
                elapsed = 0;
                onUpdated.Invoke();
            }
        }
    }
}