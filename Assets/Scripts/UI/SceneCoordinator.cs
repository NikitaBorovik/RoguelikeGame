using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace App.UI
{
    public static class SceneCoordinator
    {
        public delegate void OnSceneSwitched();
        public static Action SwitchToLoadedScene;
        private static readonly string LoadingScene = "Loading";

        public static void SwitchToSceneLoadingScreen(string name)
        {
            SwitchToLoadedScene = () => SwitchToScene(name);
            SwitchToScene(LoadingScene);
        }

        public static void SwitchToScene(string name)
        {
            if (IsRunningScene(name))
                throw new InvalidOperationException($"Cannot switch to running sceene.");

            SceneManager.LoadScene(name);
        }

        public static bool IsRunningScene(string name)
           => SceneManager.GetActiveScene().name == name;
    }
}
