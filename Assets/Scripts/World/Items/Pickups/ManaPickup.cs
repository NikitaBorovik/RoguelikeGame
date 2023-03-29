using App.Systems;
using App.World.Creatures.PlayerScripts.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickup : BasePickup
{
    [SerializeField]
    private float restoration;

    public override string PoolObjectType => "mana potion";

    public override void Init(Vector3 position, ObjectPool objectPool)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        base.Init(position, objectPool);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Player>().Mana.Restore(restoration);
        objectPool.ReturnToPool(this);
    }
}
