using App;
using App.Systems.GameStates;
using UnityEngine;

public class RoomClearedState : IState
{
    private GameStatesSystem gameStatesSystem;


    public RoomClearedState(GameStatesSystem gameStatesSystem)
    {
        this.gameStatesSystem = gameStatesSystem;
        
    }
    public void Enter()
    {
        
        gameStatesSystem.CurRoom.DrawnRoom.Open();
        gameStatesSystem.ChangeGameState(gameStatesSystem.DungeonExploringState);
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}
