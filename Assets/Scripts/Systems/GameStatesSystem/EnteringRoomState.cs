using App.Systems.Spawning;
namespace App.Systems.GameStates
{
    public class EnteringRoomState : IState
    {
        private GameStatesSystem gameStatesSystem;
        private SpawningSystem spawningSystem;
        private int curLevel;
        public int CurLevel { get => curLevel; set => curLevel = value; }

        public EnteringRoomState(GameStatesSystem gameStatesSystem, SpawningSystem spawningSystem, int curLevel)
        {
            this.gameStatesSystem = gameStatesSystem;
            this.spawningSystem = spawningSystem;
            this.CurLevel = curLevel;
        }

        

        public void Enter()
        {
            spawningSystem.Spawn(CurLevel);
            gameStatesSystem.CurRoom.DrawnRoom.Close();
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }
}

