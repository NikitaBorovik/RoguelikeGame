using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Staffs
{
    public class ProjectileStaff : BaseStaff
    {
        #region Fields
        private float damage;
        private float timeFromCoolDown;
        private float coolDown;
        private float weakProjectileFlySpeed;
        private float weakProjectileSpread;
        private int weakPearcingCount;
        private GameObject weakProjectilePrefab;
        private float strongProjectileFlySpeed;
        private float strongProjectileSpread;
        private int strongPearcingCount;
        private GameObject strongProjectilePrefab;
        private int strongProjectileCount;
        private int weakProjectileCount;
        private float weakProjectileDamage;
        private float strongProjectileDamage;
        #endregion

        public override void ShootWeak()
        {
            //if (timeFromCoolDown > coolDown)
            //{
            //    Projectile projectile = weakProjectilePrefab.GetComponent<Projectile>();
            //    if (projectile == null)
            //    {
            //        Debug.Log("Trying to shoot bullet that doesn't contain Bullet script");
            //        return;
            //    }
            //    for (int i = 0; i < weakProjectileCount; i++)
            //    {
            //        float spread = Random.Range(-weakProjectileSpread, weakProjectileSpread);
            //        Quaternion rotation = Quaternion.Euler(ShootPosition.eulerAngles.x, ShootPosition.eulerAngles.y, ShootPosition.eulerAngles.z + spread);
            //        GameObject bullet = objectPool.GetObjectFromPool(projectile.PoolObjectType, weakProjectilePrefab, ShootPosition.position).GetGameObject();
            //        bullet.transform.rotation = rotation;
            //        bullet.transform.position = ShootPosition.position;
            //        bullet.GetComponent<Projectile>().Init(weakProjectileDamage, weakPearcingCount);
            //        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * weakProjectileFlySpeed;
            //    }
            //    //audioSource.PlayOneShot(shootSound);
            //    timeFromCoolDown = 0.0f;
            //}
        }
        public override void ShootStrong()
        {
            //if (timeFromCoolDown > coolDown)
            //{
            //    Projectile projectile = strongProjectilePrefab.GetComponent<Projectile>();
            //    if (projectile == null)
            //    {
            //        Debug.Log("Trying to shoot bullet that doesn't contain Bullet script");
            //        return;
            //    }
            //    for (int i = 0; i < strongProjectileCount; i++)
            //    {
            //        float spread = Random.Range(-strongProjectileSpread, strongProjectileSpread);
            //        Quaternion rotation = Quaternion.Euler(ShootPosition.eulerAngles.x, ShootPosition.eulerAngles.y, ShootPosition.eulerAngles.z + spread);
            //        GameObject bullet = objectPool.GetObjectFromPool(projectile.PoolObjectType, strongProjectilePrefab, ShootPosition.position).GetGameObject();
            //        bullet.transform.rotation = rotation;
            //        bullet.transform.position = ShootPosition.position;
            //        bullet.GetComponent<Projectile>().Init(strongProjectileDamage, strongPearcingCount);
            //        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * strongProjectileFlySpeed;
            //    }
            //    //audioSource.PlayOneShot(shootSound);
            //    timeFromCoolDown = 0.0f;
            //}
        }
        private void Update()
        {
            timeFromCoolDown += Time.deltaTime;
        }
        protected override void Awake()
        {
            base.Awake();
            this.coolDown = (Data as ProjectileStaffSO).coolDown;
            this.weakProjectileFlySpeed = (Data as ProjectileStaffSO).weakProjectileFlySpeed;
            this.weakProjectileSpread = (Data as ProjectileStaffSO).weakProjectileSpread;
            this.weakPearcingCount = (Data as ProjectileStaffSO).weakPearcingCount;
            this.weakProjectilePrefab = (Data as ProjectileStaffSO).weakProjectilePrefab;
            this.weakProjectileCount = (Data as ProjectileStaffSO).weakProjectileCount;
            this.strongProjectileFlySpeed = (Data as ProjectileStaffSO).strongProjectileFlySpeed;
            this.strongProjectileSpread = (Data as ProjectileStaffSO).strongProjectileSpread;
            this.strongPearcingCount = (Data as ProjectileStaffSO).strongPearcingCount;
            this.strongProjectilePrefab = (Data as ProjectileStaffSO).strongProjectilePrefab;
            this.strongProjectileCount = (Data as ProjectileStaffSO).strongProjectileCount;
            this.weakProjectileDamage = (Data as ProjectileStaffSO).weakProjectileDamage;
            this.strongProjectileDamage = (Data as ProjectileStaffSO).strongProjectileDamage;
            timeFromCoolDown = coolDown;
        }

        
    }
    
}

