using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunAmmo : MonoBehaviour
{
    public float movementSpeed = 5f;
    private bool hit = false;
    private float lifetime = 3f;


    private void Awake()
    {
        Destroy(gameObject, lifetime);
    }

    // Start is called before the first frame update
    void Start()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(20);
            hit = true;
        }
        else if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(20, transform.position);
        }
        else if (other.gameObject) //(other.gameObject.tag == "Shootable")
            hit = true;
        //collision = true;
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject)
            hit = true;
    }
    */

    // Update is called once per frame
    void Update()
    {
        if (hit == false)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        }
        //transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        else if (hit == true)
        {
            Impact();
        }

    }

    void Impact()
    {
        
        
        Destroy(gameObject);
    }
}
