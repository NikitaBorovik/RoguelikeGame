using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.UI
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] 
        private PauseEvent pauseEvent;
        [SerializeField] 
        private GameObject fader;
        [SerializeField] 
        private GameObject pauseBoard;
        private bool paused;
        private float initialTimeScale;
        private Animator boardAnimator;
        private Animator faderAnimator;
        public bool Paused => paused;
        private void Awake()
        {
            paused = false;
            initialTimeScale = Time.timeScale;
            boardAnimator = pauseBoard.GetComponent<Animator>();
            faderAnimator = fader.GetComponent<Animator>();
        }

        private void Start()
        {
            fader.SetActive(false);
        }

        //private void OnEnable()
        //{
        //    onDeathScreenAppeared.OnDeathScreenAppeared += StopGameEvent;
        //}

        //private void OnDisable()
        //{
        //    onDeathScreenAppeared.OnDeathScreenAppeared -= StopGameEvent;
        //}

        public void Pause()
        {
            if (!paused)
            {
                fader.SetActive(true);
                boardAnimator.Play("Appear");
                faderAnimator.Play("Appear");
                paused = true;
                initialTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                pauseEvent.CallPauseEvent(true);
            }
            
        }

        public void Unpause()
        {
            if (paused)
            {
                boardAnimator.Play("Disappear");
                faderAnimator.Play("Disappear");
                paused = false;
                Time.timeScale = initialTimeScale;
                pauseEvent.CallPauseEvent(false);
            }
            
        }
    }
}

