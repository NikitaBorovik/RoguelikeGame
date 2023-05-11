using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="MyDungeonLevel",menuName = "Scriptable Objects/Dungeon/New Dungeon Level")]
public class LevelModel : ScriptableObject
{
    [SerializeField]
    private List<RoomModel> roomModels;
    [SerializeField]
    private List<DungeonStructureGraph> dungeonStructures;

    public List<RoomModel> RoomModels { get => roomModels; set => roomModels = value; }
    public List<DungeonStructureGraph> DungeonStructures { get => dungeonStructures; set => dungeonStructures = value; }
}
