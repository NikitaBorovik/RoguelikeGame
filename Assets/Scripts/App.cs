using App.Systems.GameStates;
using App.Systems.Input;
using App.World;
using App.World.Creatures.PlayerScripts.Components;
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
        [SerializeField]
        private GameStatesSystem gameStatesSystem;

        void Start()
        {
            inputSystem.Init(mainCamera, objectsContainer.Player.GetComponent<Player>());
            gameStatesSystem.Init(objectsContainer.DungeonGenerator);
        }

        
    }

}

