using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestMovement : MonoBehaviour
{
    // Start is called before the first frame update



    
    
    [SerializeField]
    float maxGroundSpeed = 2f;
    float initialSpeed = 0.2f;
    [SerializeField]
    float acceleration = 1f;


    [SerializeField]
    float Deacceleration = -0.5f;
    [SerializeField]
    float neutralDeacceleration = -1f;
    [SerializeField]
    float currentSpeed;


    [SerializeField]
    float gravityAcceleration;

    [SerializeField]
    float currentGravitySpeed;



    [SerializeField]
    Vector3 direction;
    [SerializeField]
    Vector3 targetDirection;

    Vector3 velocity;

    Vector3 knockbackDelta;

    float maxAngular = 180 * 0.0174532925f;

    bool canMove = true;
    float hitStunTime = 0;


    [SerializeField]
    bool onGround = true;


    void Start()
    {
        currentSpeed = 0;
        onGround = true;
      
      
    }

    // Update is called once per frame
    void Update()
    {




        if (canMove)
        {
            if (targetDirection != Vector3.zero)
            {




                if (Vector3.Dot(this.direction, targetDirection) <= -0.6)
                {
                    if (currentSpeed > 0)
                    {
                        currentSpeed += Deacceleration * Time.deltaTime;
                    }

                    else
                    {
                        direction = targetDirection;
                        currentSpeed = 0;
                    }
                }

                else if (currentSpeed < maxGroundSpeed)
                {
                    currentSpeed += acceleration * Time.deltaTime;
                }

                if (Vector3.Angle(this.direction, targetDirection) < 90)
                {
                    this.direction = Vector3.RotateTowards(this.direction, targetDirection, maxAngular * Time.deltaTime, 1);
                    this.direction.Normalize();
                }



            }

            else if (currentSpeed > 0)
            {


                currentSpeed += neutralDeacceleration * Time.deltaTime;
            }

            if (currentSpeed < 0.1f)
            {
                this.direction = targetDirection;
            }



            if (currentSpeed < 0)
            {
                currentSpeed = 0;
            }

            if (!onGround)
            {


                if (currentGravitySpeed <= 0)
                {
                    RaycastHit hit;
                    if (RayCastGround(out hit))
                    {
                        onGround = true;
                        transform.position += transform.position - hit.point;
                        currentGravitySpeed = 0;
                    }

                    else
                    {
                        currentGravitySpeed += gravityAcceleration * Time.deltaTime;
                    }

                }

            }

            else
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position, -transform.up);
                Debug.DrawRay(transform.position, -transform.up * 1f, Color.red);
                if (!Physics.Raycast(ray, out hit, 1f))
                {
                    onGround = false;
                }
            }


            velocity = (direction * currentSpeed);
            velocity = new Vector3(velocity.x, currentGravitySpeed, velocity.z);


        }
        else
        {
            hitStunTime -= Time.deltaTime;
            velocity -= knockbackDelta * Time.deltaTime;
            if (hitStunTime <= 0)
            {
                canMove = true;
            }
        }


        transform.localPosition += velocity;
    }



    public void OnDirectionInput(Vector3 direction) 
    {


        this.targetDirection = direction;


        // if not standing still slowly rotate towards direction
       
       
    }

    
    bool RayCastGround(out RaycastHit hit) 
    {
        Ray ray = new Ray(transform.position, -transform.up);
        Debug.DrawRay(transform.position, transform.up * currentGravitySpeed, Color.red);
        return Physics.Raycast(ray, out hit, -currentGravitySpeed);
    }


    public void OnKnockback(Vector3 knockBack, float hitStunTime) 
    {
        velocity += knockBack;
        this.hitStunTime = hitStunTime;
        knockbackDelta = knockbackDelta / hitStunTime;
        canMove = false;
    }
}
