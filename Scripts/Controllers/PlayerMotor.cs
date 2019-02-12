using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    public static PlayerMotor instance;

    void Awake()
    {
        instance = this;
    }

    public Transform cam;
    public Animator animator;
    CharacterController controller;

    public bool inputLock = false;
    public float speed = 6;
    public float backSpeed = 3;
    public float gravity = 5;
    public float jumpSpeed = 4;
    public float jumpGravity = 0.15f;

    [SerializeField]
    float m_fVelocityDead = 0.2f;
    float m_fNormalize;
    float vertical, horizontal, lastVertical, lastHorizontal;
    float angle;
    float followAngle;
    float lastAngle;
    public float height;

    bool isJump = false;
    bool isMove = true;
    bool followCam = false;

    Vector3 movePoint;
    public Vector3 currentVelocity;

    float angleSmoothTime = 0.08f;
    float angleSmoothTime_Back = 0.4f;
    float currentAngleSmoothTime;
    float currentAngle;
    float angleSmoothVelocity;
   

    void Start () {
        controller = GetComponent<CharacterController>();
	}
	
	void Update ()
    {
        if (!inputLock)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            HandleAngle();
        }
        HandleMove();

        SetAnim();
    }

    void HandleAngle()
    {
        if (isJump) return;

        if (Input.GetMouseButtonDown(0))
            lastAngle = followAngle;
 
        if (Input.GetMouseButton(0))
            followCam = false;
        else if (Input.GetMouseButton(1) || movePoint.magnitude != 0 )
            followCam = true;

        
        followAngle = followCam? (cam.eulerAngles.y + angle) : (lastAngle + angle);
        
        currentAngleSmoothTime = (vertical != 0 || horizontal != 0)? angleSmoothTime : angleSmoothTime_Back;
        currentAngle = Mathf.SmoothDampAngle(currentAngle, followAngle, ref angleSmoothVelocity, currentAngleSmoothTime) ;
        transform.eulerAngles = new Vector3(0, currentAngle, 0);
    }

    void HandleMove()
    {
        HandleHeight();

        m_fNormalize = MoveNormalize(vertical, horizontal);

        if (m_fNormalize > m_fVelocityDead)
        {
            if (!isJump)
                movePoint = (vertical * transform.forward + horizontal * transform.right) * (vertical < 0 ? backSpeed : speed) / m_fNormalize;
            isMove = true;
        }
        else
        {
            if(!isJump)
                movePoint = Vector3.zero;
            isMove = false;
        }

        currentVelocity = (movePoint + new Vector3(0, height, 0)) * Time.deltaTime;
        controller.Move(currentVelocity);
    }

    float MoveNormalize(float vertical, float horizontal)
    {
        return Mathf.Sqrt(vertical * vertical + horizontal * horizontal); 
    }

    void HandleHeight()
    {
        if (controller.isGrounded)
        {
            height = -gravity;
            //isGround = true;
            isJump = false;
        }
        else
        {
            height -= jumpGravity;
            //isGround = false;
        }        

        if (!isJump && Input.GetKeyDown(KeyCode.Space))
        {
            height = jumpSpeed;
            isJump = true;
            lastVertical = vertical;
            lastHorizontal = horizontal;
        }
    }

    void SetAnim()
    {
        animator.SetBool("Move", isMove);
        animator.SetBool("Jump", isJump);
        
        animator.SetFloat("LastVertical", lastVertical);
        animator.SetFloat("LastHorizontal", lastHorizontal);

        if (!isJump)
        {
            animator.SetFloat("Vertical", vertical);
            animator.SetFloat("Horizontal", horizontal);
        }
    }

    public int GetAnimParameters()
    {
        int animInfo = 0;

        if (isMove)
            animInfo |= 0x0004;

        if (isJump)
            animInfo |= 0x0008;

        return animInfo;
    }
}
