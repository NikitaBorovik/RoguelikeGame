using Cinemachine;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [SerializeField] 
    private GameObject icon;
    [SerializeField]
    private Transform player;
    private void Start()
    {
        CinemachineVirtualCamera vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        vcam.Follow = player;
    }
    private void Update()
    {
        icon.transform.position = player.position;
    }
}
