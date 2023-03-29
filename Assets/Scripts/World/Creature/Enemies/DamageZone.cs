using App.World.Creatures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.Enemies
{
    public class DamageZone : MonoBehaviour
    {
        private float damage;
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private List<AudioClip> hitSounds;

        private List<HealthStatus> targetsToDamage;
        
        public void Init(float damage)
        {
            this.damage = damage;
            targetsToDamage = new List<HealthStatus>();
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            HealthStatus collisionHealth = collision.GetComponentInParent<HealthStatus>();
            if (collisionHealth != null)
            {
                if (!targetsToDamage.Contains(collisionHealth))
                {
                    targetsToDamage.Add(collisionHealth);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            HealthStatus collisionHealth = collision.GetComponentInParent<HealthStatus>();
            if (collisionHealth != null)
            {
                if (targetsToDamage.Contains(collisionHealth))
                {
                    targetsToDamage.Remove(collisionHealth);
                }
            }
            else
                Debug.Log("Error: Trying to damage target without Health component");
        }

        public void DamageTargets()
        {
            foreach(HealthStatus target in targetsToDamage)
            {
                target.TakeDamage(damage);
            }
            //int index = Random.Range(0, hitSounds.Count);
            // audioSource.PlayOneShot(hitSounds[index]);
        }


    }

}
