using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour
{
    public static UnityAction connectionEstablished;
    
    [SerializeField] private Material[] playerColors;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotSpeed = 1000f;

    private Rigidbody _rigidbody;
    private GameObject Spawnpoints; 
    private MeshRenderer playerMesh;
    private NetworkObject netObject;
    private Vector3 moveVector;
    private Coroutine coUpdate;

    private NetworkVariable<Vector3> moveInput =
        new NetworkVariable<Vector3>();
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Spawnpoints = GameObject.FindGameObjectWithTag("SpawnPoints");
        netObject = GetComponent<NetworkObject>();
        playerMesh = GetComponentInChildren<MeshRenderer>();
        connectionEstablished += PlayerConnected;

        if (IsLocalPlayer)
        {
            PlayerInput.MovementContext += ConvertMoveInput;
            PlayerInput.SpaceContext += SpaceInput;
            /*PlayerInput.InteractContext +=*/
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

    private void PlayerConnected()
    {
        Debug.Log("happend now");
    }
    
    private void ConvertMoveInput(InputAction.CallbackContext ctx)
    {
        moveVector = new Vector3(
            ctx.ReadValue<Vector2>().x, 
            0f, 
            ctx.ReadValue<Vector2>().y);
        MoveRPC(moveVector);
    }

    private void InteractInput(InputAction.CallbackContext ctx)
    {
        
    }

    private void SpaceInput(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
            moveSpeed += 5f;
        else if(ctx.canceled)
        {
            moveSpeed -= 5f;
        }
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
            if(moveVector != Vector3.zero)
            {
                _rigidbody.rotation =
                    Quaternion.RotateTowards(transform.rotation, 
                        Quaternion.LookRotation(moveVector), 
                        rotSpeed * Time.deltaTime);


            }
        }
            
    }
    
    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector3 data)
    {
        moveInput.Value = data;
    }
}