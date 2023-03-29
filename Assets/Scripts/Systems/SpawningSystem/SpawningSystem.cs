using App.World.Creatures.Enemies;
using App.World.Creatures.PlayerScripts.Components;
using App.World.DungeonComponents;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace App.Systems.Spawning
{
    public class SpawningSystem : MonoBehaviour, INotifyEnemyDied
    {
        private ObjectPool objectPool;
        private Room currentRoom;
        private Player player;
        private int enemiesAliveCount;
        private INotifyRoomCleared notifieble;
        private int currentWaveNumber = 1;
        private BossRoom bossRoom;

        public Room CurrentRoom { get => currentRoom; set => currentRoom = value; }
        public INotifyRoomCleared Notifieble { get => notifieble; set => notifieble = value; }

        public void Init(ObjectPool objectPool, Player player)
        {
            this.objectPool = objectPool;
            this.player = player;
        }

        public void NotifyEnemyDied()
        {
            enemiesAliveCount--;
            HandleEnemiesDied();
        }
        private void HandleEnemiesDied()
        {
            if (currentRoom.RoomNodeType.isBoss && enemiesAliveCount <= 0)
            {
                notifieble.NotifyRoomCleared();
                bossRoom.SpawnPortal(currentRoom.DrawnRoom.Grid.CellToWorld(new Vector3Int(currentRoom.RoomModel.enemySpawns[0].x, currentRoom.RoomModel.enemySpawns[0].y,0)));
            }
            else if (currentWaveNumber < 3 && enemiesAliveCount <= 0)
            {
                currentWaveNumber++;
                SpawnWave(currentWaveNumber);
            }
            else if (currentWaveNumber >= 3 && enemiesAliveCount <= 0)
            {
                currentWaveNumber = 1;
                notifieble.NotifyRoomCleared();
            }
        }

        public void Spawn(int level)
        {
            if (currentRoom.RoomNodeType.isBoss)
            {
                SpawnBoss(level);
            }
            else
            {
                SpawnWave(currentWaveNumber);
            }
            
        }

        private void SpawnBoss(int level)
        {
            List<Vector2Int> spawns = new List<Vector2Int>(CurrentRoom.RoomModel.enemySpawns);
            List<BaseEnemy> bossesToSpawn;
            List<BaseEnemy> minionsToSpawn;
            bossRoom = CurrentRoom.Prefab.GetComponent<BossRoom>();
            bossesToSpawn = bossRoom.Bosses[level].bosses;
            minionsToSpawn = bossRoom.Bosses[level].minions;
            enemiesAliveCount = bossesToSpawn.Count + minionsToSpawn.Count;
            if (enemiesAliveCount == 0)
            {
                HandleEnemiesDied();
                return;
            }
            for (int i = 0; i < bossesToSpawn.Count; i++)
            {
                if (bossesToSpawn[i] == null)
                {
                    Debug.Log("Error, trying to create enemy, but gameobject doesn't contain BaseEnemy script");
                    return;
                }
                Vector2 pos = currentRoom.DrawnRoom.Grid.CellToWorld((Vector3Int)spawns[i % spawns.Count]);
                BaseEnemy enemy = objectPool.GetObjectFromPool(bossesToSpawn[i].PoolObjectType, bossesToSpawn[i].gameObject, pos).GetGameObject().GetComponent<BaseEnemy>();
                enemy.Init(pos, player.transform, 1, currentRoom, this);
            }
            for (int i = 0; i < minionsToSpawn.Count; i++)
            {
                if (minionsToSpawn[i] == null)
                {
                    Debug.Log("Error, trying to create enemy, but gameobject doesn't contain BaseEnemy script");
                    return;
                }
                Vector2 pos = currentRoom.DrawnRoom.Grid.CellToWorld((Vector3Int)spawns[i % spawns.Count]);
                BaseEnemy enemy = objectPool.GetObjectFromPool(minionsToSpawn[i].PoolObjectType, minionsToSpawn[i].gameObject, pos).GetGameObject().GetComponent<BaseEnemy>();
                enemy.Init(pos, player.transform, 1, currentRoom, this);
            }
        }

        public void SpawnWave(int waveNumber)
        {
            if (currentRoom.RoomModel.enemiesWave1.Count == 0)
            {
                Debug.Log("No enemies in room");
                notifieble.NotifyRoomCleared();
                return;
            }
            List<Vector2Int> spawns = new List<Vector2Int>(CurrentRoom.RoomModel.enemySpawns);
            Extensions.Shuffle(spawns);
            List<BaseEnemy> enemiesToSpawn;
            switch (waveNumber)
            {
                case 1:
                    enemiesToSpawn = new List<BaseEnemy>(currentRoom.RoomModel.enemiesWave1);
                    break;
                case 2:
                    enemiesToSpawn = new List<BaseEnemy>(currentRoom.RoomModel.enemiesWave2);
                    break;
                case 3:
                    enemiesToSpawn = new List<BaseEnemy>(currentRoom.RoomModel.enemiesWave3);
                    break;
                default:
                    return;
            }
            enemiesAliveCount = enemiesToSpawn.Count;
            if (enemiesAliveCount == 0)
            {
                HandleEnemiesDied();
                return;
            }
            for (int i = 0; i < enemiesToSpawn.Count; i++)
            {
                if (enemiesToSpawn[i] == null)
                {
                    Debug.Log("Error, trying to create enemy, but gameobject doesn't contain BaseEnemy script");
                    return;
                }
                Vector2 pos = currentRoom.DrawnRoom.Grid.CellToWorld((Vector3Int)spawns[i % spawns.Count]);
                BaseEnemy enemy = objectPool.GetObjectFromPool(enemiesToSpawn[i].PoolObjectType, enemiesToSpawn[i].gameObject, pos).GetGameObject().GetComponent<BaseEnemy>();
                enemy.Init(pos, player.transform, 1, currentRoom, this);
            }
        }
    }

}
