using App.Systems;
using App.World.Creatures;
using App.World.Creatures.PlayerScripts.Components;
using UnityEngine;

namespace App.World.Items.Attacks
{
    public class Projectile : MonoBehaviour, IObjectPoolItem
    {
        #region parameters
        protected float spread;
        protected float damage;
        protected float speed;
        protected float pearcingCount;
        protected bool isFlying = false;
        #endregion

        #region serialized parameters
        [SerializeField]
        protected ProjectileSO projectileData;
        [SerializeField]
        protected string poolObjectType;
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected Rigidbody2D rb;
        [SerializeField]
        protected PolygonCollider2D polygonCollider;
        #endregion

        #region connected objects
        protected ObjectPool objectPool;
        protected Player player;
        protected Shoot shoot;
        protected TrailRenderer trailRenderer;
        #endregion

        public string PoolObjectType => poolObjectType;

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
            damage = projectileData.damage;
            speed = projectileData.speed;
            pearcingCount = projectileData.pearcingCount;
            
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
            
            targetHealt.TakeDamage(projectileData.damage);
            if (pearcingCount > 0)
            {
                projectileData.pearcingCount--;
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
            rb.velocity = transform.right * projectileData.speed;
           
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

