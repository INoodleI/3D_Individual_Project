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

    [SerializeField] private Ragdoll ragdoll;
    [SerializeField] private Animator cineCam;
    [SerializeField] private GameObject animatedModel;
    [SerializeField] private Transform ragdollCamLookAt;
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
        ragdoll.Initialize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == PlayerState.Walking)
        {
            Movement();
            Gravity();
            Jump();
            CheckForRagdoll();
        }
        else if (state == PlayerState.Ragdoll)
        {
            CameraFollowRagdoll();
            CheckForUnRagdoll();
        }
        else if(state == PlayerState.Riding)
        {
            
        }
    }

    private void CameraFollowRagdoll()
    {
        ragdollCamLookAt.position = hips.transform.position ;
    }


    private void CheckForUnRagdoll()
    {
        if(Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))
            UnRagdoll();
    }
    
    public void UnRagdoll()
    {
        Debug.Log("-- UnRagdolling");
        state = PlayerState.Walking;
        transform.position = ragdollCamLookAt.position;
        
        ragdoll.Disable();
        
        animatedModel.SetActive(true);
        cineCam.Play("PlayerCam");
    }
    
    private void CheckForRagdoll()
    {
        if(Input.GetKeyDown(KeyCode.R)|| Input.GetMouseButtonDown(0))
            Ragdoll();
    }

    public void Ragdoll()
    {
        Debug.Log("Ragdolling");
        state = PlayerState.Ragdoll;
        animatedModel.SetActive(false);
        ragdoll.Enable();
        cineCam.Play("RagdollCam");
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
