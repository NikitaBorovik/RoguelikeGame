using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarTest : MonoBehaviour
{
    public GameObject pathPoint;
    public Vector3Int startPos = Vector3Int.zero;
    public Vector3Int endPos = Vector3Int.zero;
    public Grid grid;
    public AStarPathfinding aStarPathfinding;
    public Room room;


    public void Update()
    {
        if (grid != null)
        {




            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("Here1");
                startPos = grid.WorldToCell(GetMousePositionInWorld());
                GameObject.Instantiate(pathPoint, GetMousePositionInWorld(), Quaternion.identity);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Here2");
                endPos = grid.WorldToCell(GetMousePositionInWorld());
                GameObject.Instantiate(pathPoint, GetMousePositionInWorld(), Quaternion.identity);
            }
            if (Input.GetKeyDown(KeyCode.P) && startPos != Vector3Int.zero && endPos != Vector3Int.zero)
            {
                Debug.Log("Here3");
                Stack<Vector3> path = aStarPathfinding.FindPath(startPos, endPos, room);
                if(path == null)
                {
                    Debug.Log("No path!");
                }
                foreach (Vector3 p in path)
                {
                   // Debug.Log(p);
                    GameObject.Instantiate(pathPoint, p, Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log("Mouse at " + GetMousePositionInWorld());
            }
        }
    }

    private Vector3 GetMousePositionInWorld()
    {
        Vector3 mouseOnScreenPos = UnityEngine.Input.mousePosition;
        Camera mainCamera = FindObjectOfType<Camera>();
        mouseOnScreenPos = mainCamera.ScreenToWorldPoint(mouseOnScreenPos);
        mouseOnScreenPos.z = 0f;
        return mouseOnScreenPos;
    }



}
