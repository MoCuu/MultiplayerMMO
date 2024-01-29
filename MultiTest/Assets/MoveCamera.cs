using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MoveCamera : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Transform cameraPosition;

    public override void OnNetworkSpawn()
    {
        cameraHolder.SetActive(IsOwner);
        base.OnNetworkSpawn();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
