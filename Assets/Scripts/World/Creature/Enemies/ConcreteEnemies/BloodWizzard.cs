using App.Systems;
using App.Systems.Spawning;
using App.World.Creatures.Enemies.States.ConcreteStates;
using App.World.Creatures.PlayerScripts.Components;
using App.World.Items.Attacks;
using UnityEngine;

namespace App.World.Creatures.Enemies
{
    public class BloodWizzard : BaseEnemy
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
            attackState = new BloodWizzardAttackState(this, stateMachine, FindObjectOfType<ObjectPool>());
        }

        public override void Init(Vector3 position, Transform target, float hpMultiplier, RoomData currentRoom, INotifyEnemyDied notifieble)
        {
            base.Init(position, FindObjectOfType<Player>().transform, hpMultiplier, currentRoom, notifieble);
        }
    }
}
