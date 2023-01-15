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
        private WeakShootEvent weakShootEvent;
        [SerializeField]
        private StrongShootEvent strongShootEvent;
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
        public WeakShootEvent WeakShootEvent { get => weakShootEvent; }

        public StrongShootEvent StrongShootEvent { get => strongShootEvent; }
        public Transform ShootPosition { get => shootPosition; set => shootPosition = value; }
        public StaffSO Data { get => data; set => data = value; }
        #endregion
        private void OnEnable()
        {
            WeakShootEvent.OnShoot += ShootWeak;
            StrongShootEvent.OnShoot += ShootStrong;
        }

        private void OnDisable()
        {
            WeakShootEvent.OnShoot -= ShootWeak;
            StrongShootEvent.OnShoot -= ShootStrong;
        }
        public void ShootWeak(WeakShootEvent ev)
        {
            ShootWeak();
        }
        public void ShootStrong(StrongShootEvent ev)
        {
            ShootStrong();
        }
        public abstract void ShootWeak();
        public abstract void ShootStrong();

        protected virtual void Awake()
        {
            //audioSource = GetComponent<AudioSource>();
            //shootSound = Data.shootSound;
            objectPool = FindObjectOfType<ObjectPool>();
        }
    }

}
