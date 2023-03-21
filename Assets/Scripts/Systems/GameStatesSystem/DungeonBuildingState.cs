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

        public LevelModel LevelToBuild { get => levelToBuild; set => levelToBuild = value; }
        public DungeonBuildingState(GameStatesSystem gameStatesSystem,DungeonGenerator dungeonBuilder,LevelModel level)
        {
            this.gameStatesSystem = gameStatesSystem;
            this.dungeonBuilder = dungeonBuilder;
            this.LevelToBuild = level;
        }

       

        public void Enter()
        {
            if (dungeonBuilder.GenerateDungeon(LevelToBuild)) ;
            
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            
        }
    }
}
    

