using App.Systems.Spawning;
using App.World.Creatures.Enemies.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace App.Systems.GameStates
{
    public class GameStatesSystem : MonoBehaviour, INotifyRoomChanged
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
        [SerializeField]
        public AStarTest aStarTest;

        public Room CurRoom { get => curRoom; set { curRoom = value; } }

        public void Init(DungeonGenerator dungeonGenerator,SpawningSystem spawningSystem)
        {
            dungeonBuilder = dungeonGenerator;
            dungeonBuilder.NotifyRoomChanged = this;
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
           // spawningSystem.CurrentRoom = curRoom;
            if(aStarTest == null)
            {
                Debug.Log("Null");
            }
            aStarTest.room = curRoom;//
            aStarTest.grid = curRoom.DrawnRoom.Grid;//
            gameStateMachine.ChangeState(enteringRoomState);
        }

        public void NotifyOnRoomChanged(Room room)
        {
            Debug.Log(room);
            curRoom = room;
            spawningSystem.CurrentRoom = room;
        }
    }
}

