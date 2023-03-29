using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.UI
{
    public class OnLoadCallBack : MonoBehaviour
    {
        [SerializeField] private float minimalLoadingTime;
        private float currTime;

        private void Awake()
        {
            currTime = 0f;
            Time.timeScale = 1f;
        }

        private void FixedUpdate()
        {
            currTime += Time.fixedDeltaTime;

            if (currTime > minimalLoadingTime)
            {
                currTime = 0f;
                SceneCoordinator.SwitchToLoadedScene?.Invoke();
            }
        }
    }
}
