using App.Systems.Spawning;
using App.World.Creatures.Enemies.States.ConcreteStates;
using App.World.Creatures.PlayerScripts.Components;
using UnityEngine;
using App.Systems;

namespace App.World.Creatures.Enemies
{
    public class DevilEye : BaseEnemy
    {
        [SerializeField]
        private EnemyProjectile projectile;
        [SerializeField]
        private Transform shootPosition;

        public EnemyProjectile Projectile
        {
            get => projectile; set => projectile = value;
        }

        public Transform ShootPosition
        {
            get => shootPosition; set => shootPosition = value;
        }

        public override void Awake()
        {
            base.Awake();
            attackState = new DevilEyeAttackState(this, stateMachine, FindObjectOfType<ObjectPool>());
        }

        public override void Init(Vector3 position, Transform target, float hpMultiplier, Room currentRoom, INotifyEnemyDied notifieble)
        {
            base.Init(position, FindObjectOfType<Player>().transform, hpMultiplier, currentRoom, notifieble);
        }
    }
}
