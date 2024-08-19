using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour
{
    public static UnityAction connectionEstablished;
    
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Material[] playerColors;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotSpeed = 1000f;
    
    private Rigidbody _rigidbody;
    private GameObject Spawnpoints; 
    private MeshRenderer playerMesh;
    private NetworkObject netObject;
    private Vector3 moveVector;
    private float rotation;
    private Coroutine coUpdate;

    private NetworkVariable<Vector3> moveInput =
        new NetworkVariable<Vector3>();
    private NetworkVariable<float> rotInput =
        new NetworkVariable<float>();
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Spawnpoints = GameObject.FindGameObjectWithTag("SpawnPoints");
        netObject = GetComponent<NetworkObject>();
        playerMesh = GetComponentInChildren<MeshRenderer>();

        if (IsLocalPlayer)
        {
            PlayerInput.MovementContext += ConvertMoveInput;
            PlayerInput.SpaceContext += SpaceInput;
        }
            

        if (netObject.NetworkObjectId == 1)
        {
            playerMesh.material = playerColors[0];
            transform.position = Spawnpoints.transform.GetChild(0).position;
        }
        else
        {
            transform.position = Spawnpoints.transform.GetChild(1).position;
            playerMesh.material = playerColors[1];
        }
    }
    
    private void ConvertMoveInput(InputAction.CallbackContext ctx)
    {
        moveVector = playerCamera.transform.forward * ctx.ReadValue<Vector2>().y;
        rotation = ctx.ReadValue<Vector2>().x;
        MoveRPC(moveVector);
        RotRPC(rotation);
    }

    private void SpaceInput(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
            moveSpeed += 5f;
        else if(ctx.canceled)
            moveSpeed -= 5f;
    }
    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (IsServer)
        {
            _rigidbody.position += moveInput.Value * (Time.deltaTime * moveSpeed);
            if (rotation != 0)
                _rigidbody.rotation *= Quaternion.Euler(0, rotInput.Value * (rotSpeed * Time.deltaTime), 0);
        }
            
    }
    
    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector3 data)
    {
        moveInput.Value = data;
    }

    [Rpc(SendTo.Server)]
    private void RotRPC(float data)
    {
        rotInput.Value = data;
    }
}