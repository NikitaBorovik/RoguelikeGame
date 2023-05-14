using App.World.Creatures.PlayerScripts.Events;
using System;
using UnityEngine;
namespace App.World.Creatures.PlayerScripts.Components
{
    public class PlayerAnimationsController : MonoBehaviour
    {
        private Player player;

        private void OnEnable()
        {
            player = GetComponent<Player>();
            player.StandEvent.OnStand += AnimateOnStand;
            player.AimEvent.OnAimToMouse += AnimateOnLookAtDirection;
            player.MovementEvent.OnMove += AnimateOnMove;
            player.ShootEvent.OnShoot += AnimateOnShoot;
            if (player.DashEvent == null)
            {
                Debug.Log("DashEvent is null");
            }
            else
            {

                player.DashEvent.OnDash += AnimateOnDash;
            }
        }

        private void OnDisable()
        {
            player.StandEvent.OnStand -= AnimateOnStand;
            player.AimEvent.OnAimToMouse -= AnimateOnLookAtDirection;
            player.MovementEvent.OnMove -= AnimateOnMove;
            player.DashEvent.OnDash -= AnimateOnDash;
            player.ShootEvent.OnShoot -= AnimateOnShoot;
        }
        private void AnimateOnShoot(ShootEvent obj)
        {
            AddShootAnimationParams();
        }

        
        private void AnimateOnStand(StandEvent ev)
        {
            AddStandAnimationParams();
        }
        private void AnimateOnLookAtDirection(AimToMouseEvent ev,AimToMouseEventArgs args)
        {
            AddLookAtDirectionAnimationParams(args.playerPos, args.mousePos);
        }
        private void AnimateOnMove(MovementEvent ev, MovementEventArgs args)
        {
            AddMovementAnimationParams();
        }
        private void AnimateOnDash(DashEvent ev, DashEventArgs args)
        {
            AddDashAnimationParams();
        }
        public void AddStandAnimationParams()
        {
            player.PAnimator.SetBool("isIdle", true);
            player.PAnimator.SetBool("isMoving", false);
        }
        public void AddLookAtDirectionAnimationParams(float playerPos, float cursorPos)
        {
            if (cursorPos >= playerPos)
            {
                player.PAnimator.SetBool("aimRight", true);
                player.PAnimator.SetBool("aimLeft", false);
            }
            else
            {
                player.PAnimator.SetBool("aimRight", false);
                player.PAnimator.SetBool("aimLeft", true);
            }
        }
        public void AddMovementAnimationParams()
        {
            player.PAnimator.SetBool("isIdle", false);
            player.PAnimator.SetBool("isMoving", true);
        }
        public void AddDashAnimationParams()
        {
            player.PAnimator.SetBool("isDashing",true);
        }
        private void AddShootAnimationParams()
        {
            player.PAnimator.SetBool("isAttacking", true);
        }

    }
}