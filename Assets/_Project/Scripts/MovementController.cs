using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private InputController Input;
    private Animator Anim;
    private CharacterController CharController;
    public Camera Cam;

    float AllowPlayerRotation = 0.1f;
    Vector3 DesiredMoveDirection;
    float VerticalVel;
    bool IsGrounded;
    float TimeToNextJump;

    public string BlendParam = "IdleWalkRunBlend";
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;
    [Range(0, 10f)]
    public float Speed = 0.3f;
    [Range(0, 10f)]
    public float FallSpeed = 0.15f;
    [Range(0, 3f)]
    public float DesiredRotationSpeed = 0.15f;

    void Start()
    {
        Input = GetComponent<InputController>();
        Anim = this.GetComponentInChildren<Animator>();
        CharController = this.GetComponent<CharacterController>();
        Cam = Camera.main;
    }

    void Update()
    {
        Fall();
        Move();
        Jump();

        CharController.Move(new Vector3(DesiredMoveDirection.x, VerticalVel, DesiredMoveDirection.z));
    }

    private void Jump()
    {
        if (Input.Jump && CanJump())
        {
            VerticalVel = 1.0f;
            TimeToNextJump = Time.time + 0.15f;
            Anim.SetBool("Jump", true);
        }
        else if (CanJump())
        {
            Anim.SetBool("Jump", false);
        }
    }

    private bool CanJump()
    {
        return IsGrounded && Time.time > TimeToNextJump;
    }

    private void Move()
    {
        float speed = new Vector2(Input.AxisHori, Input.AxisVert).sqrMagnitude;

        if (speed > AllowPlayerRotation)
        {
            Anim.SetFloat(BlendParam, speed, StartAnimTime, Time.deltaTime);

            var forward = Cam.transform.forward;
            var right = Cam.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            DesiredMoveDirection = (forward * Input.AxisVert + right * Input.AxisHori) * Time.deltaTime * Speed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(DesiredMoveDirection), DesiredRotationSpeed);
        }
        else if (speed < AllowPlayerRotation)
        {
            DesiredMoveDirection = Vector3.zero;
            Anim.SetFloat(BlendParam, speed, StopAnimTime, Time.deltaTime);
        }
    }

    private void Fall()
    {
        IsGrounded = CharController.isGrounded;
        VerticalVel -= 1 * Time.deltaTime;
        Anim.SetBool("Fall", !IsGrounded);
    }
}
