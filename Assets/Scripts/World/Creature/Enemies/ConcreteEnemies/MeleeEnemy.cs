using App.Systems.Spawning;
using App.World.Creatures.Enemies.States.ConcreteStates;
using App.World.Creatures.PlayerScripts.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.Enemies
{
    public class MeleeEnemy : BaseEnemy
    {
        [SerializeField]
        private DamageZone attack;

        public DamageZone Attack => attack;

        public override void Awake()
        {
            base.Awake();
            attackState = new MeleeAttackState(this, stateMachine);
        }

        public override void Init(Vector3 position, Transform target, float hpMultiplier, RoomData currentRoom, INotifyEnemyDied notifieble)
        {
            base.Init(position, FindObjectOfType<Player>().transform, hpMultiplier, currentRoom, notifieble);
            attack.Init(enemyData.damage);
        }
    }

}