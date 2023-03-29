using App.Systems;
using App.World.Items.Attacks;
using UnityEngine;

namespace App.World.Creatures.Enemies
{
    public class EnemyProjectile : MonoBehaviour , IObjectPoolItem
    {
        private float spread;
        private float damage;
        [SerializeField]
        private string poolObjectType;
        [SerializeField]
        private Rigidbody2D rb;
        [SerializeField]
        private PolygonCollider2D polygonCollider;
        [SerializeField]
        private TrailRenderer trailRenderer;
        private ObjectPool objectPool;
        

        public string PoolObjectType => poolObjectType;

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

        private void Awake()
        {
            objectPool = FindObjectOfType<ObjectPool>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!gameObject.activeSelf)
                return;
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                objectPool.ReturnToPool(this);
                return;
            }
            else
            {
                HealthStatus collisionHealth = collision.GetComponentInParent<HealthStatus>();
               
                if (collisionHealth != null)
                {
                    collisionHealth.TakeDamage(damage);
                    //int index = Random.Range(0, hitSounds.Count);
                    //audioSource.PlayOneShot(hitSounds[index]);
                }
            }
            objectPool.ReturnToPool(this);
        }

        public void Init(Vector3 direction, float damage, float speed)
        {
            GetComponent<TimeToLive>().Init();
            this.damage = damage;
            var velocity = direction.normalized * speed;
            rb.velocity = velocity;
            polygonCollider.enabled = true;
            trailRenderer.enabled = true;
        }
    }

}
