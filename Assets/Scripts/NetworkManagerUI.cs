using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
   
    private void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
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
