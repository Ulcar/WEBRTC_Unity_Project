using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.WebRTC;
using UnityEngine;

public class ServerMessageHandler:MonoBehaviour
    {
    [SerializeField]
    WebSocketManager manager;

    [SerializeField]
    TMPro.TMP_Text RoomText;

    [SerializeField]
    WebPeerConnection webRTC;
    
    
    public delegate void GyroDelegate(GyroToGameClientMessage msg, string clientID);
    public event GyroDelegate OnGyroReceived;

    public delegate void ServerMessageDelegate(RoomToGameClientMessage msg);
    public event ServerMessageDelegate OnConnectionAccepted;

    public void PhoneMessageHandler(string clientID, string msg) 
    {
        Debug.Log("Message: + " + msg + " ID: " + clientID);
    }

    public void PhoneGyroData(string ID, byte[] data) 
    {
        Debug.Log("Gyro Data: " + data + " ID: " + ID);

        string dataAsString = System.Text.Encoding.UTF8.GetString(data);
        GyroToGameClientMessage msg = JsonConvert.DeserializeObject<GyroToGameClientMessage>(dataAsString);

        OnGyroReceived.Invoke(msg, ID);
    }
    
    public void OnServerMessage(string textmsg) 
    {

        RoomToGameClientMessage msg = JsonConvert.DeserializeObject<RoomToGameClientMessage>(textmsg);
       // Debug.Log("Message ID: " + msg.id + " Client ID: " + msg.clientID + "data: " + msg.data);

        

        if (msg.id == MessageID.Gyro)
        {
            GyroToGameClientMessage convertedmsg = JsonConvert.DeserializeObject<GyroToGameClientMessage>(textmsg);
        }

        else if (msg.id == MessageID.ConnectionAccepted)
        {
            OnConnectionAccepted?.Invoke(msg);
        }

        else if (msg.id == MessageID.RoomCreate)
        {
            RoomText.SetText(msg.data);
            RoomText.ForceMeshUpdate();
        }

        else if (msg.id == MessageID.SDP)
        {
            webRTC.OnSDPMessage(msg.data, msg.clientID);
        }

        else if (msg.id == MessageID.ICE) 
        {

            JObject convertedobj = JObject.Parse(textmsg);
            string data = convertedobj.Value<string>("data");
            Debug.Log(data);
            if (data != null && data != "" && data != "null")
            {
                RTCIceCandidate candidate = JsonConvert.DeserializeObject<RTCIceCandidate>(data);
                webRTC.OnIceMessage(candidate, msg.clientID);
            }
            else 
            {
                Debug.LogWarning("ICE data was null");
            }
        }
    }

    private void Start()
    {
        manager.OnMessageEvent += OnServerMessage;
    }
}



