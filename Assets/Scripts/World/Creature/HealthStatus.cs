using App.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures
{
    [DisallowMultipleComponent]
    public class HealthStatus : MonoBehaviour
    {
        private float currentHealth;
        private float maxHealth;
        [SerializeField]
        private float blinkTime;
        private Color32 blinkColor = new Color32(215, 110, 110, 255);
        private Dictionary<SpriteRenderer, Color> spriteRenderers;
        private List<SpriteRenderer> toDelete;
        private Coroutine blinkRoutine;
        [SerializeField]
        private ValueUpdateEvent healthUpdateEvent;
        [SerializeField]
        private string healthUpdateEventName;
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip damageSound;

        public float CurrentHealth
        {
            get => currentHealth;
            private set
            {
                var prev = currentHealth;

                if (value > MaxHealth)
                    currentHealth = MaxHealth;
                else if (value < 0)
                    currentHealth = 0;
                else
                    currentHealth = value;

                healthUpdateEvent?.CallValueUpdateEvent(prev, currentHealth, MaxHealth, healthUpdateEventName);

            }
        }

        public float MaxHealth
        {
            get => maxHealth;
            set
            {
                if (MaxHealth < 0f)
                {
                    maxHealth = 0f;
                    return;
                }

                var prevMaxHealth = maxHealth;
                maxHealth = value;

                if (maxHealth > prevMaxHealth)
                {
                    CurrentHealth += maxHealth - prevMaxHealth;
                }
                else if (maxHealth < CurrentHealth)
                {
                    CurrentHealth = maxHealth;
                }
            }
        }

        public void Awake()
        {
            CurrentHealth = MaxHealth;
            spriteRenderers = new Dictionary<SpriteRenderer, Color>();
            toDelete = new List<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderers.Add(spriteRenderer, spriteRenderer.color);
            }

        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            Blink();
            if (audioSource != null && damageSound != null)
                audioSource.PlayOneShot(damageSound);
            if (CurrentHealth <= 0)
            {
                IKillable baseScript = GetComponent<IKillable>();
                baseScript.Die();
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
        }

        public void HealToMax()
        {
            CurrentHealth = MaxHealth;
        }

        public void ChangeMaxHealth(float amount)
        {
            MaxHealth += amount;
        }

        private IEnumerator BlinkCoroutine()
        {
            if (spriteRenderers == null)
                yield break;
            foreach (SpriteRenderer spriteRenderer in spriteRenderers.Keys)
            {
                if (spriteRenderer != null)
                    spriteRenderer.color = blinkColor;
                else
                    toDelete.Add(spriteRenderer);
            }
            yield return new WaitForSeconds(blinkTime);
            foreach (SpriteRenderer spriteRenderer in spriteRenderers.Keys)
            {
                if (spriteRenderer != null)
                    spriteRenderer.color = spriteRenderers[spriteRenderer];
            }
            blinkRoutine = null;
        }
        private void Blink()
        {
            if (blinkRoutine != null)
            {
                StopCoroutine(blinkRoutine);
            }
            foreach (SpriteRenderer spriteRenderer in toDelete)
            {
                spriteRenderers.Remove(spriteRenderer);
            }
            toDelete.Clear();
            blinkRoutine = StartCoroutine(BlinkCoroutine());
        }
    }

}

