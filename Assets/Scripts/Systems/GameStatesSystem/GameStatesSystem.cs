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
        [SerializeField]
        private List<LevelModel> levels;
        [SerializeField]
        private int curLevel = 0;


        public void Init(DungeonGenerator dungeonGenerator)
        {
            dungeonBuilder = dungeonGenerator;
            gameStateMachine = new StateMachine();
            dungeonBuildingState = new DungeonBuildingState(this, dungeonBuilder, levels[curLevel]);
            gameStateMachine.Initialize(dungeonBuildingState);
        }
        private void Update()
        {
           // gameStateMachine.CurrentState.Update();
        }
    }
}

