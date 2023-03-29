using App.World.Creatures.PlayerScripts.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Treasures.UpgradeTreasures.ConcreteTreasures
{
    [CreateAssetMenu(fileName = "SpeedUp", menuName = "Scriptable Objects/Upgrades/SpeedUpTreasureUpgrade")]
    public class SpeedUpTreasure : BaseUpgradeTreasure
    {
        #region Serialized Fields
        [SerializeField] private float speedMultiplier;
        #endregion

        #region Overriden methods
        protected override void Upgrade(Player upgradable)
        {
            upgradable.MovementSpeed *= speedMultiplier;
        }

        protected override void Degrade(Player upgradable)
        {
            upgradable.MovementSpeed /= speedMultiplier;
        }

        protected override void UpdateIfEnabled(Player upgradable) { }

        #endregion
    }
}

