using App.World.Creatures.PlayerScripts.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Treasures.UpgradeTreasures.ConcreteTreasures
{
    [CreateAssetMenu(fileName = "ManaRestore", menuName = "Scriptable Objects/Upgrades/ManaRestoreTreasureUpgrade")]
    public class ManaRestoreTreasure : BaseUpgradeTreasure
    {
        #region Serialized Fields
        [SerializeField] 
        private float manaRegenRateAddent;
        [SerializeField]
        private float additionalMana;
        #endregion

        #region Overriden methods
        protected override void Upgrade(Player upgradable) 
        {
            var mana = upgradable.Mana;
            mana.MaxMana += additionalMana;
            mana.ManaRegenRate += manaRegenRateAddent;
            mana.RestoreToMax();
        }

        protected override void UpdateIfEnabled(Player upgradable)
        {
        }

        protected override void Degrade(Player upgradable) 
        {
            var mana = upgradable.Mana;
            mana.MaxMana -= additionalMana;
            mana.ManaRegenRate -= manaRegenRateAddent;
        }

        #endregion
    }

}

