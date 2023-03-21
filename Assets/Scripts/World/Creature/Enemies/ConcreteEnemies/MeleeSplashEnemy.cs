using App.Systems.Spawning;
using App.World.Creatures.Enemies.States.ConcreteStates;
using App.World.Creatures.PlayerScripts.Components;
using UnityEngine;

namespace App.World.Creatures.Enemies
{
    public class MeleeSplashEnemy : BaseEnemy
    {
        [SerializeField]
        private DamageZone attack;

        [SerializeField]
        private GameObject dangerZone;
        
        public DamageZone Attack => attack;

        public GameObject DangerZone { get => dangerZone; set => dangerZone = value; }

        public override void Awake()
        {
            base.Awake();
            attackState = new MeleeSplashAttackState(this, stateMachine);
        }

        public override void Init(Vector3 position, Transform target, float hpMultiplier, Room currentRoom, INotifyEnemyDied notifieble)
        {
            base.Init(position, FindObjectOfType<Player>().transform, hpMultiplier, currentRoom, notifieble);
            attack.Init(enemyData.damage);
        }
    }

}
