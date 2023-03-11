using App.Systems.Spawning;
using App.World.Creatures.Enemies.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace App.Systems.GameStates
{
    public class GameStatesSystem : MonoBehaviour, INotifyRoomChanged , INotifyRoomCleared
    {
        private StateMachine gameStateMachine;
        private DungeonGenerator dungeonBuilder;
        private DungeonBuildingState dungeonBuildingState;
        private EnteringRoomState enteringRoomState;
        private SpawningSystem spawningSystem;
        private RoomClearedState roomClearedState;
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
            roomClearedState = new RoomClearedState(this);
            gameStateMachine.Initialize(dungeonBuildingState);
        }
        public void EnteringRoom()
        {
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
            curRoom = room;
            spawningSystem.CurrentRoom = room;
        }

        public void NotifyRoomCleared()
        {
            gameStateMachine.ChangeState(roomClearedState);
        }
    }
}

