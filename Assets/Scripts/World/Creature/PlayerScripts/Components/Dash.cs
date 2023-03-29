using App.World.Creatures.PlayerScripts.Components;
using App.World.Creatures.PlayerScripts.Events;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private DashEvent dashEvent;
    

    
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
        MakeDash(rb, args.direction, args.dashDistance, args.dashTime);
    }
    private void MakeDash(Rigidbody2D body, Vector2 direction, float distance, float time)
    {
         GetComponent<Player>().DisableAllInputs();
         float speed = distance / time;
         body.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }
    private void StopDash()
    {
        GetComponent<Player>().EnableAllInputs();
        GetComponent<Player>().PAnimator.SetBool("isDashing", false);
        GetComponent<Player>().ReloadDashTimer();
    }

}
