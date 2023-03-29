using App.Systems;
using App.World.Items.Attacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Treasures
{
    public class ItemTreasure : BaseTreasure
    {
        [SerializeField]
        private List<BaseUpgradeTreasure> upgrades;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private BaseUpgradeTreasure upgrade;

        public override string PoolObjectType => "ItemTreasure";

        public override void Obtain(ObtainEvent ev)
        {
            
            var item = Instantiate(upgrade);
            player.GetComponent<UpgradeTreasureManager>().AddUpgrade(item);
            ReturnToPool();
        }

        public void SetRandomUpgrade()
        {
            int randomIndex = Random.Range(0, upgrades.Count);
            upgrade = upgrades[randomIndex];
            upgrades.Remove(upgrade);
            spriteRenderer.sprite = upgrade.sprite;
        }

        protected override void Start()
        {
            //RemoveExistingPower();
            base.Start();
            //SetRandomUpgrade();
        }

        public override void GetFromPool(ObjectPool pool)
        {
            base.GetFromPool(pool);
            SetRandomUpgrade();
        }
    }
}

