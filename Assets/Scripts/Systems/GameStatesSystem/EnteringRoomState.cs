using App.Systems.Spawning;
namespace App.Systems.GameStates
{
    public class EnteringRoomState : IState
    {
        private GameStatesSystem gameStatesSystem;
        private SpawningSystem spawningSystem;

        public EnteringRoomState(GameStatesSystem gameStatesSystem, SpawningSystem spawningSystem)
        {
            this.gameStatesSystem = gameStatesSystem;
            this.spawningSystem = spawningSystem;
        }
        public void Enter()
        {
            spawningSystem.Spawn();
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            
        }
    }
}

