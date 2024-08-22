using System;
using Unity.Netcode;
using UnityEngine;

public class CubeManager : NetworkBehaviour
{ 
    
    [SerializeField] private GameObject cubePrefab;
    
    public Material[] cubeColor;

    [SerializeField] private CubeColors[] cubeColors;
    
    private CubleHolder[] holders;

    private bool completed;

    private void Start()
    {
        holders = GetComponentsInChildren<CubleHolder>();
    }
    
    public void CorrectlyAligned()
    {
        int j = 0;
        for (int i = 0; i < cubeColors.Length; i++)
        {
            if ((int)cubeColors[i] == holders[i].colorNumber)
            {
                j++;
                if (j == cubeColors.Length)
                {
                    completed = true;
                    Debug.Log("Puzzle completed");
                    NetworkLevelManager.PuzzleCompleted?.Invoke();
                    cubePrefab.SetActive(true);
                }
            }
            else if (completed)
            {
                completed = false;
                NetworkLevelManager.UndoCompleted?.Invoke();
                cubePrefab.SetActive(false);
                return;
            }
        }
       
    }
   
}