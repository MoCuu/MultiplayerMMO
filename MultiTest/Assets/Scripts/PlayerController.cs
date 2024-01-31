using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Just a crappy character controller for the video
/// </summary>
public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    #region Movement

    [SerializeField] private float _acceleration = 80;
    [SerializeField] private float _maxVelocity = 10;
    private Vector3 _input;
    private Rigidbody _rb;

    private void HandleMovement()
    {
        _rb.velocity += _input.normalized * (_acceleration * Time.deltaTime);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVelocity);
    }

    #endregion
}
