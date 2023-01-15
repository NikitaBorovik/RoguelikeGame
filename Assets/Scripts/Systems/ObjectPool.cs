using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Systems
{
        public class ObjectPool : MonoBehaviour
        {
            Dictionary<string, Queue<IObjectPoolItem>> appObjectPool;

            private void OnEnable()
            {
                appObjectPool = new Dictionary<string, Queue<IObjectPoolItem>>();
            }

            public IObjectPoolItem GetObjectFromPool(string objectType, GameObject objectToInstantiate, Vector3 pos, Transform parent = null)
            {
                IObjectPoolItem result = null;
                if (!appObjectPool.ContainsKey(objectType))
                {
                    Queue<IObjectPoolItem> newObjectsQueue = new Queue<IObjectPoolItem>();
                    appObjectPool.Add(objectType, newObjectsQueue);
                    result = CreateObject(objectToInstantiate, pos, parent);
                    result.GetFromPool(this);
                    return result;
                }
                if (appObjectPool[objectType].Count == 0)
                {
                    result = CreateObject(objectToInstantiate, pos, parent);
                    result.GetFromPool(this);
                    return result;
                }
                result = appObjectPool[objectType].Dequeue();
                result.GetFromPool(this);
                return result;
            }

            private IObjectPoolItem CreateObject(GameObject objectToInstantiate, Vector3 pos, Transform parent = null)
            {
                GameObject createdObject;
                if (parent == null)
                {
                    createdObject = Instantiate(objectToInstantiate, pos, Quaternion.identity);
                }
                else
                {
                    createdObject = Instantiate(objectToInstantiate, parent, false);
                }

                IObjectPoolItem poolableData = createdObject.GetComponent<IObjectPoolItem>();
                if (poolableData == null)
                {
                    Debug.LogError("Invalid cast to interface IObjectPoolItem");
                }

                return poolableData;
            }

            public void ReturnToPool(IObjectPoolItem objectReturnedToPool)
            {
                if (appObjectPool != null)
                {
                    if (appObjectPool.ContainsKey(objectReturnedToPool.PoolObjectType))
                    {
                        appObjectPool[objectReturnedToPool.PoolObjectType].Enqueue(objectReturnedToPool);
                    }
                    else
                    {
                        Queue<IObjectPoolItem> newObjectsQueue = new Queue<IObjectPoolItem>();
                        newObjectsQueue.Enqueue(objectReturnedToPool);
                        appObjectPool.Add(objectReturnedToPool.PoolObjectType, newObjectsQueue);
                    }
                }
                objectReturnedToPool.ReturnToPool();
            }
        }

}
