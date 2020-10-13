using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour // also controls health
{
    public float maxHealth;
    public float currentHealth;
    public HealthBar healthBar;

    public float speed;
    public float jumpHeight; // How many units high the player will jump

    CharacterController cc;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float gravity = -9.81f;
    Vector3 velocity;
    public bool touchingGround;

    public GameObject youLosePanel, inGameCanvas;

    public float explosionDelay = 0.5f;
    public float countDown;
    bool hasBeenLaunched = false;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            youLosePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }


        cc = GetComponent<CharacterController>();
        touchingGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            countDown -= Time.deltaTime;
        }
        else if (rb == null)
        {
            countDown = explosionDelay;
            hasBeenLaunched = false;
            rb = gameObject.GetComponent<Rigidbody>();
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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
