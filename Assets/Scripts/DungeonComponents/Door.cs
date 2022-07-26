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
}
