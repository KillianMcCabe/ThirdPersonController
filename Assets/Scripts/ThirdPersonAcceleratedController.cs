using UnityEngine;
using System.Collections;

public class ThirdPersonAcceleratedContoller : MonoBehaviour
{

    Camera cam;
    Animator animator;

    float accelerationRate = 10.0f;
    float decelerationRate = 20.0f;

    float maxSpeed = 0.4f;

    Vector3 movement = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");


        // calculate movement vectors
        Vector3 verticalVector = Vector3.Cross(cam.transform.right, Vector3.up);
        Vector3 horizontalVector = (new Vector3(cam.transform.right.x, 0, cam.transform.right.z));

        if (vertical != 0)
        {
            //add to the current velocity according while accelerating
            movement += (verticalVector * vertical * accelerationRate * Time.deltaTime);
        }
        else
        {
            //subtract from the current velocity while decelerating

            movement -= (verticalVector * decelerationRate * Time.deltaTime);
        }

        if (horizontal != 0)
        {
            //add to the current velocity according while accelerating
            movement += (horizontalVector * horizontal * accelerationRate * Time.deltaTime);
        }
        else
        {
            //subtract from the current velocity while decelerating
            movement -= (horizontalVector * decelerationRate * Time.deltaTime);
        }

        movement = Vector3.ClampMagnitude(movement, maxSpeed);


        // turn in direction of movement input
        if (vertical != 0 || horizontal != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        // move
        transform.position += movement;

        // set animator variables
        animator.SetFloat("speed", movement.magnitude);
        print(movement.magnitude);
    }
}
