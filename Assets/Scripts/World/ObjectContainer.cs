using App.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World
{
    public class ObjectContainer : MonoBehaviour
    {
        [SerializeField]
        private GameObject player;
        [SerializeField]
        private DungeonGenerator dungeonGenerator;
        [SerializeField]
        private ObjectPool objectPool;
        public GameObject Player { get => player; }
        public DungeonGenerator DungeonGenerator { get => dungeonGenerator; set => dungeonGenerator = value; }
        public ObjectPool ObjectPool { get => objectPool; set => objectPool = value; }
    }
}

