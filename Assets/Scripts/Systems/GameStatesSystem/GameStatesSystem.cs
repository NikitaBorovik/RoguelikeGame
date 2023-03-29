using App.Systems.Spawning;
using App.UI;
using App.World.Creatures.PlayerScripts.Components;
using App.World.DungeonComponents;
using App.World.WorldObjects;
using System.Collections.Generic;
using UnityEngine;

namespace App.Systems.GameStates
{
    public class GameStatesSystem : MonoBehaviour, INotifyRoomChanged , INotifyRoomCleared , INotifyGameEnded
    {
        private StateMachine gameStateMachine;
        private DungeonGenerator dungeonBuilder;
        private DungeonBuildingState dungeonBuildingState;
        private EnteringRoomState enteringRoomState;
        private SpawningSystem spawningSystem;
        private RoomClearedState roomClearedState;
        private GameEndedState gameEndedState;
        private Player player;
        private int curLevel = 0;
        [SerializeField]
        private List<LevelModel> levels;
        [SerializeField]
        private Room curRoom;
        [SerializeField]
        private Portal portal;
        [SerializeField]
        private DeathScreenController deathScreenController;

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
            gameEndedState = new GameEndedState(this, deathScreenController);
            this.player = player;
            player.NotifiebleForGameEnded = this;
            gameStateMachine.Initialize(dungeonBuildingState);
        }
        public void EnteringRoom()
        {
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
                NotifyGameEnded();
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

        public void NotifyGameEnded()
        {
            gameStateMachine.ChangeState(gameEndedState);
        }
    }
}

