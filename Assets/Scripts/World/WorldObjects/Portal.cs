using App.Systems;
using App.Systems.GameStates;
using UnityEngine;

namespace App.World.WorldObjects
{
    public class Portal : MonoBehaviour, IObjectPoolItem
    {
        private GameStatesSystem gameStatesSystem;
        private ObjectPool objectPool;
        [SerializeField]
        private string poolObjectType;

        public string PoolObjectType => poolObjectType;

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
        private void Awake()
        {
            objectPool = FindObjectOfType<ObjectPool>();
            gameStatesSystem = FindObjectOfType<GameStatesSystem>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision.name);
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerCollider"))
            {
                gameStatesSystem.StageCleared();
                objectPool.ReturnToPool(this);
            }
        }
    }
}

