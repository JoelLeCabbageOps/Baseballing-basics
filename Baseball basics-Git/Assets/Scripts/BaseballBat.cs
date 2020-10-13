using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseballBat : MonoBehaviour
{
    public float rotateSpeed;
    public float batImpactForce;

    public Transform player;
    public Transform lookTarget;
    public Transform lookGoal;
    public float lookTargetSpeed;
    Transform lookGoalParent;

    Rigidbody lookTargetRb;

    private BoxCollider enemyBoxCol;


    private void Start()
    {
        lookTargetRb = lookTarget.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        FindLookTargetsTarget();
        BatRotate(); 
    }

    void FindLookTargetsTarget()
    {
        Vector3 towardsTarget = lookGoal.position - lookTarget.position;
            //Vector3.MoveTowards(lookTarget.position, lookGoal.position, 5f * Time.deltaTime);

            if (lookTargetRb.velocity.magnitude > lookTargetSpeed && Vector3.Distance(lookTarget.position, lookGoal.position) > 3)
            {
                Mathf.Clamp(lookTargetRb.velocity.magnitude, 0, lookTargetSpeed - 5);
                lookTargetRb.velocity = towardsTarget;
            }
            else if (lookTargetRb.velocity.magnitude < lookTargetSpeed)
            {
                lookTargetRb.AddForce(towardsTarget * lookTargetSpeed * Time.deltaTime, ForceMode.VelocityChange);
            }
        
    }
    void TargetRotate()
    {
        lookGoalParent = lookGoal.parent.transform;

        Vector3 targetDirection = lookTarget.position - lookGoalParent.position;

        float singleStep = lookTargetSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(lookGoalParent.forward, targetDirection, singleStep, 0.0f);

        lookGoalParent.rotation = Quaternion.LookRotation(newDirection);
    }

    void BatRotate()
    {
        Vector3 targetDirection = lookTarget.position - transform.position;

        float singleStep = rotateSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
      
        Debug.DrawRay(transform.position, newDirection, Color.red);
      
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void OnTriggerEnter(Collider collider) // This function is to enable a box collider that is not an "isTrigger" so that the box collider and be hit and launch the enemy
    {
        enemyBoxCol = collider.gameObject.GetComponent<BoxCollider>();

        if (enemyBoxCol != null)
        {
            enemyBoxCol.enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool hitPlayer = false;
        if (collision.gameObject.tag != "Player" && !hitPlayer)
        {
            hitPlayer = false;

            ObjectInformation obInfo = collision.gameObject.GetComponent<ObjectInformation>();
            EnemyManager enemyManager = collision.gameObject.GetComponent<EnemyManager>();
            
            if (enemyManager != null)
            {
                enemyManager.hasBeenHit = true;
            }

            if (obInfo != null)
            {
                obInfo.currentHealth -= 1f;
            }

            NavMeshAgent agent = collision.transform.GetComponent<NavMeshAgent>();

            if (agent != null)
            {
                // If the agent is found, store it's rigidbody, disable the agent, and add force to the gameObject.

                Rigidbody rb = agent.GetComponent<Rigidbody>();

                agent.enabled = false;

                rb.AddForce(-collision.contacts[0].normal * batImpactForce, ForceMode.Impulse);
                Debug.Log("The bat hit " + collision.gameObject.name);
            }

        } else
        {
            hitPlayer = true;
        }

    }

}
