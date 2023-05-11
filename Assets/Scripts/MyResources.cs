using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyResources : MonoBehaviour
{
    private static MyResources instance;

    public static MyResources Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<MyResources>("MyResources");
            return instance;
        }

    }
    public RoomNodeTypes roomNodeTypes;

}
