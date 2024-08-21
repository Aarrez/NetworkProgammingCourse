using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;



public class NetworkLevelManager : NetworkBehaviour
{
    public static UnityAction PuzzleCompleted;
    public static UnityAction UndoCompleted;
    [SerializeField] private CubeManager[] cubeManagers;
    
    NetworkVariable<int> puzzleVariable = 
        new NetworkVariable<int>();

    private void OnServerInitialized()
    {
        if (IsSpawned)
        {
            cubeManagers = FindObjectsByType<CubeManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            puzzleVariable.OnValueChanged += CheckIfPuzzleIsSolved;
        }
    }

    private void OnEnable()
    {
        PuzzleCompleted = () =>
        {
            if (IsServer)
            {
                if (puzzleVariable.Value < cubeManagers.Length)
                {
                    puzzleVariable.Value++;
                }
            }
        };

        UndoCompleted = () =>
        {
            if (IsServer)
            {
                if (puzzleVariable.Value > 0)
                {
                    puzzleVariable.Value--;
                }
            }
        };
    }
    
    private void CheckIfPuzzleIsSolved(int oldValue, int newValue)
    {
        Debug.Log("Happeing");
        if(newValue >= cubeManagers.Length)
            Debug.Log("Puzzle is solved");
    }
}