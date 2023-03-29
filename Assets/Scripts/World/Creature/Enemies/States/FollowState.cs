using App.World.Creatures.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace App.World.Creatures.Enemies.States
{
    public class FollowState : EnemyBaseState
    {
        private Stack<Vector3> path;
        private float timeToRecalculatePath = 0.1f;
        private float timeSinceLastPathRecalculation = 0f;
        private Vector3 currentTarget;
        private float attackDelay;
        private float radiusToCheckSeparate;
        public FollowState(BaseEnemy baseEnemy, StateMachine stateMachine) : base(baseEnemy, stateMachine) 
        {
            radiusToCheckSeparate = baseEnemy.EnemyData.distanceToSeparate;
        }

        public override void Enter()
        {
            path = baseEnemy.Pathfinding.FindPath(baseEnemy.transform.position, baseEnemy.Target.position, baseEnemy.CurrentRoom);
            if (path != null && path.Count > 0)
                currentTarget = path.Pop();
            else
                currentTarget = baseEnemy.Target.position;
            attackDelay = 0;
        }

        public override void Update()
        {
            attackDelay += Time.deltaTime;
            timeSinceLastPathRecalculation += Time.deltaTime;
            if (ShouldSeparateFromOtherEnemies())
            {
                baseEnemy.SeparateState.Velocity = CalculateVelocityToSeparate();
                stateMachine.ChangeState(baseEnemy.SeparateState);
            }   
            else if (Vector3.Distance(baseEnemy.transform.position, baseEnemy.Target.position) < baseEnemy.EnemyData.attackRange &&
                attackDelay >= baseEnemy.EnemyData.timeBetweenAttacks && !Physics.Raycast(baseEnemy.transform.position, baseEnemy.Target.position - baseEnemy.transform.position,
                (baseEnemy.Target.position - baseEnemy.transform.position).magnitude, LayerMask.NameToLayer("Wall")))
            {
                stateMachine.ChangeState(baseEnemy.AttackState);
            }
            else if (baseEnemy.EnemyData.attackType == "Ranged" && Vector3.Distance(baseEnemy.transform.position, baseEnemy.Target.position) < baseEnemy.EnemyData.attackRange / 2)
            {
                baseEnemy.MyRigidbody.velocity = (baseEnemy.transform.position - baseEnemy.Target.position).normalized * baseEnemy.EnemyData.speed;
            }
            else if(Vector3.Distance(baseEnemy.transform.position, baseEnemy.Target.position) > baseEnemy.EnemyData.attackRange)
            {
                if (Vector3.Distance(baseEnemy.transform.position, currentTarget) <= 1.5f)
                {
                    if (timeSinceLastPathRecalculation >= timeToRecalculatePath)
                    {
                        timeSinceLastPathRecalculation = 0f;
                        path = baseEnemy.Pathfinding.FindPath(baseEnemy.transform.position, baseEnemy.Target.position, baseEnemy.CurrentRoom);
                    }
                    if (path != null && path.Count > 0)
                    {
                        currentTarget = path.Pop();
                    }
                    else
                    {
                        currentTarget = baseEnemy.Target.position;
                    }
                }
                baseEnemy.MyRigidbody.velocity = (currentTarget - baseEnemy.transform.position).normalized * baseEnemy.EnemyData.speed;
            }
            SetMoveAnimationParams(baseEnemy.MyRigidbody.velocity.x);
            

        }
        public override void Exit()
        {
            baseEnemy.MyRigidbody.velocity = Vector2.zero;
        }

        private bool ShouldSeparateFromOtherEnemies()
        {
            var hits = Physics2D.OverlapCircleAll(baseEnemy.transform.position, radiusToCheckSeparate);
            foreach (var hit in hits)
            {
                if (hit.GetComponent<BaseEnemy>() != null && hit.transform != baseEnemy.transform)
                {
                    return true;
                }
            }
            return false;
            
        }
        
        private Vector3 CalculateVelocityToSeparate()
        {
            float separateSpeed = baseEnemy.EnemyData.speed / 2f;

            Vector2 sum = Vector2.zero;
            int count = 0;

            var hits = Physics2D.OverlapCircleAll(baseEnemy.transform.position, radiusToCheckSeparate);
            foreach (var hit in hits)
            {
                if (hit.GetComponent<BaseEnemy>() != null && hit.transform != baseEnemy.transform)
                {
                    Vector2 difference = baseEnemy.transform.position - hit.transform.position;
                    difference = difference.normalized / Mathf.Abs(difference.magnitude);
                    sum += difference;
                    count++;
                }
            }

            if (count > 0)
            {
                sum = sum.normalized * separateSpeed;
            }
            return sum;
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
}
