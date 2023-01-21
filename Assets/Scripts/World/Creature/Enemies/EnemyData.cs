using System.Collections.Generic;
using UnityEngine;
namespace App.World.Creatures.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Enemies/ Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string type;
        public int maxHealth;
        public int dangerLevel;
        public float speed;
        public float damage;
        public float timeBetweenAttacks;
        public float moneyDropChance;
        public float healingDropChance;
        public float attackRange;
        public float spawnAnimationDuration;
    }
}