using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.PlayerScripts.Components
{
    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/Player/ Player Data")]
    public class PlayerDataSO : ScriptableObject
    {
        public float speed;
        public float maxHealth;
        public float maxMana;

    }
}
