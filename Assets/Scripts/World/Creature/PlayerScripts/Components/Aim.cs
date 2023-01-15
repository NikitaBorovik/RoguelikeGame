using App.World.Creatures.PlayerScripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.PlayerScripts.Components
{
    public class Aim : MonoBehaviour
    {
        [SerializeField]
        private Transform weaponAnchorTransform;
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
            AimWeaponWithMouse(weaponAnchorTransform, args.angle, args.playerPos, args.mousePos);
        }
        private void AimWeaponWithMouse(Transform weaponAnchorTransform, float aimAngle, float playerPos, float cursorPos)
        {
            weaponAnchorTransform.eulerAngles = new Vector3(0f, 0f, aimAngle);
            //Debug.Log(weaponAnchorTransform.eulerAngles);
            if (cursorPos >= playerPos)
            {
                weaponAnchorTransform.localScale = new Vector3(1f, 1f, 0f);
            }
            else
            {
                weaponAnchorTransform.localScale = new Vector3(1f, -1f, 0f);
            }

        }



    }
}