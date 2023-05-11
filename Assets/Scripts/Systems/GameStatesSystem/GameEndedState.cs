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
        private bool isVictory = false;

        public GameEndedState(GameStatesSystem gameStatesSystem, DeathScreenController deathScreenController)
        {
            this.gameStatesSystem = gameStatesSystem;
            this.deathScreenController = deathScreenController;
        }

        public bool IsVictory { get => isVictory; set => isVictory = value; }

        public void Enter()
        {
            if (IsVictory)
            {
                deathScreenController.Text.SetText("You won! Game over");
            }
            else
            {
                deathScreenController.Text.SetText("You died! Game over");
            }
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
