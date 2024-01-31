using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private bool _serverAuth;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;

    private NetworkVariable<PlayerNetworkState> _playerState;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _playerState = new NetworkVariable<PlayerNetworkState>(writePerm: permission);
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(transform.GetComponent<PlayerController>());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner) TransmitState();
        else ConsumeState();
    }

    private void TransmitState()
    {
        var state = new PlayerNetworkState { Position = _rb.position };

        if (IsServer || !_serverAuth)
        {
            _playerState.Value = state;
        }
        else
        {
            TransmitStateServerRpc(state);
        }
    }

    [ServerRpc]
    private void TransmitStateServerRpc(PlayerNetworkState state)
    {
        _playerState.Value = state;
    }

    private Vector3 _posVel;

    private void ConsumeState()
    {
        _rb.MovePosition(Vector3.SmoothDamp(_rb.position, _playerState.Value.Position, ref _posVel, _cheapInterpolationTime));
    }

    struct PlayerNetworkState : INetworkSerializable
    {
        private float _x, _z;

        internal Vector3 Position
        {
            get => new Vector3(_x, 0, _z);
            set 
            {
                _x = value.x;
                _z = value.z;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _z);
        }
    }
}
