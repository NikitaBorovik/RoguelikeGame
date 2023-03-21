using App.Systems.Spawning;
using App.World.Creatures.PlayerScripts.Components;
using App.World.DungeonComponents;
using App.World.WorldObjects;
using System.Collections.Generic;
using UnityEngine;

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
        private Player player;
        private int curLevel = 0;
        [SerializeField]
        private List<LevelModel> levels;
        [SerializeField]
        private Room curRoom;
        [SerializeField]
        public AStarTest aStarTest;
        [SerializeField]
        private Portal portal;

        public Room CurRoom { get => curRoom; set { curRoom = value; } }

        public int CurLevel { get => curLevel; }

        public void Init(DungeonGenerator dungeonGenerator, SpawningSystem spawningSystem, Player player)
        {
            dungeonBuilder = dungeonGenerator;
            dungeonBuilder.NotifyRoomChanged = this;
            this.spawningSystem = spawningSystem;
            gameStateMachine = new StateMachine();
            dungeonBuildingState = new DungeonBuildingState(this, dungeonBuilder, levels[CurLevel]);
            enteringRoomState = new EnteringRoomState(this, spawningSystem , CurLevel);
            roomClearedState = new RoomClearedState(this);
            this.player = player;
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

        public void StageCleared()
        {
            curLevel++;
            if (CurLevel < levels.Count)
            {
                dungeonBuildingState.LevelToBuild = levels[curLevel];
                enteringRoomState.CurLevel = curLevel;
                gameStateMachine.ChangeState(dungeonBuildingState);
                player.transform.position = Vector3.zero;
            }
            else
            {
                Debug.Log("Game Cleared");
            }
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                //gameStateMachine.ChangeState(dungeonBuildingState);
                 StageCleared();
                 player.transform.position = Vector3.zero;
                
            }
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

