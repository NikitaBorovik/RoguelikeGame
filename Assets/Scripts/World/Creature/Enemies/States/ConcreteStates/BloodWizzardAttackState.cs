using App;
using App.Systems;
using App.World.Creatures.Enemies;
using App.World.Creatures.Enemies.States;
using App.World.Creatures.PlayerScripts.Components;
using App.World.Items.Attacks;
using System.Collections;
using UnityEngine;

namespace App.World.Creatures.Enemies.States.ConcreteStates
{
    public class BloodWizzardAttackState : EnemyBaseState
    {
        private ObjectPool objectPool;
        private BloodWizzard enemy;
        public BloodWizzardAttackState(BloodWizzard enemy, StateMachine stateMachine, ObjectPool objectPool) : base(enemy, stateMachine)
        {
            this.objectPool = objectPool;
            this.enemy = enemy;
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            baseEnemy.Animator.SetBool("IsAttacking", true);
            baseEnemy.MyRigidbody.mass *= 1000;
            //int index = Random.Range(0, baseEnemy.EnemyData.attackSounds.Count);
            // baseEnemy.AudioSource.PlayOneShot(baseEnemy.EnemyData.attackSounds[index]);
            enemy.StartCoroutine(Attack());
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            baseEnemy.Animator.SetBool("IsAttacking", false);
            baseEnemy.MyRigidbody.mass /= 1000;
        }

        private IEnumerator Attack()
        {
            Debug.Log("Attacking");
            yield return new WaitForSeconds(enemy.Animator.GetCurrentAnimatorStateInfo(0).length);
            GameObject projectile = objectPool.GetObjectFromPool(enemy.Projectile.PoolObjectType, enemy.Projectile.gameObject, enemy.ShootPosition.position).GetGameObject();
            projectile.transform.position = enemy.ShootPosition.position;
            projectile.GetComponent<EnemyProjectile>().Init(enemy.Target.position - enemy.ShootPosition.position, enemy.EnemyData.damage, enemy.EnemyData.projectileSpeed);
            stateMachine.ChangeState(baseEnemy.FollowState);
        }
    }
}

