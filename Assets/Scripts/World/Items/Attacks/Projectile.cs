using App.Systems;
using App.World.Creatures;
using App.World.Creatures.PlayerScripts.Components;
using UnityEngine;

namespace App.World.Items.Attacks
{
    public class Projectile : MonoBehaviour, IObjectPoolItem
    {
        #region parameters
        private float spread;
        private float damage;
        private float speed;
        private float pearcingCount;
        private float manacost;
        private bool isFlying = false;
        #endregion

        #region serialized parameters
        [SerializeField]
        private ProjectileSO projectileData;
        [SerializeField]
        private string poolObjectType;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Rigidbody2D rb;
        [SerializeField]
        private PolygonCollider2D polygonCollider;
        [SerializeField]
        private Sprite treasureSprite;
        #endregion

        #region connected objects
        private ObjectPool objectPool;
        private Player player;
        private Shoot shoot;
        private TrailRenderer trailRenderer;
        #endregion

        public string PoolObjectType => poolObjectType;

        public Sprite TreasureSprite { get => treasureSprite; set => treasureSprite = value; }
        public float Damage { get => damage; set => damage = value; }
        public float Spread { get => spread; set => spread = value; }
        public float Speed { get => speed; set => speed = value; }
        public float Manacost { get => manacost; set => manacost = value; }
        public ProjectileSO ProjectileData { get => projectileData; set => projectileData = value; }

        protected virtual void Update()
        {
            if (!isFlying)
            {
                transform.position = player.ShootPosition.position;
                Quaternion rotation = Quaternion.Euler(player.ShootPosition.eulerAngles.x, player.ShootPosition.eulerAngles.y, player.ShootPosition.eulerAngles.z /*+ spread*/);
                transform.rotation = rotation;
            }
        }
        protected virtual void Awake()
        {
            trailRenderer = GetComponent<TrailRenderer>();
            Damage = ProjectileData.damage;
            Speed = ProjectileData.speed;
            pearcingCount = ProjectileData.pearcingCount;
            manacost = ProjectileData.manacost;
            Debug.Log("Manacost" + manacost);
        }
        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (!gameObject.activeSelf)
                return;
            if (collision.gameObject.layer != LayerMask.NameToLayer("EnemyHitbox"))
            {
                objectPool.ReturnToPool(this);
                return;
            }
            HealthStatus targetHealt = collision.GetComponentInParent<HealthStatus>();
            if (targetHealt == null)
            {
                return;
            }
            
            targetHealt.TakeDamage(ProjectileData.damage);
            if (pearcingCount > 0)
            {
                ProjectileData.pearcingCount--;
            }
            else
            {
                objectPool.ReturnToPool(this);
            }

        }
        public virtual void Init(Player player)
        {
            GetComponent<TimeToLive>().Init();
            this.player = player;
            shoot = player.GetComponent<Shoot>();
            shoot.CanShoot = false;
            isFlying = false;
            animator.Play("Base Layer.Spawn");
        }

        protected virtual void StartFlying()
        {
            animator.Play("Base Layer.Rest");
            player.PAnimator.SetBool("isAttacking", false);
            isFlying = true;
            polygonCollider.enabled = true;
            shoot.CanShoot = true;
            trailRenderer.enabled = true;
            
            Quaternion rotation = Quaternion.Euler(player.ShootPosition.eulerAngles.x, player.ShootPosition.eulerAngles.y, player.ShootPosition.eulerAngles.z /*+ spread*/);
            transform.rotation = rotation;
            rb.velocity = transform.right * ProjectileData.speed;
           
        }

        public void GetFromPool(ObjectPool pool)
        {
            objectPool = pool;
            gameObject.SetActive(true);
           trailRenderer?.Clear();
        }

        public void ReturnToPool()
        {
            polygonCollider.enabled = false;
            gameObject.SetActive(false);
            trailRenderer.enabled = false;
        }

        public GameObject GetGameObject()
        {
            return (gameObject);
        }
    }
}

