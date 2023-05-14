using App.Systems;
using System.Collections;
using UnityEngine;

namespace App.World.Creatures.Enemies.States.ConcreteStates
{
    public class DevilEyeAttackState : EnemyBaseState
    {
        private ObjectPool objectPool;
        private DevilEye enemy;
        public DevilEyeAttackState(DevilEye enemy, StateMachine stateMachine, ObjectPool objectPool) : base(enemy, stateMachine)
        {
            this.objectPool = objectPool;
            this.enemy = enemy;
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            baseEnemy.Animator.SetBool("IsAttacking", true);
            baseEnemy.MyRigidbody.mass *= 1000;
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
                Vector3 startingDirection = (enemy.Target.position - enemy.transform.position).normalized;
                Vector3 direction = Quaternion.AngleAxis(360 / enemy.EnemyData.projectileCount * i, Vector3.forward) * startingDirection;
                projectile.GetComponent<EnemyProjectile>().Init(direction, enemy.EnemyData.damage, enemy.EnemyData.projectileSpeed);
                baseEnemy.AudioSource.PlayOneShot(baseEnemy.EnemyData.attackSound);
            }
            yield return new WaitForSeconds(enemy.Animator.GetCurrentAnimatorStateInfo(0).length / 2);
            stateMachine.ChangeState(baseEnemy.FollowState);
        }
    }
}
