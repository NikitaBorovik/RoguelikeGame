using App.UI;
using App.World.Creatures.PlayerScripts.Events;
using App.World.Items.Attacks;
using App.World.Items.Treasures;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.World.Creatures.PlayerScripts.Components
{
    #region Required
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Transform))]
    //[RequireComponent(typeof(Health))]
    //[RequireComponent(typeof(PlayerAnimationsController))]
    //[RequireComponent(typeof(Movement))]
    //[RequireComponent(typeof(Aim))]
    //[RequireComponent(typeof(Stand))]
    //[RequireComponent(typeof(UpgradeManager))]
    #endregion
    public class Player : MonoBehaviour, IKillable, IUpgradable
    {
        #region Components
        private Transform playerTransform;
        private Animator pAnimator;
        private HealthStatus health;
        private ManaStatus mana;
        private INotifyGameEnded notifiebleForGameEnded;
        [SerializeField]
        private WeaponPanel weaponPanel;
        [SerializeField]
        private PlayerDataSO playerData;
        [SerializeField]
        private List<Projectile> powers;
        #endregion

        #region Weapon
        [SerializeField]
        private Transform shootPosition;
        [SerializeField]
        private Projectile projectile;

        #endregion

        #region Events
        [SerializeField]
        private AimEvent aimEvent;
        [SerializeField]
        private StandEvent standEvent;
        [SerializeField]
        private MovementEvent movementEvent;
        [SerializeField]
        private DashEvent dashEvent;
        [SerializeField]
        private ShootEvent shootEvent;
        [SerializeField]
        private ObtainEvent obtainEvent;
        [SerializeField]
        private ValueUpdateEvent valueUpdateEvent;
        #endregion

        #region Sounds
        [SerializeField]
        private AudioClip[] stepSounds;
        private AudioSource audioSource;
        #endregion

        #region Parameters
        private float movementSpeed;
        private float dashDistance ;
        private float dashTime;
        private float dashCooldown ;
        private float dashCooldownTimer;
        #endregion

        #region Properties
        public Transform ShootPosition { get => shootPosition; set => shootPosition = value; }
        public Animator PAnimator { get => pAnimator; }
        public Transform PlayerTransform { get => playerTransform; }
        public AimEvent AimEvent { get => aimEvent; }
        public StandEvent StandEvent { get => standEvent; }
        public MovementEvent MovementEvent { get => movementEvent; }
        public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
        public float DashDistance { get => dashDistance; set => dashDistance = value; }
        public float DashTime { get => dashTime; set => dashTime = value; }
        public DashEvent DashEvent { get => dashEvent; set => dashEvent = value; }
        public ShootEvent ShootEvent { get => shootEvent; set => shootEvent = value; }
        public Projectile Projectile 
        {
            get 
            { 
                return projectile; 
            }
            set 
            { 
                projectile = value;
                weaponPanel.SetWeaponPicture(projectile.TreasureSprite); 
            }
        }
        public float DashCooldownTimer { get => dashCooldownTimer; set => dashCooldownTimer = value; }
        public ObtainEvent ObtainEvent { get => obtainEvent; set => obtainEvent = value; }
        public List<Projectile> Powers { get => powers; set => powers = value; }
        public ManaStatus Mana { get => mana; set => mana = value; }
        public HealthStatus Health { get => health; set => health = value; }
        public INotifyGameEnded NotifiebleForGameEnded { get => notifiebleForGameEnded; set => notifiebleForGameEnded = value; }
        public ValueUpdateEvent ValueUpdateEvent { get => valueUpdateEvent; set => valueUpdateEvent = value; }

        #endregion

        private void Awake()
        {
            DashCooldownTimer = dashCooldown;
            Init();
        }

        private void Update()
        {
            if (DashCooldownTimer > 0)
                DashCooldownTimer -= Time.deltaTime;
        }
        private void Init()
        {
            playerTransform = GetComponent<Transform>();
            pAnimator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            Health = GetComponent<HealthStatus>();
            Mana = GetComponent<ManaStatus>();
            Health.MaxHealth = playerData.maxHealth;
            movementSpeed = playerData.speed;
            dashDistance = playerData.dashDistance;
            dashTime = playerData.dashTime;
            dashCooldown = playerData.dashCooldown;
            mana.MaxMana = playerData.maxMana;
            foreach (AnimationClip clip in pAnimator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "DashLeft")
                    dashTime = clip.length;
            }
        }
        public void Die()
        {
            GetComponent<Movement>().enabled = false;
            GetComponent<Aim>().enabled = false;
            NotifiebleForGameEnded.NotifyGameEnded(false);

        }
        
        public void DisableAllInputs()
        {
            GetComponent<Movement>().enabled = false;
            GetComponent<Stand>().enabled = false;
            GetComponent<Aim>().enabled = false;
            GetComponent<Shoot>().enabled = false;
            GetComponent<Dash>().enabled = false;
        }
        public void EnableAllInputs()
        {
            GetComponent<Movement>().enabled = true;
            GetComponent<Stand>().enabled = true;
            GetComponent<Aim>().enabled = true;
            GetComponent<Shoot>().enabled = true;
            GetComponent<Dash>().enabled = true;
        }
        public void ReloadDashTimer()
        {
            DashCooldownTimer = dashCooldown;
        }

        public void EnableUpgrade(BaseUpgradeTreasure upgrade)
        {
            upgrade.Enable(this);
        }

        public void UpdateUpgrade(BaseUpgradeTreasure upgrade)
        {
            upgrade.UpdateUpgrade(this);
        }

        public void DisableUpgrade(BaseUpgradeTreasure upgrade)
        {
            upgrade.Disable(this);
        }
    }
}