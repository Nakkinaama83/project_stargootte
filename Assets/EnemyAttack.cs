using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    //Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;
    private GameObject target;


    void Awake ()
    {
        //target = GetComponent<EnemyMovement>().currentTarget;   //ei voi olla tässä kun kohde vaihtuu
        //player = GameObject.FindGameObjectWithTag ("Player");
        //playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        //anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            target = GetComponent<EnemyMovement>().currentTarget;
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            playerHealth = target.GetComponent<PlayerHealth>();
            Attack ();
        }

        /*
        if(playerHealth.currentHealth <= 0)
        {
            //anim.SetTrigger ("PlayerDead");
        }*/
    }


    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
