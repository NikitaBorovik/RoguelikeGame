using App.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Systems.GameStates
{
    public class GameEndedState : IState
    {
        private GameStatesSystem gameStatesSystem;
        private DeathScreenController deathScreenController;

        public GameEndedState(GameStatesSystem gameStatesSystem, DeathScreenController deathScreenController)
        {
            this.gameStatesSystem = gameStatesSystem;
            this.deathScreenController = deathScreenController;
        }

        public void Enter()
        {
            deathScreenController.Appear();
        }

        public void Exit()
        {
        }

        void IState.Update()
        {
        }
    }

}
