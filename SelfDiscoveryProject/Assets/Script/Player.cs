using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;

    public float gravity;
    public float speed;
    private float appliedSpeed;
    
    private bool isGrounded;
    private bool roofReached;

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
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * x;

        controller.Move(move * appliedSpeed * Time.deltaTime);

        velocity.y += gravity + Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
