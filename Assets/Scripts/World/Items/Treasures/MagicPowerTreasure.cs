using App.Systems;
using App.World.Creatures.PlayerScripts.Components;
using App.World.Items.Attacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Treasures
{
    public class MagicPowerTreasure : BaseTreasure
    {
        [SerializeField]
        private List<Projectile> powers;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private Projectile projectile;

        public override string PoolObjectType => "MagicPowerTreasure";

        public override void Obtain(ObtainEvent ev)
        {
            var manager = player.GetComponent<UpgradeTreasureManager>();
            manager.DisableAll();
            player.Powers.Add(projectile);
            manager.EnableAll();
            ReturnToPool();
        }

        public void SetRandomPower()
        {
            int randomIndex = Random.Range(0, powers.Count);
            projectile = powers[randomIndex];
            powers.Remove(projectile);
            spriteRenderer.sprite = projectile.TreasureSprite;

        }

        protected override void Start()
        {
            base.Start();
        }


        public override void GetFromPool(ObjectPool pool)
        {
            base.GetFromPool(pool);
            SetRandomPower();
        }
    }
}