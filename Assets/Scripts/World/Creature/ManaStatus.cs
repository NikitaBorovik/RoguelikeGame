using App.UI;
using UnityEngine;

namespace App.World.Creatures
{
    [DisallowMultipleComponent]
    public class ManaStatus : MonoBehaviour
    {
        private float currentMana;
        private float maxMana;
        private float timeCounter = 0.0f;
        private const float period = 1.0f;
        private float manaRegenRate = 1f;
        [SerializeField]
        private ValueUpdateEvent manaUpdateEvent;
        [SerializeField]
        private string manaUpdateEventName;

        public float CurrentMana
        {
            get => currentMana;
            private set
            {
                var prev = currentMana;

                if (value > MaxMana)
                    currentMana = MaxMana;
                else if (value < 0)
                    currentMana = 0;
                else
                    currentMana = value;
                manaUpdateEvent?.CallValueUpdateEvent(prev, currentMana, maxMana, manaUpdateEventName);
            }
        }

        public float MaxMana
        {
            get => maxMana;
            set
            {
                if (MaxMana < 0f)
                {
                    MaxMana = 0f;
                    return;
                }

                var prevMaxMana = maxMana;
                maxMana = value;

                if (maxMana > prevMaxMana)
                {
                    CurrentMana += maxMana - prevMaxMana;
                }
                else if (maxMana < CurrentMana)
                {
                    CurrentMana = maxMana;
                }
            }
        }

        public float ManaRegenRate { get => manaRegenRate; set => manaRegenRate = value; }

        public void Awake()
        {
            CurrentMana = MaxMana;
        }

        private void Update()
        {
            if (timeCounter > period)
            {
                Restore(ManaRegenRate);
                timeCounter = 0f;
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }   

        public void SpendMana(float amount)
        {
            CurrentMana -= amount;
        }

        public void Restore(float amount)
        {
            CurrentMana += amount;
        }

        public void RestoreToMax()
        {
            CurrentMana = MaxMana;
        }

        public void ChangeMaxMana(float amount)
        {
            MaxMana += amount;
        }
    }

}