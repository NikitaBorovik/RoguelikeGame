using App.World.Creatures.PlayerScripts.Components;
using App.World.Creatures.PlayerScripts.Events;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private DashEvent dashEvent;
    private float dashColldown = 0.5f;
    private float dashTimer = 0f;
    private float dashStopTimer = 0f;

    private void Update()
    {
        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;
        if (dashStopTimer > 0)
            dashStopTimer -= Time.deltaTime;
        else
        {
            GetComponent<Player>().EnableAllInputs();
            GetComponent<Player>().PAnimator.SetBool("isDashing", false);
        }
    }
    private void OnEnable()
    {
        dashEvent.OnDash += OnEntityDash;
    }
    private void OnDisable()
    {
        dashEvent.OnDash -= OnEntityDash;
    }
    private void OnEntityDash(DashEvent ev, DashEventArgs args)
    {
        Debug.Log("Dash");
        MakeDash(rb, args.direction, args.dashDistance, args.dashTime);
    }
    private void MakeDash(Rigidbody2D body, Vector2 direction, float distance, float time)
    {
        if (dashTimer <= 0)
        {
            float speed = distance / time;
            body.velocity = new Vector2(direction.x * speed, direction.y * speed);
            dashTimer = dashColldown;
            dashStopTimer = time;
            GetComponent<Player>().DisableAllInputs();
        }
    }
    
}
