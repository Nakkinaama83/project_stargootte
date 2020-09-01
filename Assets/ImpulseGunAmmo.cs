using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseGunAmmo : MonoBehaviour
{
    public float movementSpeed = 20f;
    private bool hit = false;
    public GameObject Explosion;

    //public LayerMask m_TankMask;
    public LayerMask ShootableMask;  // tämä
    //public ParticleSystem m_ExplosionParticles;
    public AudioSource m_ExplosionAudio;
    public float m_MaxDamage = 1000f;    //tämä              
    public float m_ExplosionForce = 1000f;
    public float m_MaxLifeTime = 2f;
    public float m_ExplosionRadius = 4f;      //tämä

 

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

    private void OnTriggerEnter(Collider other)
    {

        hit = true;
        // Find all the tanks in an area around the shell and damage them.

        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, ShootableMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
                continue;

            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            if (other.gameObject.tag == "Player")
            {
                PlayerHealth targetHealth = targetRigidbody.GetComponent<PlayerHealth>();

                if (!targetHealth)
                    continue;

                float damage = CalculateDamage(targetRigidbody.position);

                targetHealth.TakeDamage(damage);
            }
            if (other.gameObject.tag == "Enemy")
            {
                EnemyHealth targetHealth = targetRigidbody.GetComponent<EnemyHealth>();

                if (!targetHealth)
                    continue;

                float damage = CalculateDamage(targetRigidbody.position);

                targetHealth.TakeDamage(damage, transform.position);
            }
            /*
            PlayerHealth targetHealth = targetRigidbody.GetComponent<PlayerHealth>();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidbody.position);

            targetHealth.TakeDamage(damage);
            */
        }

        //m_ExplosionParticles.transform.parent = null;

        //m_ExplosionParticles.Play();

        //m_ExplosionAudio.Play();

        //Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
        //Destroy(gameObject);
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

    void Impact()
    {
       
        //Detonator - Tiny
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
