using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{
    public Rigidbody theRigid;
    [SerializeField]
    private Gun theGun;

    private float playerSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float jumpForce;

    private CrossHair theCH;
    private GunController theGunController;

    private bool reloading = false;
    public bool aiming = false;
    private bool firing = false;
    public bool running = false;
    private bool jumping = false;
    private bool walking = false;
    private Vector3 moving;
    private CapsuleCollider playerBody;
    [SerializeField]
    private GameObject hit_effect;

    private void Start()
    {
        theGunController = FindObjectOfType<GunController>();
        theCH = FindObjectOfType<CrossHair>();
        theRigid = GetComponent<Rigidbody>();
        playerBody = GetComponent<CapsuleCollider>();
        playerSpeed = walkSpeed;
    }
    private void FixedUpdate()
    {

        SendInputToServer();
    }

    private void Update()
    {
        GroundCheck();
        Jump();
        Run();
        Move();
        MoveCheck();
    }
    private void SendInputToServer()
    {
        ClientSend.PlayerPosition();
        ClientSend.PlayerRotation();
    }
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * playerSpeed;

        theRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }
    private void MoveCheck()
    {
        if (!running && !jumping)
        {
            if (Vector3.Distance(moving, transform.position) >= 0.01f)
            {
                walking = true;
            }
            else
            {
                walking = false;
            }
            theCH.WalkingAnim(walking);
            theGun.anim.SetBool("Walk", walking);
            moving = transform.position;
        }
    }
    private void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Walking();
        }
    }
    private void Running()
    {
        theGunController.CancelAim();
        running = true;
        theCH.RunningAnim(running);
        theGun.anim.SetBool("Run", running);
        playerSpeed = runSpeed;
    }
    private void Walking()
    {
        running = false;
        theCH.RunningAnim(running);
        theGun.anim.SetBool("Run", running);
        playerSpeed = walkSpeed;
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            Jumping();
        }
    }
    private void Jumping()
    {
        theCH.JumpingAnim(jumping);
        theRigid.velocity = transform.up * jumpForce;
    }
    private void GroundCheck()
    {
        jumping = !(Physics.Raycast(transform.position, Vector3.down, playerBody.bounds.extents.y + 0.1f));
        theCH.JumpingAnim(jumping);
    }
}
