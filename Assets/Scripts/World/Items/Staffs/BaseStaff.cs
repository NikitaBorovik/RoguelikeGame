using App.Systems;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace App.World.Items.Staffs
{
    public abstract class BaseStaff : MonoBehaviour
    {
        protected ObjectPool objectPool;
        #region Serialized Fields
        [SerializeField]
        private WeakShootEvent shootEvent;
        [SerializeField]
        private Transform shootPosition;
        [SerializeField]
        private StaffSO data;
        #endregion

        #region Sounds
        protected AudioSource audioSource;
        protected AudioClip shootSound;
        #endregion

        #region Properties
        public WeakShootEvent ShootEvent { get => shootEvent; }
        public Transform ShootPosition { get => shootPosition; set => shootPosition = value; }
        public StaffSO Data { get => data; set => data = value; }
        #endregion
        private void OnEnable()
        {
            ShootEvent.OnShoot += Shoot;
        }

        private void OnDisable()
        {
            ShootEvent.OnShoot -= Shoot;
        }
        public void Shoot(WeakShootEvent ev)
        {
            Shoot();
        }
        public abstract void Shoot();
        protected virtual void Awake()
        {
            //audioSource = GetComponent<AudioSource>();
            //shootSound = Data.shootSound;
            objectPool = FindObjectOfType<ObjectPool>();
        }
    }

}
