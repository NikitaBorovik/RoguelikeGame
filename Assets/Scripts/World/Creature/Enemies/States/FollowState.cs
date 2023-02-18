using App.World.Creatures.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.Enemies.States
{
    public class FollowState : EnemyBaseState
    {
        private Stack<Vector3> path;
        private float timeToRecalculatePath = 0.1f;
        private float timeSinceLastPathRecalculation = 0f;
        private Vector3 currentTarget;
        public FollowState(BaseEnemy baseEnemy, StateMachine stateMachine) : base(baseEnemy, stateMachine) 
        {
        }

        public override void Enter()
        {
            path = baseEnemy.Pathfinding.FindPath(baseEnemy.transform.position, baseEnemy.Target.position, baseEnemy.CurrentRoom);
            Debug.Log("Path " + path.Count);
            Debug.Log("Starting pos " + baseEnemy.transform.position);
            Debug.Log("Target" + baseEnemy.Target.position);
            currentTarget = path.Pop();
            Debug.Log(currentTarget);
        }

        public override void Update()
        {
            timeSinceLastPathRecalculation += Time.deltaTime;
            if (timeSinceLastPathRecalculation >= timeToRecalculatePath)
            {
                timeSinceLastPathRecalculation = 0f;
                path = baseEnemy.Pathfinding.FindPath(baseEnemy.transform.position, baseEnemy.Target.position, baseEnemy.CurrentRoom);
            }
            if (Vector3.Distance(baseEnemy.transform.position, currentTarget) <= 0.1f)
            {
                if (path != null && path.Count > 0 )
                {
                    currentTarget = path.Pop();
                }
                else
                {
                    currentTarget = baseEnemy.Target.position;
                }
            }
            baseEnemy.MyRigidbody.velocity = (currentTarget - baseEnemy.transform.position).normalized * baseEnemy.EnemyData.speed;
            SetMoveAnimationParams(baseEnemy.MyRigidbody.velocity.x);
           
        }
        public override void Exit()
        {
            baseEnemy.MyRigidbody.velocity = Vector2.zero;
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
