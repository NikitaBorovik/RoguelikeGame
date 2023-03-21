using System.Collections.Generic;
using UnityEngine;
namespace App.World.Creatures.Enemies
{
    [CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/Enemies/ Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string attackType;
        public string poolObjectType;
        public int maxHealth;
        public float speed;
        public float projectileSpeed;
        public int projectileCount;
        public float damage;
        public float timeBetweenAttacks;
        public float healingDropChance;
        public float attackRange;
    }
}