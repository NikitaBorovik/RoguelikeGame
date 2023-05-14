using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyResources : MonoBehaviour
{
    private static MyResources instance;

    public static MyResources GetInstance()
    {
        if (instance != null)
        {
            return instance;
        }
        else
        {
            instance = Resources.Load<MyResources>("MyResources");
            return instance;
        }
            
    }
    public AvailableNodeTypesForRoom roomNodeTypes;

}
