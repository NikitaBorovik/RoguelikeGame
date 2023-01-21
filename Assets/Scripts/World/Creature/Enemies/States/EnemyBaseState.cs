
namespace App.World.Creatures.Enemies.States
{
    public class EnemyBaseState : IState
    {
        protected BaseEnemy baseEnemy;
        protected StateMachine stateMachine;
        public EnemyBaseState(BaseEnemy baseEnemy, StateMachine stateMachine)
        {
            this.baseEnemy = baseEnemy;
            this.stateMachine = stateMachine;
        }
        public virtual void Enter() { }

        public virtual void Update() { }

        public virtual void Exit() { }
    }
}

