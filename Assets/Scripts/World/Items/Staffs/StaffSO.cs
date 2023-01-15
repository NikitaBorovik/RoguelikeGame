using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Staffs
{
    public class StaffSO : ScriptableObject
    {
        public float damage;
        public AudioClip shootSound;
        public GameObject staffPrefab;
        public Sprite staffSprite;
    }
}
