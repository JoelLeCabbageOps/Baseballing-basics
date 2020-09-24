using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;

    NavMeshAgent agent;
    NavMeshAgent colAgent;
    BoxCollider boxCol;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        boxCol = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (agent != null)
        {
            Debug.Log("Trying to follow player");
            target = GameObject.FindGameObjectWithTag("Player").transform;
            agent.SetDestination(target.position);
        }
    }

    private void OnTriggerEnter(Collider collider) // This function is to enable a box collider that is not an "isTrigger" so that the box collider and be hit and launch the enemy
    {
        colAgent = collider.gameObject.GetComponent<NavMeshAgent>();

        if (colAgent != null && agent == null)
        {
            boxCol = collider.gameObject.GetComponent<BoxCollider>();
            boxCol.enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colAgent = collision.gameObject.GetComponent<NavMeshAgent>();
        Debug.Log("colAgent found");
        if (colAgent != null && agent == null)
        {
            colAgent.enabled = false;
        }
    }
}
