using App.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePickup : MonoBehaviour, IObjectPoolItem
{
    private Rigidbody2D rigidBody;
    protected ObjectPool objectPool;

    public abstract string PoolObjectType { get; }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public virtual void Init(Vector3 position, ObjectPool objectPool)
    {
        transform.position = position;
        Drop();
    }

    public void Drop()
    {
        float xDirection = Random.Range(-1f, 1f);
        float yDirection = Random.Range(-1f, 1f);
        float speed = Random.Range(10f, 30f);
        Vector2 dropDirection = new Vector2(xDirection, yDirection).normalized;
        rigidBody.velocity = new Vector3(dropDirection.x * speed, dropDirection.y * speed, 0f);
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
        return gameObject;
    }
}
