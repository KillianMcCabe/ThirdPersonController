﻿using UnityEngine;
using System.Collections;

public class ThirdPersonContoller : MonoBehaviour {

    Camera cam;
    Animator animator;
    CharacterController controller;
    
    float gravity = 9.8f;
    float vSpeed = 0; // current vertical velocity

    Vector3 movement = Vector3.zero;
	public bool isGrounded = false;

	public AnimationCurve jumpColliderScale;

	float startHeight = 0f;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

		startHeight = controller.height;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("jump") )
        {
            ApplyGravity();
            return;
        }
		isGrounded = controller.isGrounded;
        if (controller.isGrounded)
        {
            vSpeed = 0; // grounded character has vSpeed = 0...
            movement = Vector3.zero;
			HandleActionInputs();
            HandleMovementInput();
            
        }
        else
        {
            // no input and keep movement
        }

		ApplyGravity();
    }

    void HandleMovementInput()
    {
        // read input
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        // calculate movement vectors
        Vector3 verticalVector = Vector3.Cross(cam.transform.right, Vector3.up);
        Vector3 horizontalVector = (new Vector3(cam.transform.right.x, 0, cam.transform.right.z));

        movement += (verticalVector * vertical);
        movement += (horizontalVector * horizontal);
        movement = Vector3.ClampMagnitude(movement, 1);

        // turn in direction of movement input
		if (movement.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

		animator.SetFloat("Speed", movement.magnitude);
    }

    void HandleActionInputs()
    {
        
		// jump
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Jump");
        }

		// attack
		if (Input.GetButtonDown("Fire2"))
		{
			animator.SetTrigger("Attack");
		}
    }

	// TODO: use animation length in calculations incase we change animation speed or have two jump animations with different lengths
	IEnumerator JumpScaling() {
		if (jumpColliderScale.length > 0) {
			// find length from last key
			float curveTime = jumpColliderScale.keys [jumpColliderScale.length-1].time; 

			for (float t = 0f; t <= curveTime; t += Time.deltaTime) {
				controller.height = startHeight * Mathf.Clamp01(jumpColliderScale.Evaluate (t));
				yield return null;
			}

			controller.height = startHeight;
		}
	}

    void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            vSpeed = 0; // grounded character has vSpeed = 0...
        }
        vSpeed -= gravity * Time.deltaTime;

		Vector3 vMovement = Vector3.zero;
        vMovement.y = vSpeed;

        controller.Move(vMovement * Time.deltaTime);
    }

	void ApplyUpwardJump() {
		vSpeed = 2.5f;
		Vector3 vMovement = Vector3.zero;
		vMovement.y = vSpeed;

		controller.Move(vMovement * Time.deltaTime);

		StartCoroutine ("JumpScaling");
	}

}
