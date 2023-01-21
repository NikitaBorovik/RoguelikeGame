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
        public DungeonBuildingState(GameStatesSystem gameStatesSystem,DungeonGenerator dungeonBuilder,LevelModel level)
        {
            this.gameStatesSystem = gameStatesSystem;
            this.dungeonBuilder = dungeonBuilder;
            this.levelToBuild = level;
        }
        public void Enter()
        {
            if (dungeonBuilder.GenerateDungeon(levelToBuild)) ;
            
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            
        }
    }
}
    

