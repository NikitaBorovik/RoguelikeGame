using App.UI.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] 
        private Transform progressTransform;
        [SerializeField] 
        private ValueUpdateEvent valueUpdateEvent;
        [Range(0f, 1f)]
        [SerializeField] 
        private float currentPercentage;
        [SerializeField]
        private string valueUpdateEventName;

        public float CurrentPercentage
        {
            get => currentPercentage;
            set
            {
                if (value < -Mathf.Epsilon || value > 1 + Mathf.Epsilon)
                    throw new System.InvalidOperationException("Percentage ranges between 0 and 1.");
                currentPercentage = value;
                SetGUIPercentage(value);
            }
        }

        private void OnEnable()
        {
            valueUpdateEvent.OnValueUpdate += OnValueUpdate;
        }

        private void OnDisable()
        {
            valueUpdateEvent.OnValueUpdate -= OnValueUpdate;
        }

        private void SetGUIPercentage(float percentage)
        {
            var prev = progressTransform.localScale;
            
            if (percentage >= 0f && percentage <= 1f)
            progressTransform.localScale = new(percentage, prev.y, prev.z);
        }

        private void OnValueUpdate(ValueUpdateEvent ev, ValueUpdateEventArgs args)
        {
            if(args.valueUpdateEventName == valueUpdateEventName)
            CurrentPercentage = args.newValue >= 0f ? args.newValue / args.maxValue : 0f;
        }
    }
}
