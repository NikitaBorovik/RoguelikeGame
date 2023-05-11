using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.World.Creatures.Enemies.States.ConcreteStates
{
    public class MeleeAttackState : EnemyBaseState
    {
        private MeleeEnemy enemy;
        public MeleeAttackState(MeleeEnemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
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
            yield return new WaitForSeconds(enemy.Animator.GetCurrentAnimatorStateInfo(0).length);
            Vector3 direction = enemy.Target.position - enemy.transform.position;
            enemy.Attack.transform.right = direction;
            enemy.Attack.DamageTargets();
            baseEnemy.AudioSource.PlayOneShot(baseEnemy.EnemyData.attackSound);
            stateMachine.ChangeState(baseEnemy.FollowState);
        }

    }
        
}
