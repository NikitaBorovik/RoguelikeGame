using App.Systems.Spawning;
using System.Collections;
using UnityEngine;

namespace App.Systems.GameStates
{
    public class EnteringRoomState : IState
    {
        private GameStatesSystem gameStatesSystem;
        private SpawningSystem spawningSystem;
        private int curLevel;
        private AudioClip fightingMusic;
        private AudioSource audioSource;
        private float musicFadeTime = 1f;
        public int CurLevel { get => curLevel; set => curLevel = value; }

        public EnteringRoomState(GameStatesSystem gameStatesSystem, SpawningSystem spawningSystem, int curLevel, AudioClip fightingMusic, AudioSource audioSource)
        {
            this.gameStatesSystem = gameStatesSystem;
            this.spawningSystem = spawningSystem;
            this.CurLevel = curLevel;
            this.fightingMusic = fightingMusic;
            this.audioSource = audioSource;
        }

        

        public void Enter()
        {
            gameStatesSystem.CurRoom.DrawnRoom.Close();
            spawningSystem.Spawn(CurLevel);
            if(!gameStatesSystem.CurRoom.RoomModel.roomType.isTreasure)
                gameStatesSystem.StartCoroutine(StartMusic());
        }

        public void Exit()
        {
            if (!gameStatesSystem.CurRoom.RoomModel.roomType.isTreasure)
                gameStatesSystem.StartCoroutine(StopMusic());
        }

        public void Update()
        {
        }
        private IEnumerator StartMusic()
        {
            yield return new WaitForSeconds(musicFadeTime);
            audioSource.volume = 0f;
            audioSource.clip = fightingMusic;
            audioSource.Play();
            float currentTime = 0;
            float start = audioSource.volume;
            while (currentTime < musicFadeTime)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, 0.1f, currentTime / musicFadeTime);
                yield return null;
            }
            yield break;
        }
        private IEnumerator StopMusic()
        {
            float currentTime = 0;
            float start = audioSource.volume;
            while (currentTime < musicFadeTime)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, 0, currentTime / musicFadeTime);
                yield return null;
            }
            audioSource.Stop();
            yield break;
        }
    }
}

