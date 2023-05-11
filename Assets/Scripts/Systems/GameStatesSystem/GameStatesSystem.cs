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
        #region Fields
        private StateMachine gameStateMachine;
        private DungeonGenerator dungeonBuilder;
        private DungeonBuildingState dungeonBuildingState;
        private EnteringRoomState enteringRoomState;
        private SpawningSystem spawningSystem;
        private RoomClearedState roomClearedState;
        private GameEndedState gameEndedState;
        private DungeonExploringState dungeonExploringState;
        private Player player;
        private int curLevel = 0;
        [SerializeField]
        private List<LevelModel> levels;
        [SerializeField]
        private RoomData curRoom;
        [SerializeField]
        private Portal portal;
        [SerializeField]
        private DeathScreenController deathScreenController;
        [SerializeField]
        private AudioClip fightingMusic;
        [SerializeField]
        private AudioClip restingMusic;
        [SerializeField]
        private AudioClip doorOpeningSound;
        private AudioSource audioSource;
        [SerializeField]
        private GameObject loadingScreen;
        #endregion

        #region Properties
        public RoomData CurRoom { get => curRoom; set { curRoom = value; } }
        public int CurLevel { get => curLevel; }
        public DungeonExploringState DungeonExploringState { get => dungeonExploringState; set => dungeonExploringState = value; }
        public GameEndedState GameEndedState { get => gameEndedState; set => gameEndedState = value; }
        public RoomClearedState RoomClearedState { get => roomClearedState; set => roomClearedState = value; }
        public SpawningSystem SpawningSystem { get => spawningSystem; set => spawningSystem = value; }
        public EnteringRoomState EnteringRoomState { get => enteringRoomState; set => enteringRoomState = value; }
        public DungeonBuildingState DungeonBuildingState { get => dungeonBuildingState; set => dungeonBuildingState = value; }
        #endregion

        public void Init(DungeonGenerator dungeonGenerator, SpawningSystem spawningSystem, Player player)
        {
            dungeonBuilder = dungeonGenerator;
            dungeonBuilder.NotifyRoomChanged = this;
            this.SpawningSystem = spawningSystem;
            gameStateMachine = new StateMachine();
            AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.1f);
            audioSource = GetComponent<AudioSource>();
            DungeonBuildingState = new DungeonBuildingState(this, dungeonBuilder, levels[CurLevel], loadingScreen);
            EnteringRoomState = new EnteringRoomState(this, spawningSystem, CurLevel, fightingMusic, audioSource);
            DungeonExploringState = new DungeonExploringState(this, restingMusic, audioSource);
            RoomClearedState = new RoomClearedState(this);
            GameEndedState = new GameEndedState(this, deathScreenController);
            this.player = player;
            player.NotifiebleForGameEnded = this;

            gameStateMachine.Initialize(DungeonBuildingState);
        }
        public void EnteringRoom()
        {
            ChangeGameState(EnteringRoomState);
        }

        public void StageCleared()
        {
            curLevel++;
            if (CurLevel < levels.Count)
            {

                DungeonBuildingState.LevelToBuild = levels[curLevel];
                EnteringRoomState.CurLevel = curLevel;
                player.transform.position = Vector3.zero;
                ChangeGameState(DungeonBuildingState);
                
            }
            else
            {
                NotifyGameEnded(true);
            }
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                 StageCleared();
                
            }
        }
        public void NotifyOnRoomChanged(RoomData room)
        {
            curRoom = room;
            SpawningSystem.CurrentRoom = room;
        }

        public void NotifyRoomCleared()
        {
            ChangeGameState(RoomClearedState);
        }

        public void NotifyGameEnded(bool isVictory)
        {
            GameEndedState.IsVictory = isVictory;
            ChangeGameState(GameEndedState);
        }
        public void ChangeGameState(IState state)
        {
            gameStateMachine.ChangeState(state);
        }
    }
}

