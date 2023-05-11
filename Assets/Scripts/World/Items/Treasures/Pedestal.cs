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
        
        [SerializeField]
        private GameObject textHintArea;

        [SerializeField]
        private bool withWeapon;
        private GameObject treasureGO;

        private void Update()
        {
            if(!treasureGO.activeSelf)
                textHintArea.SetActive(false);
        }

        private void Awake()
        {
            ObjectPool objectPool = FindObjectOfType<ObjectPool>();
            if (withWeapon)
            {
                treasureGO = objectPool.GetObjectFromPool(magicPowerTreasure.PoolObjectType, magicPowerTreasure.gameObject, transform.position,transform).GetGameObject();
                treasureGO.transform.position = treasurePoint.position;
            }
            else
            {
                treasureGO = objectPool.GetObjectFromPool(itemTreasure.PoolObjectType, itemTreasure.gameObject, transform.position, transform).GetGameObject();
                treasureGO.transform.position = treasurePoint.position;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                textHintArea.SetActive(true);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                textHintArea.SetActive(false);
            }
        }
    }
}
