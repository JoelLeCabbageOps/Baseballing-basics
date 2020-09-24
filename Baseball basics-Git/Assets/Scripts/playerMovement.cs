using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float jumpHeight; // How many units high the player will jump

    CharacterController cc;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float gravity = -9.81f;
    Vector3 velocity;
    bool touchingGround;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        touchingGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (touchingGround && velocity.y < 0)
        {
            velocity.y = -9.81f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        cc.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && touchingGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -9.81f * gravity);
        }
    }
}
