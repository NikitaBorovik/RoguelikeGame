using App.Systems;
using App.World;
using App.World.Creatures.PlayerScripts.Components;
using UnityEngine;

namespace App.World.Items.Treasures
{
    public abstract class BaseTreasure : MonoBehaviour, IObjectPoolItem
    {
        protected ObjectContainer container;

        [SerializeField]
        protected AudioClip obtainSound;

        protected ObjectPool objectPool;

        protected Player player;

        protected AudioSource audioSource;

        public virtual string PoolObjectType => throw new System.NotImplementedException();

        protected virtual void Awake()
        {

            // audioSource = GetComponent<AudioSource>();
        }

        protected virtual void Start()
        {
            container = FindObjectOfType<ObjectContainer>();
            Debug.Log(container);
            player = container.Player.GetComponent<Player>();
            objectPool = FindObjectOfType<ObjectPool>();
        }
        public abstract void Obtain(ObtainEvent ev);


        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            player.ObtainEvent.OnObtain += this.Obtain;
        }
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            player.ObtainEvent.OnObtain -= this.Obtain;
        }

        public virtual void GetFromPool(ObjectPool pool)
        {
            objectPool = pool;
            gameObject.SetActive(true);
        }

        public virtual void ReturnToPool()
        {
            gameObject.SetActive(false);
        }

        public virtual GameObject GetGameObject()
        {
            return (gameObject);
        }
    }
}