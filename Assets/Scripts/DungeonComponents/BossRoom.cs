using App.Systems;
using App.World.Creatures.Enemies;
using App.World.WorldObjects;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.DungeonComponents
{
    public class BossRoom : MonoBehaviour
    {
        [SerializeField]
        private List<BossData> bosses;
        [SerializeField]
        private Portal portal;
        private ObjectPool objectPool;
        
        public List<BossData> Bosses { get => bosses; }


        public void SpawnPortal(Vector3 position)
        {
            objectPool = FindObjectOfType<ObjectPool>();

            Debug.Log("Spawn Portal");
            Debug.Log(portal);
            Debug.Log("Obj"+portal.gameObject);
            Debug.Log(position);
            var type = portal.PoolObjectType;
            var go = portal.gameObject;
            IObjectPoolItem item = objectPool.GetObjectFromPool(type, go, position);
            GameObject portalObject = item.GetGameObject();
            Debug.Log("Portal Object" + portalObject);
            portalObject.transform.position = position;
        }
    }
}
