using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;
    private List<GameObject> targets;
    public GameObject currentTarget;

    void Awake ()
    {
       
        //targets = new List<Transform>(GameObject.FindGameObjectsWithTag("Player").transform); //hmm, tääkään ei saa transformia ulos
        //targets = GameObject.FindObjectOfType<PlayerChange>().players; //toimii muuten mutta ei saa transformia ulos, luultavasti koska sitä ei ole listassa
        //player = GameObject.FindGameObjectWithTag ("Player").transform;
        //playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
        //currentTarget = targets[0];
        
        /*
        Transform GetClosestEnemy(Transform[] enemies)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (GameObject i in targets)
            {
                Vector3 directionToTarget = i.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = i.transform;
                }
            }
            
            return bestTarget;
        }*/
    }


    void Update ()
    {
        playerHealth = currentTarget.GetComponent<PlayerHealth>();
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            nav.enabled = true;
            //nav.SetDestination (player.position);
            nav.SetDestination(currentTarget.transform.position);
        }
        else
        {
            nav.enabled = false;
        }
    }
    private void FixedUpdate()
    {
        targets = GameObject.FindObjectOfType<PlayerChange>().players;
        if(currentTarget == null)
        {
            currentTarget = targets[0];
        }
        float distanceToLastTarget = Vector3.Distance(this.gameObject.transform.position, currentTarget.transform.position);

       
        foreach (GameObject target in targets)
        {
            if (playerHealth != null)
            {
                if ((Vector3.Distance(gameObject.transform.position, target.transform.position)) < distanceToLastTarget && target.GetComponent<PlayerHealth>().currentHealth > 0 || playerHealth.currentHealth <= 0)
                {
                    currentTarget = target;
                }
            }
                        
        }
    }
}

/*
  Transform GetClosestEnemy (Transform[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }
*/