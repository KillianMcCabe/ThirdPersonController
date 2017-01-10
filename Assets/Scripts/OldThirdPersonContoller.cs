using UnityEngine;
using System.Collections;

public class OldThirdPersonContoller : MonoBehaviour {

    Camera cam;
    Animator animator;
    CharacterController controller;
    
    float speed = 5.0f;
    float gravity = 9.8f;
    float vSpeed = 0; // current vertical velocity

    bool inAir = false;

    Vector3 movement = Vector3.zero;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("parry") || animator.GetCurrentAnimatorStateInfo(0).IsName("jump") )
        {
            animator.speed = 1;

            
            ApplyGravity();
            return;
        }

        // add gravity to vertical movement
        if (controller.isGrounded)
        {
            vSpeed = 0; // grounded character has vSpeed = 0...

            if (inAir)
            {
                animator.SetTrigger("land");
                print("land");
                inAir = false;
            }
            movement = Vector3.zero;
            HandleMovementInput();
            HandleActionInputs();
        }
        else
        {
            // no input and keep movement
            //inAir = true;
        }

        //controller.Move(movement * Time.deltaTime);
        ApplyGravity();

        //print("v: " + vertical + ", s: " + horizontal + ", mag: " + movement.magnitude);
    }

    void HandleMovementInput()
    {
        // read input
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        // calculate movement vectors
        Vector3 verticalVector = Vector3.Cross(cam.transform.right, Vector3.up);
        Vector3 horizontalVector = (new Vector3(cam.transform.right.x, 0, cam.transform.right.z));

        movement += (verticalVector * vertical * speed);
        movement += (horizontalVector * horizontal * speed);
        //movement = Vector3.ClampMagnitude(movement, maxSpeed);

        // turn in direction of movement input
        if (vertical != 0 || horizontal != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        // set animator variables
        Vector3 nonVerticalSpeed = movement;
        nonVerticalSpeed.y = 0;
        animator.SetFloat("speed", nonVerticalSpeed.magnitude);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            animator.speed = Mathf.Max(0.5f, movement.magnitude);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            animator.speed = Mathf.Max(0.5f, movement.magnitude / 4);
        }
        else
        {
            animator.speed = 1;
        }
    }

    void HandleActionInputs()
    {
        // attack
        if (Input.GetButtonDown("Fire3"))
        {
            animator.SetTrigger("attack");
        }
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("parry");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("jump");
            animator.ResetTrigger("land");
        }
    }

    // called from animation event
    void Jump()
    {
        inAir = true;
    }

    void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            vSpeed = 0; // grounded character has vSpeed = 0...

            if (inAir)
            {
                animator.SetTrigger("land");
                inAir = false;
            }
        }
        
        vSpeed -= gravity * Time.deltaTime;

		Vector3 vMovement = Vector3.zero;
        vMovement.y = vSpeed;

        //controller.Move(vMovement * Time.deltaTime);
    }

}
