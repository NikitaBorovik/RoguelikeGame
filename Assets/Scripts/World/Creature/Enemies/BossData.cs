using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.Enemies
{
    [CreateAssetMenu(fileName = "BossDataSO", menuName = "Scriptable Objects/Enemies/ Boss Data")]
    public class BossData : ScriptableObject
    {
        public List<BaseEnemy> bosses;
        public List<BaseEnemy> minions;
    }
}

