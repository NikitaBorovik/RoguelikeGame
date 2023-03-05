using App.World.Creatures.PlayerScripts.Events;
using App.World.Items.Attacks;
using App.World.Items.Staffs;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Rendering;
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
    public class Player : MonoBehaviour//, IKillable, IUpgradable
    {
        #region Components
        private Transform playerTransform;
        private Animator pAnimator;
        //private Health health;
        //private UpgradeManager upgradeManager;
        [SerializeField]
        private PlayerDataSO playerData;
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
        #endregion

        #region Sounds
        [SerializeField]
        private AudioClip[] stepSounds;
        private AudioSource audioSource;
        #endregion

        #region Parameters
        private float movementSpeed;
        private float dashDistance = 4;
        private float dashTime = 0.25f;
        private int money;
        private bool isDead; //TODO replace with more global "game stop"
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
        public Projectile Projectile { get => projectile; set => projectile = value; }

        #endregion

        private void Awake()
        {
            Init();
        }
        private void Init()
        {
            playerTransform = GetComponent<Transform>();
            pAnimator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            //Staff = CurWeaponObj.GetComponent<BaseStaff>();
            movementSpeed = 8;
            isDead = false;
        }
        public void Die()
        {
            GetComponent<Movement>().enabled = false;
            GetComponent<Aim>().enabled = false;
            if (isDead) return;
        }
        
        public void DisableAllInputs()
        {
            GetComponent<Movement>().enabled = false;
            GetComponent<Stand>().enabled = false;
            GetComponent<Movement>().enabled = false;
            GetComponent<Aim>().enabled = false;
        }
        public void EnableAllInputs()
        {
            GetComponent<Movement>().enabled = true;
            GetComponent<Stand>().enabled = true;
            GetComponent<Movement>().enabled = true;
            GetComponent<Aim>().enabled = true;
        }
    }
}