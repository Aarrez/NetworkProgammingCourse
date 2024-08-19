using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class NetwrokManagerUI : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    public static UnityAction PuzzleCompleted;
    public static UnityAction UndoCompleted;
    public bool[] puzzles;

    private void Awake()
    {
        puzzles = new bool[2];
        puzzles[0] = false;
        puzzles[1] = false;
        networkManager = GetComponent<NetworkManager>();
       
    }
    private void Start()
    {
        networkManager.OnConnectionEvent += (m, s) => PlayerController.connectionEstablished?.Invoke();
    }

    private void OnEnable()
    {
        PuzzleCompleted = () =>
        {
            for (int i = 0; i < puzzles.Length -1; i++)
            {
                if (!puzzles[i])
                {
                    puzzles[i] = true;
                    break;
                }
            }
            CheckIfPuzzleIsSolved();
        };

        UndoCompleted = () =>
        {
            for (int i = 0; i < puzzles.Length - 1; i++)
            {
                if (puzzles[i])
                {
                    puzzles[i] = false;
                    break;
                }
            }
        };
    }

    private void CheckIfPuzzleIsSolved()
    {
        foreach (var variable in puzzles)
        {
            if (!variable)
                return;
        }
        Debug.Log("Puzzle is solved");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Host"))
        {
            networkManager.StartHost();
            
        }
        if (GUILayout.Button("Join"))
            networkManager.StartClient();
        
        if(GUILayout.Button("Quit"))
            Application.Quit();
    }
}
