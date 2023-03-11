using App.World.Creatures.PlayerScripts.Events;
using System;
using UnityEngine;
namespace App.World.Creatures.PlayerScripts.Components
{
    public class PlayerAnimationsController : MonoBehaviour
    {
        private Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }
        private void OnEnable()
        {
            player.StandEvent.OnStand += AnimateOnStand;
            player.AimEvent.OnAim += AnimateOnAim;
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

        private void Update()
        {
          //  if (player.PAnimator.GetBool("isDashing"))
           // Debug.Log("Working");
        }

        private void OnDisable()
        {
            player.StandEvent.OnStand -= AnimateOnStand;
            player.AimEvent.OnAim -= AnimateOnAim;
            player.MovementEvent.OnMove -= AnimateOnMove;
            player.DashEvent.OnDash -= AnimateOnDash;
            player.ShootEvent.OnShoot -= AnimateOnShoot;
        }
        private void AnimateOnShoot(ShootEvent obj)
        {
            SetShootAnimationParams();
        }

        
        private void AnimateOnStand(StandEvent ev)
        {
            SetStandAnimationParams();
        }
        private void AnimateOnAim(AimEvent ev,AimEventArgs args)
        {
            SetAimAnimationParams(args.playerPos, args.mousePos);
        }
        private void AnimateOnMove(MovementEvent ev, MovementEventArgs args)
        {
            SetMovementAnimationParams();
        }
        private void AnimateOnDash(DashEvent ev, DashEventArgs args)
        {
            SetDashAnimationParams();
        }
        public void SetStandAnimationParams()
        {
            player.PAnimator.SetBool("isIdle", true);
           // player.PAnimator.SetBool("isDashing", false);
            player.PAnimator.SetBool("isMoving", false);
        }
        public void SetAimAnimationParams(float playerPos, float cursorPos)
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
        public void SetMovementAnimationParams()
        {
            player.PAnimator.SetBool("isIdle", false);
          //  player.PAnimator.SetBool("isDashing", false);
            player.PAnimator.SetBool("isMoving", true);
        }
        public void SetDashAnimationParams()
        {
            player.PAnimator.SetBool("isDashing",true);
        }
        private void SetShootAnimationParams()
        {
            player.PAnimator.SetBool("isAttacking", true);
        }

    }
}