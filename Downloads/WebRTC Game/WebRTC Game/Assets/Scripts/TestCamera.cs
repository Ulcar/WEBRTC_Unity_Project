using UnityEngine;
using System.Collections;
using Cinemachine;

public class TestCamera : MonoBehaviour
{



    [SerializeField]
    CinemachineVirtualCamera cam;

    [SerializeField]
    float sensitivity;

    [SerializeField]
    Transform lockonTarget;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }



    public void MoveCamera(Vector2 movement) 
    {
    }
}
