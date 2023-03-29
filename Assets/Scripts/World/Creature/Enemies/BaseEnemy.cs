using App.Systems;
using App.Systems.Spawning;
using App.World.Creatures.Enemies.States;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace App.World.Creatures.Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IKillable, IObjectPoolItem
    {
        private bool initialised;
        private Transform target;
        private FollowState followState;
        private SpawningState spawningState;
        private SeparateState separateState;
        private DieState dieState;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        
        
        private Room currentRoom;
        [SerializeField]
        private AStarPathfinding astarPathfinding;
        [SerializeField]
        private Rigidbody2D myRigidbody;
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        protected EnemyData enemyData;
        [SerializeField]
        protected List<Collider2D> myColliders;
        [SerializeField]
        private HealthStatus health;
        private INotifyEnemyDied notifieble;

        protected StateMachine stateMachine;
        protected EnemyBaseState attackState;
        protected ObjectPool objectPool;

        public Transform Target => target;
        public AStarPathfinding Pathfinding => astarPathfinding;
        public Rigidbody2D MyRigidbody => myRigidbody;
        public EnemyData EnemyData => enemyData;
        public FollowState FollowState => followState;
        public EnemyBaseState AttackState => attackState;
        public Animator Animator => animator;
        public List<Collider2D> MyColliders => myColliders;
        public SpriteRenderer SpriteRenderer => spriteRenderer;
        public AudioSource AudioSource => audioSource; 
        public Room CurrentRoom => currentRoom;
        public virtual string PoolObjectType => enemyData.poolObjectType;

        public SpawningState SpawningState { get => spawningState; set => spawningState = value; }
        public SeparateState SeparateState { get => separateState; set => separateState = value; }

        public virtual void Awake()
        {
            initialised = false; 
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            stateMachine = new StateMachine();
            followState = new FollowState(this, stateMachine);
            SpawningState = new SpawningState(this, stateMachine,Animator);
            SeparateState = new SeparateState(this, stateMachine);
            dieState = new DieState(this, stateMachine);
           // stateMachine.Initialize(spawningState);
        }

        void Update()
        {
            
            if (initialised)
                stateMachine.CurrentState.Update();
        }

        public virtual void Init(Vector3 position, Transform target, float hpMultiplier, Room currentRoom, INotifyEnemyDied notifieble)
        {
            this.target = target;
            transform.position = position;
            this.currentRoom = currentRoom;
            health.MaxHealth = enemyData.maxHealth * hpMultiplier;
            health.HealToMax();
            initialised = true;
            this.notifieble = notifieble;
            if (stateMachine.CurrentState == null)
            {
                stateMachine.Initialize(SpawningState);
            }
            else
                stateMachine.ChangeState(SpawningState);
        }

        

        public void Die()
        {
            if (stateMachine.CurrentState != dieState)
            {
                StopAllCoroutines();
                DropHealing();
                stateMachine.ChangeState(dieState);
                notifieble.NotifyEnemyDied();
            }
        }

        private void DropHealing()
        {
            if (Random.value <= enemyData.healingDropChance)
            {
                GameObject healing = objectPool.GetObjectFromPool(enemyData.healthPickupPrefab.PoolObjectType, enemyData.healthPickupPrefab.gameObject, transform.position).GetGameObject();
                healing.GetComponent<HealthPickup>().Init(transform.position, objectPool);

            }
        }

        public void DyingSequence()
        {
            objectPool.ReturnToPool(this);
        }

        public void GetFromPool(ObjectPool pool)
        {
            objectPool = pool;
            gameObject.SetActive(true);
        }

        public void ReturnToPool()
        {
            gameObject.SetActive(false);
        }

        public GameObject GetGameObject()
        {
            return (gameObject);
        }
    }
}

