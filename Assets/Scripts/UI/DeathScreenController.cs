using App.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField]
        private TextMeshProUGUI text;
        private Animator boardAnimator;
        private Animator faderAnimator;

        public TextMeshProUGUI Text { get => text; set => text = value; }

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
