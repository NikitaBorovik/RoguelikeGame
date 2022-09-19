using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<LevelModel> levels;
    [SerializeField]
    private int curLevel = 0;
    private GlobalStates globalGameStates;

    public GlobalStates GlobalGameStates { get => globalGameStates; set => globalGameStates = value; }

    // Start is called before the first frame update
    void Start()
    {
        GlobalGameStates = GlobalStates.start;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCurrentameState();
        if(Input.GetKeyDown(KeyCode.R))
        {
            GlobalGameStates = GlobalStates.start;
        }
    }

    private void CheckCurrentameState()
    {
        switch (GlobalGameStates)
        {
            case GlobalStates.start:
                Debug.Log("1");
                StartCurrentLevel(curLevel);
                GlobalGameStates = GlobalStates.playing;
                break;
            case GlobalStates.playing:
                break;
        }

    }

    private void StartCurrentLevel(int curLevel)
    {
        bool buildSuccessfull = FindObjectOfType<DungeonGenerator>().GenerateDungeon(levels[curLevel]);
        if (buildSuccessfull)
        {
            Debug.Log("OK!");
        }
        if(!buildSuccessfull)
        {
            Debug.Log("Cannot create the dungeon!");
        }
    }

    private void OnValidate()
    {
        if(levels == null || levels.Count == 0)
        {
            Debug.Log("No levels were added to the game manager!");
        }
    }
}
