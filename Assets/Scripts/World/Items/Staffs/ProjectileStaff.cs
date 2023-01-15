using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Staffs
{
    public class ProjectileStaff : BaseStaff
    {
        private float damage;
        private float timeFromCoolDown;
        private float coolDown;
        private float projectileFlySpeed;
        private float projectileSpread;
        private int pearcingCount;
        private GameObject projectilePrefab;
        private int projectileCount;
        
        public override void Shoot()
        {
            if (timeFromCoolDown > coolDown)
            {
                Projectile projectile = projectilePrefab.GetComponent<Projectile>();
                if (projectile == null)
                {
                    Debug.Log("Trying to shoot bullet that doesn't contain Bullet script");
                    return;
                }
                for (int i = 0; i < projectileCount; i++)
                {
                    float spread = Random.Range(-projectileSpread, projectileSpread);
                    Quaternion rotation = Quaternion.Euler(ShootPosition.eulerAngles.x, ShootPosition.eulerAngles.y, ShootPosition.eulerAngles.z + spread);
                    GameObject bullet = objectPool.GetObjectFromPool(projectile.PoolObjectType, projectilePrefab, ShootPosition.position).GetGameObject();
                    bullet.transform.rotation = rotation;
                    bullet.transform.position = ShootPosition.position;
                    bullet.GetComponent<Projectile>().Init(damage, pearcingCount);
                    bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * projectileFlySpeed;
                }
                //audioSource.PlayOneShot(shootSound);
                timeFromCoolDown = 0.0f;
            }
        }
        private void Update()
        {
            timeFromCoolDown += Time.deltaTime;
        }
        protected override void Awake()
        {
            base.Awake();
            this.damage = Data.damage;
            this.coolDown = (Data as ProjectileStaffSO).coolDown;
            this.projectileFlySpeed = (Data as ProjectileStaffSO).projectileFlySpeed;
            this.projectileSpread = (Data as ProjectileStaffSO).projectileSpread;
            this.pearcingCount = (Data as ProjectileStaffSO).pearcingCount;
            this.projectilePrefab = (Data as ProjectileStaffSO).projectilePrefab;
            this.projectileCount = (Data as ProjectileStaffSO).projectileCount;
            timeFromCoolDown = coolDown;
        }
        
    }
    
}

