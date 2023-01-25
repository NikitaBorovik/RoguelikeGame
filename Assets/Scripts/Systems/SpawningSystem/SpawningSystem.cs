using App.World.Creatures.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Systems.Spawning
{
    public class SpawningSystem : MonoBehaviour
    {
        private ObjectPool objectPool;
        private Room currentRoom;

        public Room CurrentRoom { get => currentRoom; set => currentRoom = value; }

        public void Init(ObjectPool objectPool)
        {
            this.objectPool = objectPool;
        }
        
        public void Spawn()
        {
            Debug.Log(CurrentRoom.RoomNodeType);
            BaseEnemy baseEnemy = currentRoom.Enemies[Random.Range(0, currentRoom.Enemies.Count)];
            if (baseEnemy == null)
            {
                Debug.Log("Error, trying to create enemy, but gameobject doesn't contain BaseEnemy script");
                return;
            }
            Vector2 pos = currentRoom.DrawnRoom.Grid.CellToWorld((Vector3Int)currentRoom.EnemySpawns[Random.Range(0, currentRoom.EnemySpawns.Length)]);
            baseEnemy = objectPool.GetObjectFromPool(baseEnemy.PoolObjectType, baseEnemy.gameObject, pos).GetGameObject().GetComponent<BaseEnemy>();
            if (baseEnemy == null)
            {
                Debug.Log("Error, took enemy out of object pool, but didn't find BaseEnemy script on it");
                return;
            }
            Debug.Log(pos);
            baseEnemy.Init(pos, baseEnemy.transform, 1);
        }
    }

}
