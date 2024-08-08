using System;
using Unity.Netcode;
using UnityEngine;

public class NetwrokManagerUI : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    private void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
       
    }
    private void Start()
    {
        networkManager.OnConnectionEvent += (m, s) => PlayerController.connectionEstablished?.Invoke();

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
