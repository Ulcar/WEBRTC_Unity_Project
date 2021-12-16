using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System;
using Newtonsoft.Json;

public class WebSocketManager : MonoBehaviour
{
    // Start is called before the first frame update

    public delegate void OnOpened();
    public event OnOpened OnOpenedEvent;

    public delegate void onClosed();
    public event onClosed OnClosedEvent;

    public delegate void OnMessage(string msg);

    public event OnMessage OnMessageEvent;


    WebSocket websocket;

    async void Start()
    {

       websocket  = new WebSocket("wss://ulcargames.com:24669/GameClientEndPoint?");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
             var message = System.Text.Encoding.UTF8.GetString(bytes);
             Debug.Log("OnMessage! " + message);
            OnMessageEvent?.Invoke(message);
        };

        await websocket.Connect();

    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }
    private void websocket_Opened(EventArgs e)
    {
        Debug.Log("Websocket is opened.");
        OnOpenedEvent?.Invoke();
    }

    private void websocket_Closed(object sender, EventArgs e)
    {
        Debug.Log("Websocket is closed.");
        OnClosedEvent?.Invoke();
    }

  
    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    public void SendWebSocketMessage(string text) 
    {
        websocket.SendText(text);
    }
}
