using App.World.Creatures.PlayerScripts.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Systems.Input
{
    public class InputSystem : MonoBehaviour
    {

        private Camera mainCamera;
        private Player player;

        private void Update()
        {
            HandleAimInput();
            HandleMoveInput();
            HandleShootInput();
            HandleDashInput();
        }
        public void Init(Camera mainCamera, Player player)
        {
            this.mainCamera = mainCamera;
            this.player = player;
            
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
            if ((GetMousePositionInWorld() - player.WeaponAnchor.position).magnitude < (player.ShootPosition.position - player.WeaponAnchor.position).magnitude * 2.2)
            {
                lookDirection = GetMousePositionInWorld() - player.WeaponAnchor.position;
            }
            else
            {
                lookDirection = GetMousePositionInWorld() - player.ShootPosition.position;
            }
            float rads = Mathf.Atan2(lookDirection.y, lookDirection.x);
            float direction = rads * Mathf.Rad2Deg;
            return direction;
        }
        private void HandleAimInput()
        {
            float aimAngle = GetDirectionAngle();
            float cursorPos = UnityEngine.Input.mousePosition.x;
            float playerPosInScreen = mainCamera.WorldToScreenPoint(player.PlayerTransform.position).x;
            player.AimEvent.CallAimEvent(aimAngle, playerPosInScreen, cursorPos);
        }
        private void HandleMoveInput()
        {
            float horizontalMove = UnityEngine.Input.GetAxis("Horizontal");
            float verticalMove = UnityEngine.Input.GetAxis("Vertical");
            Vector2 movingDirection = new Vector2(horizontalMove, verticalMove).normalized;

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
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                player.DashEvent.CallDashEvent(movingDirection, player.DashDistance, player.DashTime);
            }
        }
        private void HandleShootInput()
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                player.Staff.WeakShootEvent.CallWeakShootEvent();
            }
            if (UnityEngine.Input.GetMouseButton(1))
            {
                player.Staff.StrongShootEvent.CallStrongShootEvent();
            }
        }
    }
}
