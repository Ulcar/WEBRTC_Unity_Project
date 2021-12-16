using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.WebRTC;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System;

public class WebPeerConnection : MonoBehaviour
{

    
    private List<RTCInstance> clientConnections = new List<RTCInstance>();
    private Coroutine sdpCheck;
    private RTCDataChannel dataChannel;

    [SerializeField]
    private WebSocketManager manager;
    [SerializeField]
    private ServerMessageHandler handler;

    [SerializeField]
    Camera cam;

    
    private RTCOfferOptions OfferOptions = new RTCOfferOptions
    {
        iceRestart = false,
        offerToReceiveAudio = true,
        offerToReceiveVideo = true
    };

    private RTCAnswerOptions AnswerOptions = new RTCAnswerOptions
    {
        iceRestart = false,
    };

    private void Awake()
    {
        WebRTC.Initialize(EncoderType.Hardware);

        StartCoroutine(WebRTC.Update());
    }

    private void OnDestroy()
    {

        WebRTC.Dispose();
    }

    private void Start()
    {
        handler.OnConnectionAccepted += OnPhoneClientConnected;
    }


    public RTCInstance GetInstanceByID(string ID) 
    {
        foreach (RTCInstance instance in clientConnections) 
        {
            if (ID == instance.clientID) 
            {
                return instance;
            }
        }

        return null;
    }



    public void OnSDPAnswer(string data, string clientID) 
    {
        StartCoroutine(OnSDPAnswerCourotine(data, clientID));
    }

    public IEnumerator OnSDPAnswerCourotine(string data, string clientID) 
    {
        Debug.Log("Received SDP answer");
        RTCConfiguration config = GetSelectedSdpSemantics();
        RTCInstance instance = GetInstanceByID(clientID);
        int waitCount = 0;
        while (instance == null)
        {
            // wait for creation of instance
            waitCount++;
            Debug.Log("waiting");
            yield return new WaitForSeconds(0.1f);
            instance = GetInstanceByID(clientID);
            if (waitCount > 100)
            {
                Debug.LogError("SDP Instance timed out");
            }
        }

        yield return null;
    }
    
    public void OnSDPMessage(string data, string clientID) 
    {
        StartCoroutine(OnSDPMessageCourotine(data, clientID));
        // h
    }


    public void SendSDPOffer(RTCInstance client) 
    {
        StartCoroutine(SendSDPCoroutine(client));
    }

    public IEnumerator SendSDPCoroutine(RTCInstance client) 
    {
        RTCOfferOptions OfferOptions = new RTCOfferOptions
        {
            offerToReceiveAudio = true,
            offerToReceiveVideo = true
        };

        
        var SDP = client.connection.CreateOffer(ref OfferOptions);
        yield return new WaitForSeconds(0.1f);
        SDPData data = new SDPData();
        data.clientID = client.clientID;
        data.SDP = SDP.Desc.sdp;
        data.type = "offer";
        var msg = new GameClientToServerMessage();
        msg.data = JsonConvert.SerializeObject(data);
        msg.status = 30;
        manager.SendWebSocketMessage(JsonConvert.SerializeObject(msg));
    }
    
    
    public IEnumerator OnSDPMessageCourotine(string data, string clientID) 
    {
        
        RTCConfiguration config = GetSelectedSdpSemantics();
        RTCInstance instance = GetInstanceByID(clientID);
        int waitCount = 0;
        while (instance == null)
        {
            // wait for creation of instance
            waitCount++;
            Debug.Log("waiting");
            yield return new WaitForSeconds(0.1f);
            instance = GetInstanceByID(clientID);
            if (waitCount > 100) 
            {
                Debug.LogError("SDP Instance timed out");
            }
        }
        SDPData sdp = JsonConvert.DeserializeObject<SDPData>(data);
        Debug.Log("Received SDP " + sdp.type);
        RTCSessionDescription desc = new RTCSessionDescription();
        desc.sdp = sdp.SDP;
        if (sdp.type == "offer")
        {
            desc.type = RTCSdpType.Offer;
            
        }

        else if (sdp.type == "answer") 
        {
            desc.type = RTCSdpType.Answer;
        }
        instance.connection.SetRemoteDescription(ref desc);



        RTCAnswerOptions options = new RTCAnswerOptions();
        options.iceRestart = false;

        if (desc.type == RTCSdpType.Offer)
        {
            Debug.Log("Creating SDP answer");
            var answer = instance.connection.CreateAnswer(ref options);
            yield return answer;
            RTCSessionDescription desc2 = answer.Desc;

            instance.connection.SetLocalDescription(ref desc2);
            Debug.Log("Sending SDP answer");
            GameClientToServerMessage msg = new GameClientToServerMessage();
            SDPData sdpdata = new SDPData();
            sdpdata.clientID = clientID;
            sdpdata.SDP = desc2.sdp;
            sdpdata.type = "answer";
            msg.data = JsonConvert.SerializeObject(sdpdata);
            msg.status = (int)GameToServerStatus.SDP;
            manager.SendWebSocketMessage(JsonConvert.SerializeObject(msg));
        }
        yield return null;
    }

    public void OnIceMessage(RTCIceCandidate candidate, string clientID) 
    {
        StartCoroutine(OnIcecMessageCourotine(candidate, clientID));
    }

    public IEnumerator OnIcecMessageCourotine(RTCIceCandidate candidate, string clientID) 
    {

        RTCInstance instance = null;
        Debug.Log("Adding Ice candidate");
        while (instance == null)
        {
            instance = GetInstanceByID(clientID);
            yield return new WaitForSeconds(0.1f);
        }
        instance.connection.AddIceCandidate(ref candidate);
    }

    RTCConfiguration GetSelectedSdpSemantics()
    {
        RTCConfiguration config = default;
        config.iceServers = new RTCIceServer[]
        {
            new RTCIceServer { urls = new string[] { "stun.l.google.com:19302" } },
            new RTCIceServer{  urls = new string[] { },  }
        };
        

        return config;
    }

    public void OnPhoneClientConnected(RoomToGameClientMessage msg)
    {
        RTCConfiguration config = GetSelectedSdpSemantics();
        RTCInstance instance = new RTCInstance(msg.clientID, manager, handler, this,  ref config);

        var gfxType = SystemInfo.graphicsDeviceType;
        var stream = cam.CaptureStream(1280, 720, 10000);
        RTCDataChannelInit options = new RTCDataChannelInit(true);

        //instance.connection.CreateDataChannel("gyro", ref options);
        foreach (var track in stream.GetTracks())
        {
            instance.connection.AddTrack(track, stream);

        } 

        clientConnections.Add(instance);
    }





}

public class ICEData
{
    public string candidate;
    public string clientID;
}




