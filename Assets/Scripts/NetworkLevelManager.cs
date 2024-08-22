using System;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;



public class NetworkLevelManager : NetworkBehaviour
{
    public static UnityAction PuzzleCompleted;
    public static UnityAction UndoCompleted;
    [SerializeField] private CubeManager[] cubeManagers;
    private NetworkManager manager;
    
    NetworkVariable<int> puzzleVariable = 
        new NetworkVariable<int>();

    private void Start()
    {
        manager = FindObjectOfType<NetworkManager>();
        cubeManagers = FindObjectsByType<CubeManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    private void OnEnable()
    {
        puzzleVariable.OnValueChanged += CheckIfPuzzleIsSolved;
        
        PuzzleCompleted = () =>
        {
            
                if (puzzleVariable.Value < cubeManagers.Length)
                {
                    int temp = puzzleVariable.Value;
                    temp++;
                    PuzzleVariableRPC(temp);
                }
        };

        UndoCompleted = () =>
        {
                if (puzzleVariable.Value > 0)
                {
                    int temp = puzzleVariable.Value;
                    temp--;
                    PuzzleVariableRPC(temp);
                }
        };
    }

    [Rpc(SendTo.Server)]
    private void PuzzleVariableRPC(int data)
    {
        Debug.Log("PuzzleVariableRPC");
        puzzleVariable.Value = data;
    }

    [Rpc(SendTo.Everyone)]
    private void PuzzleCompletedRPC()
    {
        Debug.Log("Puzzle is solved");
        manager.Shutdown();
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
    
    private void CheckIfPuzzleIsSolved(int oldValue, int newValue)
    {
        if (newValue >= cubeManagers.Length)
        {
            PuzzleCompletedRPC();
        }
    }
}