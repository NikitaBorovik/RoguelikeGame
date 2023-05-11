using App;
using App.Systems.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DungeonExploringState : IState
{
    private GameStatesSystem gameStatesSystem;
    private AudioClip restingMusic;
    private AudioSource audioSource;
    private float musicFadeTime = 1f;
    public DungeonExploringState(GameStatesSystem gameStatesSystem, AudioClip restingMusic, AudioSource audioSource)
    {
        this.gameStatesSystem = gameStatesSystem;
        this.restingMusic = restingMusic;
        this.audioSource = audioSource;
    }
    public void Enter()
    {
        gameStatesSystem.StartCoroutine(StartMusic());
    }

    public void Exit()
    {
        gameStatesSystem.StartCoroutine(StopMusic());
    }

    public void Update()
    {
        
    }
    private IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(musicFadeTime);
        audioSource.volume = 0f;
        audioSource.clip = restingMusic;
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
