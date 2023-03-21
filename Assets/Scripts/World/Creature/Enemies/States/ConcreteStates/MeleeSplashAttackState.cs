using App.World.Creatures.Enemies.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.Enemies.States.ConcreteStates
{
    public class MeleeSplashAttackState : EnemyBaseState
    {
        private MeleeSplashEnemy enemy;
        public MeleeSplashAttackState(MeleeSplashEnemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
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
            Debug.Log("Attacking splash");
            enemy.DangerZone.SetActive(true);
            yield return new WaitForSeconds(enemy.Animator.GetCurrentAnimatorStateInfo(0).length);
            enemy.Attack.DamageTargets();
            enemy.DangerZone.SetActive(false);
            stateMachine.ChangeState(baseEnemy.FollowState);
        }
    }

}
