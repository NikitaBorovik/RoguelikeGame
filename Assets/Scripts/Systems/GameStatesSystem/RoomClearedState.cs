using App;
using App.Systems.GameStates;

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
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}
