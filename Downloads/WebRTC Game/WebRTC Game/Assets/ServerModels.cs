using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.WebRTC;
using UnityEngine.Networking;

public enum ConnectStateMessagesPhone
{
    Invaild = 0,
    Connect = 4,
    SetName = 5,
    Start = 6,
    Message = 7,


    StartGame = 10,

    Handiness = 20,
    Gyro = 21

}

public enum ServerToPhoneClientStatus
{
    Invaild = 0,
    ConnectionAccepted = 5,
    IDMessage = 2,
    ConnectionRefused = 1,
    GameConnectionAccepted = 6,
    Start = 10,
    Gyro = 20
}

public class ServerToPhoneClientMessage
{
    public ServerToPhoneClientStatus status;
    public string data;
}


public class PhoneClientToServerMessage
{
    public int message;
    public string data;
    public string clientID;
    public string room;


}


public class GyroPhoneClientToServerMessage : PhoneClientToServerMessage
{
    public float gyro_x;
    public float gyro_y;
    public float gyro_z;


    public float x;
    public float y;
    public float z;

}

[Serializable]
public class GyroToGameClientMessage
{
    public float x;
    public float y;
    public float z;

    public float gyro_x;
    public float gyro_y;
    public float gyro_z;
}


public class RoomToGameClientMessage
{
    public string data;
    public string clientID;
    public MessageID id = MessageID.Invaild;

}

public enum MessageID
{
    Invaild = 0,
    RoomCreate = 1,
    ConnectionAccepted = 5,
    Gyro = 20,
    SDP = 30,
    ICE = 31


}


public enum GameToServerStatus
{
    SDP = 30,
    ICE = 31
}
public class GameClientToServerMessage
{
    public int status;
    public string data;
}


public class SDPData
{
    public string SDP;
    public string clientID;
    public string type;
}


public class ServerICEMessage 
{
    public RTCIceCandidateClass data;
    public string clientID;
}

public class RTCIceCandidateClass
{
    public string candidate;
    public string sdpMid;
    public int sdpMLineIndex;
}

