using UnityEngine;
[System.Serializable]
public class Door
{
    [HideInInspector]
    private bool isConnected = false;
    [HideInInspector]
    private bool isAvailable = true;
    [SerializeField]
    private Vector2Int pos;
    [SerializeField]
    private DoorOrientation orientation;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Vector2Int wallToBuildPosition;
    [SerializeField]
    private int wallBuildingWidthInTiles;
    [SerializeField]
    private int wallBuildingHeigthInTiles;

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public bool IsConnected { get => isConnected; set => isConnected = value; }
    public bool IsAvailable { get => isAvailable; set => isAvailable = value; }
    public Vector2Int Pos { get => pos; set => pos = value; }
    public DoorOrientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int WallToBuildPosition { get => wallToBuildPosition; set => wallToBuildPosition = value; }
    public int WallBuildingWidthInTiles { get => wallBuildingWidthInTiles; set => wallBuildingWidthInTiles = value; }
    public int WallBuildingHeigthInTiles { get => wallBuildingHeigthInTiles; set => wallBuildingHeigthInTiles = value; }

    public Door() { }

    public Door(Vector2Int pos, DoorOrientation orientation, GameObject prefab, Vector2Int wallToBuildPosition, int wallBuildingWidthInTiles, int wallBuildingHeigthInTiles)
    {
        this.Pos = pos;
        this.Orientation = orientation;
        this.Prefab = prefab;
        this.WallToBuildPosition = wallToBuildPosition;
        this.WallBuildingWidthInTiles = wallBuildingWidthInTiles;
        this.WallBuildingHeigthInTiles = wallBuildingHeigthInTiles;
    }
    public enum DoorOrientation
    {
        top,
        bottom,
        left,
        right,
    }
}
