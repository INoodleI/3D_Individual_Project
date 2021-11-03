using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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


    [SerializeField] private GameObject animatedModel;
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private Rigidbody hips;
    
    private enum PlayerState
    {
        Walking,
        Ragdoll,
        Riding
    }

    private PlayerState state = PlayerState.Walking;
    
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
        switch (state)
        {
            case PlayerState.Walking:
                Movement();
                Gravity();
                Jump();
                CheckForRagdoll();
                break;
            case PlayerState.Ragdoll:
                break;
            case PlayerState.Riding:
                break;
            default:
                Debug.LogError("WAAAAAAAHHHHHHHHHHHHH - Player Controller State");
                break;
        }
    }

    private void CheckForRagdoll()
    {
        if(Input.GetKeyDown(KeyCode.R))
            Ragdoll();
    }

    public void Ragdoll()
    {
        state = PlayerState.Ragdoll;
        animatedModel.SetActive(false);
        ragdoll.SetActive(true);
    }

    private void Movement()
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
    }

    private void Jump()
    {
        
        if(controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
                fallingVel = jumpForce * Vector3.up;
            else
                fallingVel = Vector3.zero;
        }
        controller.Move(fallingVel);
    }
    private void Gravity()
    {
        fallingVel -= Physics.gravity * (gravityModifier * Time.fixedDeltaTime);
        controller.Move(fallingVel);
    }

    private void BlendAnimSpeed(float target)
    {
        animSpeed = Mathf.SmoothDamp(animSpeed, target, ref animVel, turnSmoothTime);
        anim.SetFloat("Speed", animSpeed);
    }
}
