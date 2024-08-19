using System;
using Unity.Netcode;
using UnityEngine;




public class CubeManager : NetworkBehaviour
{ 
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
                    NetwrokManagerUI.PuzzleCompleted?.Invoke();
                }
            }
            else if (completed)
            {
                completed = false;
                NetwrokManagerUI.UndoCompleted?.Invoke();
                return;
            }
        }
       
    }
}