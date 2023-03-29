using App.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Treasures
{
    public class Pedestal : MonoBehaviour
    {
        [SerializeField]
        private MagicPowerTreasure magicPowerTreasure;

        [SerializeField]
        private ItemTreasure itemTreasure;

        [SerializeField]
        private Transform treasurePoint;

        private BaseTreasure treasure;
        private void Awake()
        {
            ObjectPool objectPool = FindObjectOfType<ObjectPool>();
            bool treasureType = Random.Range(1, 1) == 0;
            if (treasureType)
            {
                GameObject treasureGO = objectPool.GetObjectFromPool(magicPowerTreasure.PoolObjectType, magicPowerTreasure.gameObject, transform.position,transform).GetGameObject();
                treasureGO.transform.position = treasurePoint.position;
               // treasure = treasureGO.GetComponent<BaseTreasure>();
            }
            else
            {
                GameObject treasureGO = objectPool.GetObjectFromPool(itemTreasure.PoolObjectType, itemTreasure.gameObject, transform.position, transform).GetGameObject();
                treasureGO.transform.position = treasurePoint.position;
            }
        }
    }
}
