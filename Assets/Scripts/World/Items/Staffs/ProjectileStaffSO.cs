using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Staffs
{
    [CreateAssetMenu(fileName = "StaffDataSO", menuName = "Scriptable Objects/Weapons/ Projectile Staff Data")]
    public class ProjectileStaffSO : StaffSO
    {
        public float coolDown;
        public float weakProjectileFlySpeed;
        public float weakProjectileSpread;
        public int weakPearcingCount;
        public GameObject weakProjectilePrefab;
        public GameObject strongProjectilePrefab;
        public float strongProjectileFlySpeed;
        public float strongProjectileSpread;
        public int strongPearcingCount;
        public int strongProjectileCount;
        public int weakProjectileCount;
        public float weakProjectileDamage;
        public float strongProjectileDamage;
    }

}

