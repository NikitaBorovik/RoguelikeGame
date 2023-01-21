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
        public GameObject Player { get => player; }
        public DungeonGenerator DungeonGenerator { get => dungeonGenerator; set => dungeonGenerator = value; }
    }
}

