using App.Systems;
using App.UI;
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
        [SerializeField]
        private PauseController pauseController;
        public GameObject Player { get => player; }
        public DungeonGenerator DungeonGenerator { get => dungeonGenerator; set => dungeonGenerator = value; }
        public ObjectPool ObjectPool { get => objectPool; set => objectPool = value; }
        public PauseController PauseController { get => pauseController; set => pauseController = value; }
    }
}

