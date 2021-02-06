using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;

    public float gravity;
    public float speed;
    private float appliedSpeed;

    public float nextJumpDelay;
    private float currentNextJumpDelay;
    public float jumpHeight;
    private bool isGrounded;
    public Transform feetPosition;
    public float checkRadius;
    public LayerMask groundType;

    Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        appliedSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Glide();
    }

    private void Glide()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Time.time > currentNextJumpDelay)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                currentNextJumpDelay = Time.time + nextJumpDelay;
            }
        }
        
    }
    private void Movement()
    {
        isGrounded = Physics.CheckSphere(feetPosition.position, checkRadius, groundType);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1;
        }
        float x = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * x;

        controller.Move(move * appliedSpeed * Time.deltaTime);

        velocity.y += gravity + Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(feetPosition.transform.position, checkRadius);
    }
}
