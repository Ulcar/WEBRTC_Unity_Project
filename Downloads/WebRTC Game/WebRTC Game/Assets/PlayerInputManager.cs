using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

   public class PlayerInputManager:MonoBehaviour
    {

    [SerializeField]
    WebPeerConnection webRTC;


    [SerializeField]
    string clientID;


    [SerializeField]
    Camera cam;


    RTCInstance instance;

    public void OnGameStart() 
    {
        instance =  webRTC.GetInstanceByID(clientID);

        instance.AddCameraTrack(cam);

    }

    public void Update()
    {

    }

}
