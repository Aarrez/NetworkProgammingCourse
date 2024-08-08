using System;
using Unity.Netcode;
using UnityEngine;

public enum CubeColors
{
    Green,
    Yellow,
    Purple,
    LightBlue
}

public class CubleHolder : NetworkBehaviour
{
    [SerializeField] private CubeColors color;
    private Vector3 cubePos; 

    private void Start()
    {
        cubePos = transform.GetChild(0).position;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}