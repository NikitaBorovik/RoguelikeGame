using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomComponentsEnabler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var room = collision.GetComponent<DrawnRoom>();
        if (room != null && room.IsVisited)
        {
            foreach (GameObject go in collision.GetComponent<DrawnRoom>().ToHideAndReveal)
            {
                go.SetActive(true);
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var room = collision.GetComponent<DrawnRoom>();
        if (room != null && room.IsVisited)
        {
            foreach (GameObject go in collision.GetComponent<DrawnRoom>().ToHideAndReveal)
            {
                go.SetActive(false);
            }
        }
        
    }
}
