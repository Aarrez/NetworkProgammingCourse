using System;
using UnityEngine;
using TMPro;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = FindObjectOfType<TextMeshPro>();
    }

    public void EnableUIText()
    {
        textMesh.enabled = true;
    }

    private void DisableUIText()
    {
        textMesh.enabled = false;
    }
}