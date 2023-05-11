using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Systems.GameStates
{
    public class DungeonBuildingState : IState
    {
        private GameStatesSystem gameStatesSystem;
        
        private DungeonGenerator dungeonBuilder;
        private LevelModel levelToBuild;
        private GameObject loadingScreen;

        public LevelModel LevelToBuild { get => levelToBuild; set => levelToBuild = value; }
        public DungeonBuildingState(GameStatesSystem gameStatesSystem,DungeonGenerator dungeonBuilder,LevelModel level, GameObject loadingScreen)
        {
            this.gameStatesSystem = gameStatesSystem;
            this.dungeonBuilder = dungeonBuilder;
            this.LevelToBuild = level;
            this.loadingScreen = loadingScreen;
        }

        public void Enter()
        {
            loadingScreen.SetActive(true);
            dungeonBuilder.CreateDungeonForLevel(LevelToBuild);
            gameStatesSystem.ChangeGameState(gameStatesSystem.DungeonExploringState); 
        }

        public void Exit()
        {
            gameStatesSystem.StartCoroutine(Wait());
        }

        public void Update()
        {
            
        }
        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.5f);
            loadingScreen.SetActive(false);
        }
    }
}
    

