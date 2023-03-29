using App.Systems.GameStates;
using App.World.Items.Treasures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class DrawnRoom : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [HideInInspector]
    public Room room;
    private Grid grid;
    private Tilemap backgroundFloor;
    private Tilemap decoration1;
    private Tilemap decoration2;
    private Tilemap wall;
    private Tilemap front;
    private Tilemap collisions;
    private Tilemap minimap;
    private Bounds roomCollider;
    private int[,] gridTilesPriorityWeigths;
    [SerializeField]
    private Tile[] collisionTileVariants;
    [SerializeField]
    private Tile preferedTile;
    private List<InstanciatedDoor> drawnDoors;
    [SerializeField]
    private List<GameObject> toHideAndReveal;


    TilemapRenderer[] tileRenderers;
    private bool isVisited = false;
    private GameStatesSystem gameStates;
    [SerializeField]
    private bool isEntrance = false;

    public Grid Grid { get => grid; set => grid = value; }
    public int[,] GridTilesPriorityWeigths { get => gridTilesPriorityWeigths; set => gridTilesPriorityWeigths = value; }
    public Tilemap BackgroundFloor { get => backgroundFloor; set => backgroundFloor = value; }
    public Tilemap Decoration1 { get => decoration1; set => decoration1 = value; }
    public Tilemap Decoration2 { get => decoration2; set => decoration2 = value; }
    public Tilemap Wall { get => wall; set => wall = value; }
    public Tilemap Front { get => front; set => front = value; }
    public Tilemap Collisions { get => collisions; set => collisions = value; }
    public Tilemap Minimap { get => minimap; set => minimap = value; }
    public Bounds RoomCollider { get => roomCollider; set => roomCollider = value; }
    

    private void Awake()
    {
        //Debug.Log(room != null);
        boxCollider = GetComponent<BoxCollider2D>();
        RoomCollider = boxCollider.bounds;
        tileRenderers = GetComponentsInChildren<TilemapRenderer>();
        
        if (!isEntrance)
        {
            foreach (var tileRenderer in tileRenderers)
            {
                tileRenderer.material.SetFloat("_AlphaValue", 0f);
            }
            
        }
        foreach (var item in toHideAndReveal)
        {
            item.SetActive(false);
        }
        gameStates = FindObjectOfType<GameStatesSystem>();

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        room.Notifieble.NotifyOnRoomChanged(room);
        if (!isVisited && !room.RoomNodeType.isEntrance) 
        {
            foreach (var tileRenderer in tileRenderers)
            {
                StartCoroutine(Reveal(tileRenderer));
            }
            
        }
        foreach (var item in toHideAndReveal)
        {
            item.SetActive(true);
        }

        //gameStates.CurRoom = room;

        if (!room.RoomNodeType.isCorridor && !room.RoomNodeType.isEntrance && !isVisited)
        {
            gameStates.EnteringRoom();
        }
        isVisited = true;


    }
    
    public void Open()
    {
        foreach (InstanciatedDoor door in drawnDoors)
        {
            door.SetOpenAnimation();
        }
    }
    
    public void Close()
    {
        foreach (InstanciatedDoor door in drawnDoors)
        {
            door.SetCloseAnimation();
        }
    }

    private IEnumerator Reveal(TilemapRenderer tileRenderer)
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += 0.1f;
            tileRenderer.material.SetFloat("_AlphaValue", alpha);
            foreach (InstanciatedDoor door in drawnDoors)
            {
                foreach (SpriteRenderer renderer in door.GetComponentsInChildren<SpriteRenderer>())
                {
                    renderer.material.SetFloat("_AlphaValue", alpha);
                }
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void DrawRoom(GameObject roomObject)
    {
        AddMemberVariablesToTilemap(roomObject);
        BlockRedundantDoors();
        if(Collisions != null)
        Collisions.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        AddRoomObstacles();
        AddDoors();
    }

    private void AddDoors()
    {
        drawnDoors = new List<InstanciatedDoor>();
        foreach (Door door in room.Doors)
        {
            if(door.prefab != null && door.isConnected)
            {
                // GameObject.Instantiate(door.prefab, Grid.CellToWorld(new Vector3Int(door.pos.x, door.pos.y, 0)) + new Vector3(Grid.cellSize.x / 2, Grid.cellSize.y / 2, 0), Quaternion.identity);
                if (door.orientation == DoorOrientation.top || door.orientation == DoorOrientation.bottom)
                {
                    drawnDoors.Add(GameObject.Instantiate(door.prefab, Grid.CellToWorld(new Vector3Int(door.pos.x, door.pos.y, 0)) + new Vector3(Grid.cellSize.x / 2, Grid.cellSize.y / 2, 0), Quaternion.identity, this.transform).GetComponent<InstanciatedDoor>());
                }
                    
                if (door.orientation == DoorOrientation.left || door.orientation == DoorOrientation.right)
                {
                    drawnDoors.Add(GameObject.Instantiate(door.prefab, Grid.CellToWorld(new Vector3Int(door.pos.x, door.pos.y, 0)) + new Vector3(Grid.cellSize.x / 2, Grid.cellSize.y * 3 / 2, 0), Quaternion.identity,this.transform).GetComponent<InstanciatedDoor>());
                }
                   

            }
        }
        foreach (InstanciatedDoor door in drawnDoors)
        {
            door.SetOpenAnimation();
            
            foreach (SpriteRenderer renderer in door.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.material.SetFloat("_AlphaValue", 0f);
            }
        }
    }

    private void AddRoomObstacles()
    {
        gridTilesPriorityWeigths = new int[room.RoomModel.rightTopPoint.x - room.RoomModel.leftBottomPoint.x + 1, room.RoomModel.rightTopPoint.y - room.RoomModel.leftBottomPoint.y + 1];
        for (int i = 0; i < gridTilesPriorityWeigths.GetLength(0); i++)
        {
            for (int j = 0; j < gridTilesPriorityWeigths.GetLength(1); j++)
            {
                
                gridTilesPriorityWeigths[i, j] = 50;
                if (Collisions == null)
                    return;
                TileBase tile = Collisions.GetTile(new Vector3Int(i + room.RoomModel.leftBottomPoint.x, j + room.RoomModel.leftBottomPoint.y, 0));
                if (tile == preferedTile)
                    gridTilesPriorityWeigths[i, j] = 1;
                foreach (TileBase t in collisionTileVariants)
                {
                    if (tile == t)
                        gridTilesPriorityWeigths[i, j] = 0;    
                }
                
            }
        }
    }

    private void BlockRedundantDoors()
    {
        foreach(Door door in room.Doors)
        {
            if(!door.isConnected)
            {
                BlockRedundantDoorsForTilemap(door, BackgroundFloor);
                BlockRedundantDoorsForTilemap(door, Wall);
                BlockRedundantDoorsForTilemap(door, Decoration1);
                BlockRedundantDoorsForTilemap(door, Decoration2);
                BlockRedundantDoorsForTilemap(door, Front);
                BlockRedundantDoorsForTilemap(door, Collisions);
                BlockRedundantDoorsForTilemap(door, Minimap);
            }
        }
    }

    private void BlockRedundantDoorsForTilemap(Door door, Tilemap tilemap)
    {
        if (tilemap == null)
            return;
        if (door.orientation == DoorOrientation.top || door.orientation == DoorOrientation.bottom)
            AddTilesHorizontally(door,tilemap);
        if (door.orientation == DoorOrientation.left || door.orientation == DoorOrientation.right)
            AddTilesVertically(door, tilemap);
    }

    private void AddTilesVertically(Door door, Tilemap tilemap)
    {
        Vector2Int start = door.wallToBuildPosition;
        for (int i = 0; i < door.wallBuildingHeigthInTiles; i++)
        {
            
            for (int j = 0; j < door.wallBuildingWidthInTiles; j++)
            {
                Tile tile = (Tile)tilemap.GetTile(new Vector3Int(start.x + j, start.y - i, 0));
                Matrix4x4 transform = tilemap.GetTransformMatrix(new Vector3Int(start.x + j, start.y - i, 0));
                tilemap.SetTile(new Vector3Int(start.x + j, start.y - i - 1, 0), tile);
                tilemap.SetTransformMatrix(new Vector3Int(start.x + j, start.y - i - 1, 0), transform);
            }
        }
    }

    private void AddTilesHorizontally(Door door, Tilemap tilemap)
    {
        Vector2Int start = door.wallToBuildPosition;
        for(int i = 0;i < door.wallBuildingWidthInTiles;i++)
        {
           // Tile tile = (Tile)tilemap.GetTile(new Vector3Int(start.x + i, start.y, 0));
            for(int j = 0;j < door.wallBuildingHeigthInTiles;j++)
            {
                Tile tile = (Tile)tilemap.GetTile(new Vector3Int(start.x + i, start.y - j, 0));
                Matrix4x4 transform = tilemap.GetTransformMatrix(new Vector3Int(start.x+i,start.y-j,0));
                tilemap.SetTile(new Vector3Int(start.x+i+1,start.y - j,0), tile);
                tilemap.SetTransformMatrix(new Vector3Int(start.x+i+1,start.y-j,0),transform);
            }
        }
    }

    private void AddMemberVariablesToTilemap(GameObject roomObject)
    {
        Grid = roomObject.GetComponentInChildren<Grid>();
        Tilemap[] tilemaps = roomObject.GetComponentsInChildren<Tilemap>();
        FillTilemaps(tilemaps);
    }

    private void FillTilemaps(Tilemap[] tilemaps)
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            if(tilemap.gameObject.tag == "floor")
                BackgroundFloor = tilemap;
            else if(tilemap.gameObject.tag == "wall")
                Wall = tilemap;
            else if(tilemap.gameObject.tag == "decoration1")
                Decoration1 = tilemap;
            else if(tilemap.gameObject.tag == "decoration2")
                Decoration2 = tilemap;
            else if(tilemap.gameObject.tag == "front")
                Front = tilemap;
            else if(tilemap.gameObject.tag == "collisions")
                Collisions = tilemap;
            else if(tilemap.gameObject.tag == "minimap")
                Minimap = tilemap;
        }
    }
}
