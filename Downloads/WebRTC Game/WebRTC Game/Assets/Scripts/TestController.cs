/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TestController : MonoBehaviour
{
    // Start is called before the first frame update

    Rewired.Player player;
    TestMovement movement;
    TestCamera cam;
    [SerializeField]
    Camera activeCamera;
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        movement = GetComponent<TestMovement>();
        cam = GetComponent<TestCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = player.GetAxis2D("GroundMovementX", "GroundMovementY");


        Vector2 camera = player.GetAxis2D("CameraX", "CameraY");
        Debug.Log(camera);

        //camera forward and right vectors:
        var forward = activeCamera.transform.forward;
        var right = activeCamera.transform.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        if (dir.x != 0 || dir.y != 0)
        {

            movement.OnDirectionInput(forward * dir.y + right * dir.x);
        }

        else 
        {
            movement.OnDirectionInput(Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.OnKnockback(new Vector3(0, 0, -1), 1);
        }

    }
}
*/