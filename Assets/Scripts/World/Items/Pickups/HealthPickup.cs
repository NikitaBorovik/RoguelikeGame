using App.Systems;
using App.World.Creatures.PlayerScripts.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : BasePickup
{
    [SerializeField]
    private float healing;
    public override string PoolObjectType => "healing potion";

    public override void Init(Vector3 position, ObjectPool objectPool)
    {
        base.Init(position, objectPool);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null&&
            collision.gameObject.GetComponentInParent<Player>() == null)
            return;
        collision.gameObject.GetComponentInParent<Player>().Health.Heal(healing);
        objectPool.ReturnToPool(this);
    }
}
