using App.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.Enemies.States.ConcreteStates
{
    public class BatAttackState : EnemyBaseState
    {
        private ObjectPool objectPool;
        private Bat enemy;
        public BatAttackState(Bat enemy, StateMachine stateMachine, ObjectPool objectPool) : base(enemy, stateMachine)
        {
            this.objectPool = objectPool;
            this.enemy = enemy;
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            baseEnemy.Animator.SetBool("IsAttacking", true);
            baseEnemy.MyRigidbody.mass *= 1000;
            baseEnemy.AudioSource.PlayOneShot(baseEnemy.EnemyData.attackSound);
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
            yield return new WaitForSeconds(enemy.Animator.GetCurrentAnimatorStateInfo(0).length/2);
            for (int i = 0; i < enemy.EnemyData.projectileCount; i++)
            {
                GameObject projectile = objectPool.GetObjectFromPool(enemy.Projectile.PoolObjectType, enemy.Projectile.gameObject, enemy.ShootPosition.position).GetGameObject();
                projectile.transform.position = enemy.ShootPosition.position;
                projectile.GetComponent<EnemyProjectile>().Init(enemy.Target.position - enemy.transform.position, enemy.EnemyData.damage, enemy.EnemyData.projectileSpeed);
                baseEnemy.AudioSource.PlayOneShot(baseEnemy.EnemyData.attackSound);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(enemy.Animator.GetCurrentAnimatorStateInfo(0).length / 2 - 0.01f * enemy.EnemyData.projectileCount);
            stateMachine.ChangeState(baseEnemy.FollowState);
        }
    }
}
