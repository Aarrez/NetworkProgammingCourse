using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class MultiplayerChat : NetworkBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    
    [SerializeField] private TextMeshProUGUI text;
    private void Start()
    {
        if (inputField != null)
        {
            inputField.onSubmit.AddListener(OnSend());
            
        }
        SubmitMessageRPC("Hello World!");
    }

    private void OnSend(string message)
    {
        
    }

    [Rpc(SendTo.Server)]
    public void SubmitMessageRPC(FixedString128Bytes message)
    {
        UpdateMessageRPC(message);
    }

    [Rpc(SendTo.Everyone)]
    public void UpdateMessageRPC(FixedString128Bytes message)
    {
        text.text = message.ToString();
    }
}