using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Staffs
{
    [CreateAssetMenu(fileName = "StaffDataSO", menuName = "Scriptable Objects/Weapons/ Projectile Staff Data")]
    public class ProjectileStaffSO : StaffSO
    {
        public float coolDown;
        public float projectileFlySpeed;
        public float projectileSpread;
        public int pearcingCount;
        public GameObject projectilePrefab;
        public int projectileCount;
    }

}

