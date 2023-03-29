using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.UI
{
    public class MonoSceneCoordinator : MonoBehaviour
    {
        public void SwitchToSceneLoadingScreen(string sceneName)
                => SceneCoordinator.SwitchToSceneLoadingScreen(sceneName);

        public static void SwitchToScene(string sceneName)
            => SceneCoordinator.SwitchToScene(sceneName);

        public static bool IsRunningScene(string sceneName)
           => SceneCoordinator.IsRunningScene(sceneName);

        public static void Quit()
            => Application.Quit();
    }
}
