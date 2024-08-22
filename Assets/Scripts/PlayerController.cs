using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour
{
    public static UnityAction DestroyPlayer;
    
    
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Material[] playerColors;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotSpeed = 1000f;
    
    private Rigidbody _rigidbody;
    private GameObject Spawnpoints; 
    private MeshRenderer playerMesh;
    private NetworkObject netObject;
    private Vector3 moveVector;
    private Vector2 rotation;
    private Coroutine coUpdate;

    private NetworkVariable<Vector3> moveInput =
        new NetworkVariable<Vector3>();

    private NetworkVariable<Vector2> rotInput = 
        new NetworkVariable<Vector2>();
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Spawnpoints = GameObject.FindGameObjectWithTag("SpawnPoints");
        netObject = GetComponent<NetworkObject>();
        playerMesh = GetComponentInChildren<MeshRenderer>();

        DestroyPlayer = LevelComplete;

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
        rotation = ctx.ReadValue<Vector2>();
        InputRPC(moveVector, rotation);
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
            
            if (rotInput.Value.x != 0)
            {
                _rigidbody.rotation *= Quaternion.Euler(0, rotInput.Value.x * (rotSpeed * Time.deltaTime), 0);
            }
        }
    }
    
    [Rpc(SendTo.Server)]
    private void InputRPC(Vector3 moveData, Vector2 rotData)
    {
        moveInput.Value = moveData;
        rotInput.Value = rotData;
    }

    private void LevelComplete()
    {
        Destroy(gameObject);
    }
    
}