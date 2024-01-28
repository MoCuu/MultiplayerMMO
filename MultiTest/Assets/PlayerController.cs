using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Position.Value;
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPosition();
            transform.position = randomPosition;
            Position.Value = randomPosition; // Gracze czytaj¹ to
        }
        else
        {
            SubmitPositionRequestServerRpc(); // Rpc = domaganie siê od serwera wykonania funkcji
        }    
    }

    static Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-3, 3), 1, Random.Range(-3, 3));
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = GetRandomPosition(); // Modyfikacja pozycji sieciowej
    }
}
