using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciatedDoor : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Animator animator;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsOpen", true);
        
    }
    public void SetOpenAnimation()
    {
        animator.SetBool("IsOpen", true);
    }
    public void SetCloseAnimation()
    {
        animator.SetBool("IsOpen", false);
        
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
