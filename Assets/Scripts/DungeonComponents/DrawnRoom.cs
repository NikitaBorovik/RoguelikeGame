using App.Systems.GameStates;
using App.World.Items.Treasures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class DrawnRoom : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [HideInInspector]
    public RoomData room;
    private Grid grid;
    private Tilemap backgroundFloor;
    private Tilemap decoration;
    private Tilemap wall;
    private Tilemap front;
    private Tilemap collisions;
    private Tilemap minimap;
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
    public Tilemap Decoration { get => decoration; set => decoration = value; }
    public Tilemap Wall { get => wall; set => wall = value; }
    public Tilemap Front { get => front; set => front = value; }
    public Tilemap Collisions { get => collisions; set => collisions = value; }
    public Tilemap Minimap { get => minimap; set => minimap = value; }
    public List<GameObject> ToHideAndReveal { get => toHideAndReveal; set => toHideAndReveal = value; }
    public bool IsVisited { get => isVisited; set => isVisited = value; }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        tileRenderers = GetComponentsInChildren<TilemapRenderer>();
        
        if (!isEntrance)
        {
            foreach (var tileRenderer in tileRenderers)
            {
                tileRenderer.material.SetFloat("_AlphaValue", 0f);
            }
            foreach (var item in ToHideAndReveal)
            {
                item.SetActive(false);
            }
        }
        else
        {
            isVisited = true;
        }
        
        gameStates = FindObjectOfType<GameStatesSystem>();

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        room.Notifieble.NotifyOnRoomChanged(room);
        if (!IsVisited && !room.RoomNodeType.isEntrance) 
        {
            foreach (var tileRenderer in tileRenderers)
            {
                StartCoroutine(Reveal(tileRenderer));
            }
            
        }
        foreach (var item in ToHideAndReveal)
        {
            item.SetActive(true);
        }

        if (!room.RoomNodeType.isCorridor && !room.RoomNodeType.isEntrance && !IsVisited)
        {
            gameStates.EnteringRoom();
        }
        IsVisited = true;


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
            if(door.Prefab != null && door.IsConnected)
            {
                if (door.Orientation == Door.DoorOrientation.top || door.Orientation == Door.DoorOrientation.bottom)
                {
                    drawnDoors.Add(GameObject.Instantiate(door.Prefab, Grid.CellToWorld(new Vector3Int(door.Pos.x, door.Pos.y, 0)) + new Vector3(Grid.cellSize.x / 2, Grid.cellSize.y / 2, 0), Quaternion.identity, this.transform).GetComponent<InstanciatedDoor>());
                }
                    
                if (door.Orientation == Door.DoorOrientation.left || door.Orientation == Door.DoorOrientation.right)
                {
                    drawnDoors.Add(GameObject.Instantiate(door.Prefab, Grid.CellToWorld(new Vector3Int(door.Pos.x, door.Pos.y, 0)) + new Vector3(Grid.cellSize.x / 2, Grid.cellSize.y * 3 / 2, 0), Quaternion.identity,this.transform).GetComponent<InstanciatedDoor>());
                }
                   

            }
        }
        foreach (InstanciatedDoor door in drawnDoors)
        {
            
            foreach (SpriteRenderer renderer in door.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.material.SetFloat("_AlphaValue", 0f);
            }
        }
    }

    private void AddRoomObstacles()
    {
        if (Collisions == null)
            return;

        int width = room.RoomModel.rightTopPoint.x - room.RoomModel.leftBottomPoint.x + 1;
        int height = room.RoomModel.rightTopPoint.y - room.RoomModel.leftBottomPoint.y + 1;

        gridTilesPriorityWeigths = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i + room.RoomModel.leftBottomPoint.x, j + room.RoomModel.leftBottomPoint.y, 0);
                TileBase tile = Collisions.GetTile(tilePosition);

                gridTilesPriorityWeigths[i, j] = tile == preferedTile ? 1 : collisionTileVariants.Contains(tile) ? 0 : 50;
            }
        }
    }

    private void BlockRedundantDoors()
    {
        foreach(Door door in room.Doors)
        {
            if(!door.IsConnected)
            {
                BlockRedundantDoorsForTilemap(door, BackgroundFloor);
                BlockRedundantDoorsForTilemap(door, Wall);
                BlockRedundantDoorsForTilemap(door, Decoration);
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
        if (door.Orientation == Door.DoorOrientation.top || door.Orientation == Door.DoorOrientation.bottom)
            AddTilesHorizontally(door,tilemap);
        if (door.Orientation == Door.DoorOrientation.left || door.Orientation == Door.DoorOrientation.right)
            AddTilesVertically(door, tilemap);
    }

    private void AddTilesVertically(Door door, Tilemap tilemap)
    {
        Vector2Int start = door.WallToBuildPosition;
        for (int i = 0; i < door.WallBuildingHeigthInTiles; i++)
        {
            for (int j = 0; j < door.WallBuildingWidthInTiles; j++)
            {
                Vector3Int currentPos = new Vector3Int(start.x + j, start.y - i, 0);
                Vector3Int newPos = new Vector3Int(currentPos.x, currentPos.y - 1, 0);

                Tile tile = (Tile)tilemap.GetTile(currentPos);
                Matrix4x4 transform = tilemap.GetTransformMatrix(currentPos);

                tilemap.SetTile(newPos, tile);
                tilemap.SetTransformMatrix(newPos, transform);
            }
        }
    }

    private void AddTilesHorizontally(Door door, Tilemap tilemap)
    {
        Vector2Int start = door.WallToBuildPosition;
        for (int i = 0; i < door.WallBuildingWidthInTiles; i++)
        {
            for (int j = 0; j < door.WallBuildingHeigthInTiles; j++)
            {
                Vector3Int currentPos = new Vector3Int(start.x + i, start.y - j, 0);
                Vector3Int newPos = new Vector3Int(currentPos.x + 1, currentPos.y, 0);

                Tile tile = (Tile)tilemap.GetTile(currentPos);
                Matrix4x4 transform = tilemap.GetTransformMatrix(currentPos);

                tilemap.SetTile(newPos, tile);
                tilemap.SetTransformMatrix(newPos, transform);
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
        for(int i = 0;i<tilemaps.Length;i++)
        {
            switch (tilemaps[i].gameObject.tag)
            {
                case "floor":
                    BackgroundFloor = tilemaps[i];
                    break;
                case "wall":
                    Wall = tilemaps[i];
                    break;
                case "decoration1":
                    Decoration = tilemaps[i];
                    break;
                case "front":
                    Front = tilemaps[i];
                    break;
                case "collisions":
                    Collisions = tilemaps[i];
                    break;
                case "minimap":
                    Minimap = tilemaps[i];
                    break;
            }
        }
    }
}
