using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGunAmmo : MonoBehaviour
{
    public float movementSpeed = 5f;
    private bool hit = false;
    public GameObject Explosion;
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
        if (other.gameObject) //(other.gameObject.tag == "Shootable")   other.gameObject
            hit = true;
        //collision = true;
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject)
    //        hit = true;
    //}

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
        //Detonator - Tiny
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    /*
    public LayerMask m_TankMask;
    public LayerMask Shootable;  // tämä
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;    //tämä              
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;      //tämä
     
    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.

        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
                continue;

            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidbody.position);

            targetHealth.TakeDamage(damage);
        }

        m_ExplosionParticles.transform.parent = null;

        m_ExplosionParticles.Play();

        m_ExplosionAudio.Play();

        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;

        float releativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        float damage = releativeDistance * m_MaxDamage;

        damage = Mathf.Max(0f, damage);

        return damage;
    }
    */
}
