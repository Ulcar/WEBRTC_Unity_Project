using UnityEngine;
using System.Collections;
using Mirror;
public class EpicNetworkManager :NetworkManager
{

    // Use this for initialization

    NetworkClient localClient;

 public  override void Start()
    {
        base.Start();


      StartHost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
