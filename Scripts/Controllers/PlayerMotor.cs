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
    PlayerAnimator playerAnimator;
    
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
    float m_fMoveVertical, m_fMoveHorizontal, m_fJumpVertical, m_fJumpHorizontal, m_fVertical, m_fHorizontal;
    float angle;
    float followAngle;
    float tmpAngle;
    float lastAngle;
    public float height;

    int animAngle;
    float animAngleDead = 0.5f;
    bool isGround = true;
    bool isMove = true;
    bool followCam = false;

    Vector3 movePoint;
    public Vector3 targetPoint;

    float angleSmoothTime = 0.08f;
    float angleSmoothTime_Back = 0.4f;
    float currentAngleSmoothTime;
    float currentAngle;
    float angleSmoothVelocity;
   

    void Start () {
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }
	
	void Update ()
    {
        m_fVertical = Input.GetAxis("Vertical");
        m_fHorizontal = Input.GetAxis("Horizontal");
        if (!inputLock)
        {
            m_fMoveVertical = m_fVertical;
            m_fMoveHorizontal = m_fHorizontal;
            HandleAngle();
        }
        HandleMove();
        SetAnim();
    }

    void HandleAngle()
    {
        if (Input.GetMouseButton(0))
        {
            tmpAngle = lastAngle;
            followCam = false;
        }
        else if (Input.GetMouseButton(1) || movePoint.magnitude != 0 )
            followCam = true;

        
        followAngle = followCam? cam.eulerAngles.y : tmpAngle;

        currentAngleSmoothTime = (m_fMoveVertical != 0 || m_fMoveHorizontal != 0)? angleSmoothTime : angleSmoothTime_Back;
        currentAngle = Mathf.SmoothDampAngle(currentAngle, followAngle, ref angleSmoothVelocity, currentAngleSmoothTime) ;
        transform.eulerAngles = new Vector3(0, currentAngle, 0);

        if (currentAngle - lastAngle > animAngleDead)
            animAngle = 1;
        else if (currentAngle - lastAngle < -animAngleDead)
            animAngle = -1;
        else
            animAngle = 0;
        lastAngle = currentAngle;
    }

    void HandleMove()
    {
        HandleHeight();

        m_fNormalize = MoveNormalize(m_fMoveVertical, m_fMoveHorizontal);

        if (m_fNormalize > m_fVelocityDead)
        {
            if (isGround)
                movePoint = (m_fMoveVertical * transform.forward + m_fMoveHorizontal * transform.right) * (m_fMoveVertical < 0 ? backSpeed : speed) / m_fNormalize;
            isMove = true;
        }
        else
        {
            if(isGround)
                movePoint = Vector3.zero;
            isMove = false;
        }

        targetPoint = (movePoint + new Vector3(0, height, 0)) * Time.deltaTime;
        controller.Move(targetPoint);
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
            isGround = true;
        }
        else
        {
            height -= jumpGravity;
            isGround = false;
        }        

        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            height = jumpSpeed;
            isGround = false;

            playerAnimator.bJump = true;

            m_fJumpVertical = m_fMoveVertical;
            m_fJumpHorizontal = m_fMoveHorizontal;
        }
    }

    void SetAnim()
    {
        playerAnimator.bMove = isMove;
        playerAnimator.bGround = isGround;
        playerAnimator.fJumpVertical = m_fJumpVertical;
        playerAnimator.fJumpHorizontal = m_fJumpHorizontal;
        playerAnimator.iAnimAngle = animAngle;
        playerAnimator.fVertical = m_fVertical;
        playerAnimator.fHorizontal = m_fHorizontal;
        playerAnimator.fMoveVertical = m_fMoveVertical;
        playerAnimator.fMoveHorizontal = m_fMoveHorizontal;
    }

    public int GetAnimParameters()
    {
        int animInfo = 0;

        if (isMove)
            animInfo |= 0x0004;

        if (isGround)
            animInfo |= 0x0008;

        return animInfo;
    }
}
