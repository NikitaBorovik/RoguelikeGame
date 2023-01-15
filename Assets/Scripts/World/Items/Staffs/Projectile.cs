using App.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Staffs
{
    public class Projectile : MonoBehaviour, IObjectPoolItem
    {
        protected float damage;
        protected float pearcingCount;
        protected ObjectPool objectPool;
        [SerializeField]
        protected string poolObjectType;
        public string PoolObjectType => poolObjectType;

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            

        }
        public virtual void Init(float damage, int pearcingCount)
        {
            this.damage = damage;
            this.pearcingCount = pearcingCount;
            GetComponent<TimeToLive>().Init();
        }

        public void GetFromPool(ObjectPool pool)
        {
            objectPool = pool;
            gameObject.SetActive(true);
        }

        public void ReturnToPool()
        {
            gameObject.SetActive(false);
        }

        public GameObject GetGameObject()
        {
            return (gameObject);
        }
    }
}

