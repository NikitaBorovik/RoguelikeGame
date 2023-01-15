using App.Systems.Input;
using App.World;
using App.World.Creatures.PlayerScripts.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App
{
    public class App : MonoBehaviour
    {
        [SerializeField]
        private ObjectContainer objectsContainer;
        [SerializeField]
        private InputSystem inputSystem;
        [SerializeField]
        private Camera mainCamera;
        
        void Start()
        {
            inputSystem.Init(mainCamera, objectsContainer.Player.GetComponent<Player>());
        }

        
    }

}

