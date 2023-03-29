using App.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.UI
{
    public class DeathScreenController : MonoBehaviour
    {
        [SerializeField]
        private PauseController pauseController;
        [SerializeField]
        private GameObject fader;
        [SerializeField]
        private GameObject deathBoard;
        private Animator boardAnimator;
        private Animator faderAnimator;
        private void Awake()
        {
            boardAnimator = deathBoard.GetComponent<Animator>();
            faderAnimator = fader.GetComponent<Animator>();
        }

        private void Start()
        {
            fader.SetActive(false);
        }
        
        public void Appear()
        {
            fader.SetActive(true);
            pauseController.enabled = false;
            boardAnimator.Play("Appear");
            faderAnimator.Play("Appear");
        }
    }

}
