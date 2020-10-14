using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float explosionRadius;
    public float force = 20f;
    public float explosionDamage;
    public float countdown;

    private bool reachedPlayer = false;
    public bool canExplode = false, hasExploded = false;

    public float sinceHitDelay;
    public float sinceHitCountdown;
    public bool hasBeenHit = false;

    public GameObject explosionEffect;

    Collider[] colliders;
    Rigidbody rb;
    GameObject player;


    public Animator countdownAnimator;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        sinceHitCountdown = sinceHitDelay;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (hasBeenHit)
        {
            canExplode = true;
        }

        if (rb == null && countdown <= 0)
        {
            rb = player.gameObject.AddComponent<Rigidbody>();
            rb = player.gameObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        colliders = Physics.OverlapSphere(transform.position, radius);

        if (reachedPlayer)
        {
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
            }
            else
            {
                canExplode = true;
            }
        }
        

        foreach (Collider nearbyObject in colliders)
        {

            if (nearbyObject.gameObject.tag == "Player")
            {
                reachedPlayer = true;
                if (countdownAnimator == null)
                {
                   // Animator[] children = GetComponentsInChildren<Animator>();
                   // foreach (Animator child in children)
                   // {
                   //     if (child.gameObject.tag == "CountdownCanvas")
                   //     {
                   //         countdownAnimator = child;
                   //     }
                   // }
                } else
                {
                    countdownAnimator.speed = 1;
                    countdownAnimator.SetBool("countingDown", true);
                    Debug.Log("Set countingDown to true");
                }
                rb = nearbyObject.gameObject.GetComponent<Rigidbody>();

                
                if (rb == null)
                {
                    if (canExplode && countdown <= 0)
                    {
                        rb = nearbyObject.gameObject.AddComponent<Rigidbody>(); //nearbyObject.GetComponent<Rigidbody>();
                        rb.constraints = RigidbodyConstraints.FreezeRotation;
                    } 
                }

                if (rb != null && canExplode && !hasExploded && countdown <= 0)
                {
                    Explode();
                }
            } else
            {
                //countdownAnimator.speed = 0f;
                Debug.Log("stopping the countdown");
                //countdownAnimator.SetBool("countingDown", false);
            }
        }

        if (rb != null && canExplode && !hasExploded && countdown <= 0 && hasBeenHit)
        {
            Explode();
        } else
        {
            Debug.Log("rb is null");
            
            rb = player.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                Debug.Log("the rb is now on " + rb.gameObject.name);
            }


        }
    }

    public void Explode()
    {
        if (rb != null && canExplode && !hasExploded)
        {
                playerMovement pMove = rb.GetComponent<playerMovement>();

            if (Vector3.Distance(rb.position, transform.position) < explosionRadius)
            {
                Debug.Log(rb.gameObject.name);
                CharacterController cc = rb.GetComponent<CharacterController>();
                cc.enabled = false;
            }
                if (pMove != null)
            {
                if (Vector3.Distance(rb.position, transform.position) < explosionRadius)
                {
                    pMove.TakeDamage(explosionDamage);
                }
                
                if (pMove.touchingGround)
                {
                    pMove.rb = rb;
                }
            }

            Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);

            if (Vector3.Distance(rb.position, transform.position) < explosionRadius)
            {
                rb.AddExplosionForce(force, explosionPosition, explosionRadius);
            }

            hasExploded = true;
            Destroy(gameObject);
            GameObject explosionEffectObject = Instantiate(explosionEffect, transform.position, Quaternion.identity); //Spawn the explosion particle effect
            Destroy(explosionEffectObject, 3f);
            Debug.Log("KABOOM");
        }
    }

}
