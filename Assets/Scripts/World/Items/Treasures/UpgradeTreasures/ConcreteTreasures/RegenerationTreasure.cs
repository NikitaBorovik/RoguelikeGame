using App.World.Creatures.PlayerScripts.Components;
using UnityEngine;

namespace App.World.Items.Treasures.UpgradeTreasures.ConcreteTreasures
{
    [CreateAssetMenu(fileName = "HPRegenRateUp", menuName = "Scriptable Objects/Upgrades/RegenerationTreasureUpgrade")]
    public class RegenerationTreasure : BaseUpgradeTreasure
    {
        #region Serialized Fields
        [SerializeField] 
        private float hpRegenRateAddent;
        [SerializeField]
        private float additionalHP;
        #endregion

        #region Non-serialized Fields
        private float timeCounter = 0.0f;
        private const float period = 1.0f;
        #endregion

        #region Overriden methods
        protected override void Upgrade(Player upgradable) 
        {
            var health = upgradable.Health;
            health.MaxHealth += additionalHP;
            health.HealToMax();
        }

        protected override void UpdateIfEnabled(Player upgradable)
        {
            var health = upgradable.Health;
            if (timeCounter > period)
            {
                health.Heal(hpRegenRateAddent);
                timeCounter = 0f;
                Debug.Log("RegenerationTreasure: " + health.CurrentHealth);
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }

        protected override void Degrade(Player upgradable) 
        {
            var health = upgradable.Health;
            health.MaxHealth -= additionalHP;
        }

        #endregion
    }

}

