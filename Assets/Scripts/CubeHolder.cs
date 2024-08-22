using UnityEngine;
using UnityEngine.InputSystem;

public class CubleHolder : MonoBehaviour
{
    public int colorNumber;
    
    private CubeManager cubeManager;
    private MeshRenderer meshRenderer;
    private Material[] matColors;
    private int arrayLength;
    private bool interact;
    private Vector3 cubePos;

    private void Start()
    {
        cubeManager = GetComponentInParent<CubeManager>();
        matColors = cubeManager.cubeColor;
        
        meshRenderer = GetComponent<MeshRenderer>();
        
        arrayLength = matColors.Length-1;
        for (int i = 0; i < matColors.Length-1; i++)
        {
            if (meshRenderer.material.name != matColors[i].name) continue;
            colorNumber = i;
            Debug.Log(colorNumber);
            break;
        }
        
    }

    private void OnEnable()
    {
        PlayerInput.InteractContext += ChangeColor;
    }

    private void OnDisable()
    {
        PlayerInput.InteractContext -= ChangeColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.transform.parent.CompareTag("Player")) return;
        interact = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.transform.parent.CompareTag("Player") && !interact) return;
        interact = false;
    }

    private void ChangeColor(InputAction.CallbackContext arg0)
    {
        if(!interact) return;
        colorNumber++;
        if (colorNumber > arrayLength)
            colorNumber = 0;
        meshRenderer.material = matColors[colorNumber];
        cubeManager.CorrectlyAligned();
    }
}