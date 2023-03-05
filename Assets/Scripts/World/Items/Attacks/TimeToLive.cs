using App.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Attacks
{
    public class TimeToLive : MonoBehaviour
    {
        [SerializeField]
        private float timeToLive;
        private ObjectPool objectPool;
        private float timeToLiveLeft;
        private IObjectPoolItem objectPoolItem;
        public void Init()
        {
            objectPool = FindObjectOfType<ObjectPool>();
            objectPoolItem = GetComponent<IObjectPoolItem>();
            if (objectPoolItem == null)
                Destroy(gameObject, timeToLive);
            else
                timeToLiveLeft = timeToLive;
        }
        void Update()
        {
            if (objectPoolItem != null)
            {
                timeToLiveLeft -= Time.deltaTime;
                if (timeToLiveLeft <= 0)
                    objectPool.ReturnToPool(objectPoolItem);
            }


        }

        private IEnumerator destroyObjectPoolItem(float delay, IObjectPoolItem item)
        {
            yield return new WaitForSeconds(delay);
            objectPool.ReturnToPool(item);
        }
    }
}

