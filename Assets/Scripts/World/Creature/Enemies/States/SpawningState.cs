using App.World.Creatures.Enemies;
using System;
using System.Collections;
using UnityEngine;
namespace App.World.Creatures.Enemies.States
{
    public class SpawningState : EnemyBaseState
    {
        private Animator animator;
        private float spawnTime;
        
        public SpawningState(BaseEnemy baseEnemy, StateMachine stateMachine, Animator animator) : base(baseEnemy, stateMachine)
        {
            this.animator = animator;
        }
        public override void Enter()
        {
            spawnTime = animator.GetCurrentAnimatorStateInfo(0).length;
            baseEnemy.StartCoroutine(Spawn());
        }

        public override void Exit()
        {
            baseEnemy.Animator.SetBool("IsSpawning", false);
        }

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(spawnTime);
            stateMachine.ChangeState(baseEnemy.FollowState);
        }

        private void resetAllAnimationParams()
        {
            baseEnemy.Animator.SetBool("IsSpawning", true);
            baseEnemy.Animator.SetBool("MovingRight", false);
            baseEnemy.Animator.SetBool("MovingLeft", false);
            baseEnemy.Animator.SetBool("IsAttacking", false);
            baseEnemy.Animator.SetBool("IsDying", false);
        }
    }
}

