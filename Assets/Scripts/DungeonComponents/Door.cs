using UnityEngine;
[System.Serializable]
public class Door
{
    public Vector2Int pos;
    public DoorOrientation orientation;
    public GameObject prefab;
    public Vector2Int wallToBuildPosition;
    public int wallBuildingWidthInTiles;
    public int wallBuildingHeigthInTiles;
    [HideInInspector]
    public bool isConnected = false;
    [HideInInspector]
    public bool isAvailable = true;
    public Door() { }

    public Door(Vector2Int pos, DoorOrientation orientation, GameObject prefab, Vector2Int wallToBuildPosition, int wallBuildingWidthInTiles, int wallBuildingHeigthInTiles)
    {
        this.pos = pos;
        this.orientation = orientation;
        this.prefab = prefab;
        this.wallToBuildPosition = wallToBuildPosition;
        this.wallBuildingWidthInTiles = wallBuildingWidthInTiles;
        this.wallBuildingHeigthInTiles = wallBuildingHeigthInTiles;
        //this.isConnected = isConnected;
        //this.isAvailable = isAvailable;
    }
}
