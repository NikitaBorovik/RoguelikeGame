using App.World.Creatures.PlayerScripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.PlayerScripts.Components
{
    public class Aim : MonoBehaviour
    {
        [SerializeField]
        private Transform shootPositionTransform;
        [SerializeField]
        private AimEvent aimEvent;

        private void OnEnable()
        {
            aimEvent.OnAim += OnAimWeapon;
        }
        private void OnDisable()
        {
            aimEvent.OnAim -= OnAimWeapon;
        }
        private void OnAimWeapon(AimEvent ev, AimEventArgs args)
        {
            AimWeaponWithMouse(shootPositionTransform, args.angle, args.playerPos, args.mousePos);
        }
        private void AimWeaponWithMouse(Transform shootPositionTransform, float aimAngle, float playerPos, float cursorPos)
        {
            shootPositionTransform.eulerAngles = new Vector3(0f, 0f, aimAngle);
            //Debug.Log(weaponAnchorTransform.eulerAngles);
            if (cursorPos >= playerPos)
            {
                shootPositionTransform.localScale = new Vector3(1f, 1f, 0f);
            }
            else
            {
                shootPositionTransform.localScale = new Vector3(1f, -1f, 0f);
            }

        }



    }
}