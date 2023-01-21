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
    [HideInInspector]
    public Grid grid;
    [HideInInspector]
    public Tilemap backgroundFloor;
    [HideInInspector]
    public Tilemap decoration1;
    [HideInInspector]
    public Tilemap decoration2;
    [HideInInspector]
    public Tilemap wall;
    [HideInInspector]
    public Tilemap front;
    [HideInInspector]
    public Tilemap collisions;
    [HideInInspector]
    public Tilemap minimap;
    [HideInInspector]
    public Bounds roomCollider;
    TilemapRenderer[] tileRenderers;
    [SerializeField]
    private bool isEntrance ;
    private bool isVisited = false;
    

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        roomCollider = boxCollider.bounds;
        tileRenderers = GetComponentsInChildren<TilemapRenderer>();
        if (!isEntrance)
        {
            foreach (var tileRenderer in tileRenderers)
            {
                tileRenderer.material.SetFloat("_AlphaValue", 0f);
            }
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isVisited && !isEntrance) 
        {
            foreach (var tileRenderer in tileRenderers)
            {
                StartCoroutine(Reveal(tileRenderer));
            }
            isVisited = true;
        }
        

    }

    private IEnumerator Reveal(TilemapRenderer tileRenderer)
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += 0.1f;
            tileRenderer.material.SetFloat("_AlphaValue", alpha);
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void DrawRoom(GameObject roomObject)
    {
        AddMemberVariablesToTilemap(roomObject);
        BlockRedundantDoors();
        if(collisions != null)
        collisions.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }

    private void BlockRedundantDoors()
    {
        foreach(Door door in room.doors)
        {
            if(!door.isConnected)
            {
                BlockRedundantDoorsForTilemap(door, backgroundFloor);
                BlockRedundantDoorsForTilemap(door, wall);
                BlockRedundantDoorsForTilemap(door, decoration1);
                BlockRedundantDoorsForTilemap(door, decoration2);
                BlockRedundantDoorsForTilemap(door, front);
                BlockRedundantDoorsForTilemap(door, collisions);
                BlockRedundantDoorsForTilemap(door, minimap);
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
        grid = roomObject.GetComponentInChildren<Grid>();
        Tilemap[] tilemaps = roomObject.GetComponentsInChildren<Tilemap>();
        FillTilemaps(tilemaps);
    }

    private void FillTilemaps(Tilemap[] tilemaps)
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            if(tilemap.gameObject.tag == "floor")
                backgroundFloor = tilemap;
            else if(tilemap.gameObject.tag == "wall")
                wall = tilemap;
            else if(tilemap.gameObject.tag == "decoration1")
                decoration1 = tilemap;
            else if(tilemap.gameObject.tag == "decoration2")
                decoration2 = tilemap;
            else if(tilemap.gameObject.tag == "front")
                front = tilemap;
            else if(tilemap.gameObject.tag == "collisions")
                collisions = tilemap;
            else if(tilemap.gameObject.tag == "minimap")
                minimap = tilemap;
        }
    }
}
