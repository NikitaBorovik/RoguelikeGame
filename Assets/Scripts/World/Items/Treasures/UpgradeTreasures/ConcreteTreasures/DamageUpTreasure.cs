using App.World.Creatures.PlayerScripts.Components;
using UnityEngine;

namespace App.World.Items.Treasures.UpgradeTreasures.ConcreteTreasures
{
    [CreateAssetMenu(fileName = "DamageUp", menuName = "Scriptable Objects/Upgrades/DamageUpTreasureUpgrade")]
    public class DamageUpTreasure : BaseUpgradeTreasure
    {
        #region Serialized Fields
        [SerializeField] private float damageMultiplier;
        #endregion

        #region Overriden methods
        protected override void Upgrade(Player upgradable)
        {
            foreach (var power in upgradable.Powers)
            {
                power.Damage *= damageMultiplier;
            }
        }

        protected override void Degrade(Player upgradable)
        {
            foreach (var power in upgradable.Powers)
            {
                power.Damage /= damageMultiplier;
            }
        }

        protected override void UpdateIfEnabled(Player upgradable) { }

        #endregion
    }
}
