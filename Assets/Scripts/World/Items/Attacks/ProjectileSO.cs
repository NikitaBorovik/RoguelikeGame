using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Attacks
{
    [CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/Player/Projectile data")]
    public class ProjectileSO : ScriptableObject
    {
        public float damage;
        public float pearcingCount;
        public float speed;

    }
}

