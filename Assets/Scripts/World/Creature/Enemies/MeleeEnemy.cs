using App.Systems.Spawning;
using App.World.Creatures.Enemies;
using App.World.Creatures.PlayerScripts.Components;
using UnityEngine;

namespace Assets.Scripts.World.Creature.Enemies
{
    public class MeleeEnemy : BaseEnemy
    {
        
        public override void Awake()
        {
            base.Awake();
            //Init(transform.position,transform,1);//TODO remove
        }

        public override void Init(Vector3 position, Transform target, float hpMultiplier, Room currentRoom, INotifyEnemyDied notifieble)
        {
            base.Init(position,FindObjectOfType<Player>().transform, hpMultiplier,currentRoom, notifieble);
          
        }
    }
}