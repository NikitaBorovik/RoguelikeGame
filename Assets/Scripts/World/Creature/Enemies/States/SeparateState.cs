using App;
using App.World.Creatures.Enemies;
using App.World.Creatures.Enemies.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateState : EnemyBaseState
{
    
    private Vector3 velocity;
    public Vector3 Velocity { get => velocity; set => velocity = value; }

    public SeparateState(BaseEnemy baseEnemy, StateMachine stateMachine) : base(baseEnemy, stateMachine)
    {
        this.baseEnemy = baseEnemy;
    }

    public override void Enter()
    {
        baseEnemy.MyRigidbody.velocity = Velocity;
        SetMoveAnimationParams(baseEnemy.MyRigidbody.velocity.x);
        baseEnemy.StartCoroutine(WaitUntillSeparate());
    }

    private IEnumerator WaitUntillSeparate()
    {
        yield return new WaitForSeconds(baseEnemy.EnemyData.separateShift / Velocity.magnitude);
        stateMachine.ChangeState(baseEnemy.FollowState);
    }
    private void SetMoveAnimationParams(float vx)
    {
        if (vx < 0)
        {
            baseEnemy.Animator.SetBool("MovingRight", false);
            baseEnemy.Animator.SetBool("MovingLeft", true);
        }
        else
        {
            baseEnemy.Animator.SetBool("MovingRight", true);
            baseEnemy.Animator.SetBool("MovingLeft", false);
        }
    }

}
