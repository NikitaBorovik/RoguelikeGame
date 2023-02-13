using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.Enemies.States
{
    public class DieState : EnemyBaseState
    {
        public DieState(BaseEnemy baseEnemy, StateMachine stateMachine) : base(baseEnemy, stateMachine) { }

        public override void Enter()
        {
            Debug.Log("I am dead");
            foreach (Collider2D collider in baseEnemy.MyColliders)
                collider.enabled = false;
            baseEnemy.Animator.SetBool("IsSpawning", false);
            baseEnemy.Animator.SetBool("IsAttacking", false);
            if (!baseEnemy.Animator.GetBool("MovingRight") && !baseEnemy.Animator.GetBool("MovingLeft"))
                baseEnemy.Animator.SetBool("MovingLeft", true);
            baseEnemy.Animator.SetBool("IsDying", true);
        }

        public override void Exit()
        {
            foreach (Collider2D collider in baseEnemy.MyColliders)
                collider.enabled = true;
            baseEnemy.Animator.SetBool("IsDying", false);
            baseEnemy.Animator.SetBool("MovingRight", false);
            baseEnemy.Animator.SetBool("MovingLeft", false);
        }
    }
}
