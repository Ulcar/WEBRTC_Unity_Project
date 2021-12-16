using UnityEngine;
using System.Collections;
public class GyroControl : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    ServerMessageHandler server;

    [SerializeField]
    Quaternion offset;

    [SerializeField]
  public  string clientID = "";

    [SerializeField]
    GyroToGameClientMessage lastmsg;

    [SerializeField]
    Vector3 rotatedAccel;



    void Start()
    {
        server.OnGyroReceived += OnGyroDataReceived;
    }


    public void OnGyroDataReceived(GyroToGameClientMessage msg, string clientID) 
    {
        if (this.clientID == clientID) 
        {
            // update gyro data on object

            transform.rotation = Quaternion.Euler(msg.gyro_y, msg.gyro_x, msg.gyro_z) * offset;
            lastmsg = msg;

        }



        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            offset = Quaternion.Inverse(Quaternion.Euler(msg.gyro_y, msg.gyro_x, msg.gyro_z));
        }

        rotatedAccel = Vector3.RotateTowards(new Vector3(msg.x, msg.y, msg.z), offset.eulerAngles, 360, 1);

        if (rotatedAccel.x > 10 || rotatedAccel.x < -10) 
        {
            Debug.Log("Swing detected");
        }


        
    }
    // Update is called once per frame
    void Update()
    {

    }
}
