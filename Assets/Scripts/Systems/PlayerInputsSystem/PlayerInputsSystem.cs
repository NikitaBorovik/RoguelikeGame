using App.UI;
using App.World.Creatures.PlayerScripts.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Systems.Input
{
    public class PlayerInputsSystem : MonoBehaviour
    {

        private Camera mainCamera;
        private Player player;
        private PauseController pauseController;

        private void Update()
        {
            HandleAimToMouseInput();
            HandleMoveInput();
            HandleShootInput();
            HandleDashInput();
            HandleObtainInput();
            HandleMouseWheelInput();
            HandlePauseInput();
        }
        public void Init(Camera mainCamera, Player player, PauseController pauseController)
        {
            this.mainCamera = mainCamera;
            this.player = player;
            this.pauseController = pauseController;
        }

        private Vector3 GetMousePositionInWorld()
        {
            Vector3 mouseOnScreenPos = UnityEngine.Input.mousePosition;
            mouseOnScreenPos = mainCamera.ScreenToWorldPoint(mouseOnScreenPos);
            mouseOnScreenPos.z = 0f;
            return mouseOnScreenPos;
        }
        private float GetDirectionAngle()
        {
            Vector3 lookDirection;
            lookDirection = GetMousePositionInWorld() - player.ShootPosition.position;
            float rads = Mathf.Atan2(lookDirection.y, lookDirection.x);
            float direction = rads * Mathf.Rad2Deg;
            return direction;
        }
        private void HandleAimToMouseInput()
        {
            float aimAngle = GetDirectionAngle();
            float cursorPos = UnityEngine.Input.mousePosition.x;
            float playerPosInScreen = mainCamera.WorldToScreenPoint(player.PlayerTransform.position).x;
            player.AimEvent.CallAimEvent(aimAngle, playerPosInScreen, cursorPos);
        }
        private void HandleMoveInput()
        {
            float horizontalAxis = UnityEngine.Input.GetAxis("Horizontal");
            float verticalAxis = UnityEngine.Input.GetAxis("Vertical");
            Vector2 movingDirection = new Vector2(horizontalAxis, verticalAxis).normalized;

            if (movingDirection == Vector2.zero)
            {
                if (player.GetComponent<Stand>().isActiveAndEnabled)
                    player.StandEvent.CallStandEvent();
            }
            else
            {
                player.MovementEvent.CallMovementEvent(movingDirection, player.MovementSpeed);
            }
        }
        private void HandleDashInput()
        {
            float horizontalMove = UnityEngine.Input.GetAxis("Horizontal");
            float verticalMove = UnityEngine.Input.GetAxis("Vertical");
            Vector2 movingDirection = new Vector2(horizontalMove, verticalMove).normalized;
            if (movingDirection == Vector2.zero)
                return;
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && player.DashCooldownTimer <= 0 && !player.PAnimator.GetBool("isAttacking"))
            {
                player.DashEvent.CallDashEvent(movingDirection, player.DashDistance, player.DashTime);
            }
        }
        private void HandleShootInput()
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                player.ShootEvent.CallShootEvent();
            }
            
        }

        private void HandleObtainInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                player.ObtainEvent.CallObtainEvent();
            }
        }

        private void HandleMouseWheelInput()
        {
            if (UnityEngine.Input.mouseScrollDelta.y > 0)
            {
                int index = player.Powers.IndexOf(player.Projectile);
                player.Projectile = player.Powers[(index + 1) % player.Powers.Count];
            }
            else if (UnityEngine.Input.mouseScrollDelta.y < 0)
            {
                int index = player.Powers.IndexOf(player.Projectile) - 1;
                if (index < 0)
                {
                    index = player.Powers.Count - 1;
                }
                player.Projectile = player.Powers[index];
            }
        }
        private bool HandlePauseInput()
        {
            if (!UnityEngine.Input.GetKeyDown(KeyCode.Escape)) return pauseController.Paused;

            if (pauseController.Paused)
                pauseController.Unpause();
            else
                pauseController.Pause();

            return pauseController.Paused;
        }
    }
}
