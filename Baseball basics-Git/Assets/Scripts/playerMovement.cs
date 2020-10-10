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

    public GameObject youLosePanel, inGameCanvas;

    public float explosionDelay = 0.5f;
    public float countDown;
    bool hasBeenLaunched = false;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }

    // Update is called once per frame
    void Update()
    {
        cc = GetComponent<CharacterController>();
        touchingGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            countDown -= Time.deltaTime;
        }
        else
        {
            countDown = explosionDelay;
            hasBeenLaunched = false;
        }

        if (countDown <= 0 && touchingGround)
        {
            Destroy(rb);
            cc.enabled = true;
            this.enabled = true;
        }
        if (touchingGround)
        {
            Debug.Log("Hit the ground");  
        }
        if (cc.enabled == true) 
        {
           

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

            if (transform.position.y < -2)
            {
                inGameCanvas.SetActive(true);
                youLosePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
      
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            
        }
    }
}
