using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerMotor : MonoBehaviour {

    //CharacterController controller;
    Animator animator;

    Vector3 currentVelocity;
    Vector3 targetPosition;
    Vector3 smoothVelocity;
    float smoothTime = 0.1f;

    float targetAngle;
    float currentAngle;
    float angleSmoothVelocity;
    public float angleSmoothTime = 0.1f;

    void Start () {
        //controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
	}

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime);

        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleSmoothVelocity, angleSmoothTime);
        transform.eulerAngles = new Vector3(0, currentAngle, 0);
    }

    public void HandleMovementMessage(Vector3 velocity ,Vector3 position, float eularAngle, int animInt)
    {
        targetPosition = position;
        targetAngle = eularAngle;
        SetAnimator(animInt);
    }

    void SetAnimator(int animInt)
    {
        animator.SetBool("Forward", (animInt & 0x0001) != 0 ? true : false);

        animator.SetBool("Back", (animInt & 0x0002) != 0 ? true : false);

        animator.SetBool("Ground", (animInt & 0x0004) != 0 ? true : false);

        animator.SetBool("Jump", (animInt & 0x0008) != 0 ? true : false);
    }
}
