using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator anim;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSmoothTime;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityModifier;
    private Transform cam;
    private float animSpeed;
    private float animVel;

    private float groundedTimer;
    
    private Vector3 fallingVel;
    private float turnSmoothVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        fallingVel = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horiz,0,vert).normalized;

        float speed = 0;
        if (dir.magnitude > 0.1)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
                BlendAnimSpeed(1);
            }
            else
            {
                speed = walkSpeed;
                BlendAnimSpeed(0.5f);
            }
            float targetAngle = Vector3.SignedAngle(Vector3.forward, dir, Vector3.up) + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * (speed * Time.fixedDeltaTime));
        }
        else
        {
            BlendAnimSpeed(0);
        }

        
        fallingVel -= Physics.gravity * (gravityModifier * Time.fixedDeltaTime);
        if (controller.isGrounded)
        {
            groundedTimer = 0.2f;
        }
        if(groundedTimer > 0)
        {
            groundedTimer -= Time.fixedDeltaTime;
            if (Input.GetKey(KeyCode.Space))
                fallingVel = jumpForce * Vector3.up;
            else
                fallingVel = Vector3.zero;
        }

        controller.Move(fallingVel);
    }

    private void BlendAnimSpeed(float target)
    {
        animSpeed = Mathf.SmoothDamp(animSpeed, target, ref animVel, turnSmoothTime);
        anim.SetFloat("Speed", animSpeed);
    }
}
