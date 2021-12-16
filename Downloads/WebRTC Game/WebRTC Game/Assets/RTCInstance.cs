using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity.WebRTC;
using Newtonsoft.Json;



public abstract class RTCConnection 
{

}




//TODO: Build abstraction layer on top of this class to switch between Unity plugin and javascript

//TODO: Make all external class references into events, so this class can work without the external classes existing

public class RTCInstance
{
    //should be some kind of interface
    public RTCPeerConnection connection;
    public string clientID;
    WebSocketManager manager;
    ServerMessageHandler handler;
    WebPeerConnection webRTCHandler;
    public bool inOffer = false;

    public delegate void OnJoystick(byte[] data);
    public event OnJoystick OnJoystickEvent;
    private Camera associatedCamera;

    public delegate void OnJoyStickConnected(string clientID);
    public event OnJoyStickConnected OnJoystickConnectedEvent;


    DelegateOnIceCandidate delegateOnIceCandidate;
    DelegateOnTrack delegateOnTrack;
    DelegateOnDataChannel delegateOnDataChannel;
    DelegateOnNegotiationNeeded delegateOnNegotiationNeeded;
    DelegateOnIceConnectionChange delegateOnIceConnectionChange;



    public RTCInstance(string clientID, WebSocketManager manager, ServerMessageHandler handler, WebPeerConnection conn, ref RTCConfiguration config)
    {
        this.clientID = clientID;
        RTCConfiguration configh = GetSelectedSdpSemantics();
        connection = new RTCPeerConnection(ref configh);
        this.manager = manager;
        this.handler = handler;
        this.webRTCHandler = conn;
        delegateOnIceCandidate += OnIceCandidate;
        delegateOnTrack += OnTrack;
        delegateOnDataChannel += OnDataChannel;
        delegateOnNegotiationNeeded = OnNegotiation;
        connection.OnIceCandidate = delegateOnIceCandidate;
        connection.OnTrack = delegateOnTrack;
        connection.OnDataChannel = delegateOnDataChannel;
        //connection.OnNegotiationNeeded = delegateOnNegotiationNeeded;
        connection.OnIceConnectionChange = delegateOnIceConnectionChange; 


    }





    public void OnIceConnectionChange(RTCIceConnectionState state)
    {
        Debug.LogWarning("Current ICE state: " + state);
    }

    public void OnIceCandidate(RTCIceCandidate​ candidate)
    {
        //GetOtherPc(pc).AddIceCandidate(ref candidate);
        //Debug.Log($"{GetName(pc)} ICE candidate:\n {candidate.candidate}");
        Debug.Log("Sending ICE candidate");
        GameClientToServerMessage msg = new GameClientToServerMessage();
        ICEData data = new ICEData();
        data.candidate = JsonConvert.SerializeObject(candidate);
        data.clientID = clientID;
        msg.data = JsonConvert.SerializeObject(data);
        msg.status = 31;
        manager.SendWebSocketMessage(JsonConvert.SerializeObject(msg));
    }




    public void OnNegotiation()
    {
        Debug.Log("Negotiating");
        inOffer = true;
        this.SendSDPOffer();
    }

    public void AddCameraTrack(Camera cam, int width = 1280, int height = 720, int bitrate = 10000)
    {
        associatedCamera = cam;
        Debug.Log("Adding camera");


        var stream = cam.CaptureStream(width, height, bitrate);
        foreach (var track in stream.GetTracks())
        {
            connection.AddTrack(track, stream);

        }
    }

    public void OnTrack(RTCTrackEvent e)
    {
        Debug.Log("Got track");
    }

    public void SendSDPOffer()
    {
        // I can't do coroutines in a class lol
        // maybe make these scriptable objects, so I don't have to link back?
        webRTCHandler.SendSDPOffer(this);


    }



    public void OnDataChannel(RTCDataChannel channel)
    {

        Debug.Log("Got data channel");
        channel.OnOpen += OnDataChannelOpen;
        channel.OnClose += OnDataChannelClose;
        if (channel.Label == "gyro")
        {
            channel.OnMessage += OnGyroMessage;
        }

        else if (channel.Label == "joystick1")
        {
            channel.OnMessage += OnJoystickMessage;
        }

        else
        {
            channel.OnMessage += OnStringMessage;
        }
    }

    public void OnGyroMessage(byte[] data)
    {
        handler.PhoneGyroData(clientID, data);
    }

    public void OnJoystickMessage(byte[] data)
    {
        OnJoystickEvent?.Invoke(data);
    }

    public void OnStringMessage(byte[] data)
    {
        string msg = System.Text.Encoding.UTF8.GetString(data);
        handler.PhoneMessageHandler(clientID, msg);
    }

    public void OnDataChannelOpen()
    {
        Debug.Log("Data channel open");
    }
    public void OnDataChannelClose()
    {
        Debug.Log("Data channel close");
    }

    static RTCConfiguration GetSelectedSdpSemantics()
    {
        RTCConfiguration config = default;
        config.iceServers = new RTCIceServer[]
        {
            new RTCIceServer { urls = new string[] { "stun:stun.l.google.com:19302" } }
        };


        return config;
    }
}
