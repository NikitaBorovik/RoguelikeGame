using App.Systems;
using App.World.Creatures.PlayerScripts.Components;
using App.World.Items.Attacks;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private ShootEvent shootEvent;
    [SerializeField]
    private Player player;
    private bool canShoot = true;
    private ObjectPool objectPool; 

    public bool CanShoot { get => canShoot; set => canShoot = value; }

    private void Awake()
    {
        objectPool = FindObjectOfType<ObjectPool>();
    }

    private void OnEnable()
    {
        shootEvent.OnShoot += OnShoot;
    }
    private void OnDisable()
    {
        shootEvent.OnShoot -= OnShoot;
    }
    
    private void OnShoot(ShootEvent obj)
    {
        if (canShoot && player.Mana.CurrentMana >= player.Projectile.ProjectileData.manacost)
        {
            player.Mana.SpendMana(player.Projectile.ProjectileData.manacost);
            GameObject projectile = objectPool.GetObjectFromPool(player.Projectile.PoolObjectType, player.Projectile.gameObject, player.ShootPosition.position).GetGameObject();
            projectile.transform.position = player.ShootPosition.position;
            projectile.GetComponent<Projectile>().Init(player);
        }
        
    }
}
