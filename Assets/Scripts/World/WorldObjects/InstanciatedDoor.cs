using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciatedDoor : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip doorSound;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsOpen", true);
        audioSource = GetComponent<AudioSource>();
        
    }
    public void SetOpenAnimation()
    {
        animator.SetBool("IsOpen", true);
        audioSource.PlayOneShot(doorSound);
    }
    public void SetCloseAnimation()
    {
        animator.SetBool("IsOpen", false);
        audioSource.PlayOneShot(doorSound);
        
    }
    public void SwitchColliderState()
    {
        if (boxCollider2D.isActiveAndEnabled)
        {
            boxCollider2D.enabled = false;
        }
        else
        {
            boxCollider2D.enabled = true;
        }
    }
}
