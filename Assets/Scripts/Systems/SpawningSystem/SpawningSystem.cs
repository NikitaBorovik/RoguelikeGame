using App.World.Creatures.Enemies;
using App.World.Creatures.PlayerScripts.Components;
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
            if (currentWaveNumber < 3 && enemiesAliveCount <= 0)
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

        public void Spawn()
        {
            SpawnWave(currentWaveNumber);
        }
        
        public void SpawnWave(int waveNumber)
        {
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
            Debug.Log("HERE");
            Debug.Log("Alive"+ enemiesAliveCount);
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
