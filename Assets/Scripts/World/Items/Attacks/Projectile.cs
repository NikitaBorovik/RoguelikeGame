using App.Systems;
using App.World.Creatures;
using App.World.Creatures.PlayerScripts.Components;
using UnityEngine;

namespace App.World.Items.Attacks
{
    public class Projectile : MonoBehaviour, IObjectPoolItem
    {
        
        protected ProjectileSO projectileData;
        protected ObjectPool objectPool;
        [SerializeField]
        protected string poolObjectType;
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected Rigidbody2D rb;
        [SerializeField]
        protected CircleCollider2D circleCollider;
        protected Player player;
        protected Shoot shoot;
        protected bool isFlying = false;
        protected TrailRenderer trailRenderer;
        public string PoolObjectType => poolObjectType;

        private void Update()
        {
            if (!isFlying)
            {
                transform.position = player.ShootPosition.position;
            }
        }
        private void Awake()
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }
        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (!gameObject.activeSelf)
                return;
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            {
                objectPool.ReturnToPool(this);
                return;
            }
            HealthStatus targetHealt = collision.GetComponent<HealthStatus>();
            if (targetHealt == null)
            {
                return;
            }
            
            targetHealt.TakeDamage(80/*projectileData.damage*/);
            //if (projectileData.pearcingCount > 0)
            //{
            //    projectileData.pearcingCount--;
            //}
            //else
            //{
            //    objectPool.ReturnToPool(this);
            //}
            objectPool.ReturnToPool(this);

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
        
        public void StartFlying()
        {
            isFlying = true;
            circleCollider.enabled = true;
            Quaternion rotation = Quaternion.Euler(player.ShootPosition.eulerAngles.x, player.ShootPosition.eulerAngles.y, player.ShootPosition.eulerAngles.z /*+ spread*/);
            transform.rotation = rotation;
            rb.velocity = transform.right * 10;//projectileData.speed;
            shoot.CanShoot = true;
            trailRenderer.enabled = true;
            player.PAnimator.SetBool("isAttacking", false);
        }

        public void GetFromPool(ObjectPool pool)
        {
            objectPool = pool;
            gameObject.SetActive(true);
            GetComponent<TrailRenderer>()?.Clear();
        }

        public void ReturnToPool()
        {
            circleCollider.enabled = false;
            gameObject.SetActive(false);
            trailRenderer.enabled = false;
        }

        public GameObject GetGameObject()
        {
            return (gameObject);
        }
    }
}

