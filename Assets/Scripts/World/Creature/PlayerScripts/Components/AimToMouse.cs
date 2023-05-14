using App.World.Creatures.PlayerScripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.PlayerScripts.Components
{
    public class AimToMouse : MonoBehaviour
    {
        [SerializeField]
        private Transform shootPositionTransform;
        [SerializeField]
        private AimToMouseEvent aimEvent;

        private void OnEnable()
        {
            aimEvent.OnAimToMouse += OnAimToMouse;
        }
        private void OnDisable()
        {
            aimEvent.OnAimToMouse -= OnAimToMouse;
        }
        private void OnAimToMouse(AimToMouseEvent ev, AimToMouseEventArgs args)
        {
            DoAimToMouse(shootPositionTransform, args.angle, args.playerPos, args.mousePos);
        }
        private void DoAimToMouse(Transform shootPositionTransform, float aimAngle, float playerPos, float cursorPos)
        {
            shootPositionTransform.eulerAngles = new Vector3(0f, 0f, aimAngle);
            Vector3 scaleVector;
            if (cursorPos >= playerPos)
                scaleVector = new Vector3(1f, 1f, 0f);
            else
                scaleVector = new Vector3(1f, -1f, 0f);
            shootPositionTransform.localScale = scaleVector;
        }



    }
}