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
        inputField = FindObjectOfType<TMP_InputField>();
        text = GameObject.FindGameObjectWithTag("GameController").GetComponent<TextMeshProUGUI>();
        
        if (IsLocalPlayer)
        {
            if (inputField != null)
            {
                inputField.onSubmit.AddListener(OnSend);
            }
        }
    }

    private void OnSend(string message)
    {
        switch (System.Text.Encoding.Unicode.GetByteCount(message))
        {
            case 0: return;
            case < 29: 
                FixedString32Bytes messageToSend32 = new (message);
                SubmitMessageRPC(messageToSend32);
                break;
            case < 61:
                FixedString64Bytes messageToSend64 = new (message);
                SubmitMessageRPC(messageToSend64);
                break;
            case < 125:
                FixedString128Bytes messageToSend128 = new (message);
                SubmitMessageRPC(messageToSend128);
                break;
            default:
                string toLongMessage = "Message Is too long";
                inputField.text = toLongMessage;
                break;
        }
        
    }

    [Rpc(SendTo.Server)]
    public void SubmitMessageRPC(FixedString128Bytes message)
    {
        UpdateMessageRPC(message);
        Debug.Log("Message Sent");
    }

    [Rpc(SendTo.Everyone)]
    public void UpdateMessageRPC(FixedString128Bytes message)
    {
        text.text = "Player " + NetworkObject.NetworkObjectId+ ": " + message.ToString();
    }
}