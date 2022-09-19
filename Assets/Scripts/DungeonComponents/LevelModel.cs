using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="MyDungeonLevel",menuName = "Scriptable Objects/Dungeon/New Dungeon Level")]
public class LevelModel : ScriptableObject
{
    public string levelName;
    public List<RoomModel> roomModels;
    public List<DungeonStructureGraph> dungeonStructures;
    public void OnValidate()
    {
        if (name == null)
            Debug.Log("Name for the level is not set!");
        if (dungeonStructures == null || dungeonStructures.Count == 0)
            Debug.Log("There are no dungeon structure graphs assigned to the level!");
        if (roomModels == null || roomModels.Count == 0)
            Debug.Log("There are no room models assigned to the level!");
    }

}
