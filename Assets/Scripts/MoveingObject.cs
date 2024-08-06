using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class MoveingObject : NetworkBehaviour
{
    private InputMappingContext inputMapping;

    [SerializeField] private float speed = 2f;
    private bool shouldMove = false;

    private void Awake()
    {
        inputMapping = new InputMappingContext();
    }

    private void OnEnable()
    {
        inputMapping.PlayerInput.Movement.performed += ctx =>
        {
            
            shouldMove = true;
            StartCoroutine(MovePlayer(new Vector3(ctx.ReadValue<Vector2>().x, 0f, ctx.ReadValue<Vector2>().y)));
        };
        inputMapping.PlayerInput.Movement.canceled += _ =>
        {
            shouldMove = false;
            StopCoroutine(MovePlayer(Vector3.zero));
        };
        inputMapping.PlayerInput.Enable();
    }

    private void OnDisable()
    {
        inputMapping.Disable();
    }

    private IEnumerator MovePlayer(Vector3 MoveVector)
    {
        while (MoveVector != Vector3.zero)
        {
            yield return new WaitForFixedUpdate();
            transform.position += MoveVector * (Time.deltaTime * speed);
        }
    }
}