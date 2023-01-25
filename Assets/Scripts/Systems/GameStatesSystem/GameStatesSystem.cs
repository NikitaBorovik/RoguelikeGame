using App.Systems.Spawning;
using App.World.Creatures.Enemies.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace App.Systems.GameStates
{
    public class GameStatesSystem : MonoBehaviour
    {
        private StateMachine gameStateMachine;
        private DungeonGenerator dungeonBuilder;
        private DungeonBuildingState dungeonBuildingState;
        private EnteringRoomState enteringRoomState;
        private SpawningSystem spawningSystem;
        [SerializeField]
        private List<LevelModel> levels;
        [SerializeField]
        private int curLevel = 0;
        private Room curRoom;

        public Room CurRoom { get => curRoom; set => curRoom = value; }

        public void Init(DungeonGenerator dungeonGenerator,SpawningSystem spawningSystem)
        {
            dungeonBuilder = dungeonGenerator;
            this.spawningSystem = spawningSystem;
            gameStateMachine = new StateMachine();
            dungeonBuildingState = new DungeonBuildingState(this, dungeonBuilder, levels[curLevel]);
            enteringRoomState = new EnteringRoomState(this, spawningSystem);
            gameStateMachine.Initialize(dungeonBuildingState);
        }
        private void Update()
        {
           // gameStateMachine.CurrentState.Update();
        }
        public void EnteringRoom()
        {
            spawningSystem.CurrentRoom = curRoom;
            gameStateMachine.ChangeState(enteringRoomState);
        }
    }
}

