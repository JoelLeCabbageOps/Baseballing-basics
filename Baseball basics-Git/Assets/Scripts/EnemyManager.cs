using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float force = 20f; 
    public float countdown;

    private bool canExplode = false, hasExploded = false;

    public GameObject explosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb;

            if (nearbyObject.gameObject.tag == "Player")
            {
                rb = nearbyObject.gameObject.GetComponent<Rigidbody>();

                if (countdown >= 0)
                {
                    countdown -= Time.deltaTime;
                }
                else
                {
                    canExplode = true;
                }
                if (rb == null)
                {
                    if (canExplode)
                    {
                        rb = nearbyObject.gameObject.AddComponent<Rigidbody>(); //nearbyObject.GetComponent<Rigidbody>();
                        rb.constraints = RigidbodyConstraints.FreezeRotation;
                    } 
                }

                if (rb != null && canExplode && !hasExploded)
                {
                    Debug.Log(rb.gameObject.name);
                    CharacterController cc = rb.GetComponent<CharacterController>();
                    cc.enabled = false;
                   //playerMovement pMove = rb.GetComponent<playerMovement>();
                   //pMove.enabled = false;
                    Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
                    rb.AddExplosionForce(force, explosionPosition, radius);
                    hasExploded = true;
                    Destroy(gameObject);
                    GameObject explosionEffectObject = Instantiate(explosionEffect, transform.position, Quaternion.identity); //Spawn the explosion particle effect
                    Destroy(explosionEffectObject, 3f);
                    Debug.Log("KABOOM");
                }
            }
        }

        
    }
}
