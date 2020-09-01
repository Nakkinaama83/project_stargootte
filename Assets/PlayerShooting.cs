using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public GameObject GravityGunAmmo;
    public GameObject ShotGunAmmo;
    public Transform GunBarrelEnd;
    public AudioClip AssaultRifleAudio;
    public AudioClip GravityGunAudio;
    public AudioClip ShotgunAudio;
    AudioSource gunAudio;
    private Text WeaponName;


    float timer;
    Ray shootRay;
    Ray friendlyRay;
    RaycastHit shootHit;
    RaycastHit friendlyHit;
    int shootableMask;
    //ParticleSystem gunParticles;
    LineRenderer gunLine;
    //AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    public List<string> weapons = new List<string> { };
    string currentWeapon = "";  //assault rifle
    public int assaultClip = 30;
    public int shotGunClip = 10;
    public bool pelaaja = false;
    public bool groupTargeting = false;
    public bool friendlyInSight = false;
    public Transform left;
    public Transform right;

    private bool firstShot;
    private float delay;



    void Awake ()
    {
        
        firstShot = true;
        shootableMask = LayerMask.GetMask ("Shootable");
        //gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();

        //weapons = new List<string> { "impulse cannon" };
        weapons.Add("Impulse Cannon");
    }


    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && pelaaja==true)   //.GetAxis("Mouse ScrollWheel") > 0f NextWeapon = weapons.SkipWhile(x => x != CurrentWeapon).Skip(1).DefaultIfEmpty(weapons[0]).FirstOrDefault();
        {
            string NextWeapon;
            NextWeapon = weapons.SkipWhile(x => x != currentWeapon).Skip(1).DefaultIfEmpty(weapons[0]).FirstOrDefault();
            currentWeapon = NextWeapon;
            Debug.Log("Current Weapon is " + currentWeapon);
            ChangeWeaponName();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && pelaaja==true)   //muuta ykkös ja kakkos numeroihin
        {
            string PrevWeapon;
            PrevWeapon = weapons.TakeWhile(x => x != currentWeapon).DefaultIfEmpty(weapons[weapons.Count - 1]).LastOrDefault();
            currentWeapon = PrevWeapon;
            Debug.Log("Current Weapon is " + currentWeapon);
            ChangeWeaponName();
        }

        if (groupTargeting == true)
        {
            if (Input.GetButton("Fire2"))
            {
                gameObject.transform.parent.GetComponent<AiController>().aiming = true;
                float step = 1 * Time.deltaTime;
                left = gameObject.transform.parent.Find("Left");
                right = gameObject.transform.parent.Find("Right");
                Transform target = left;    //tsekkaa vielä Rayllä onko seinä vieressä että kumpaan suuntaan lähetään
                for (int i = 0; i < 3; i++)
                {
                    Vector3 spread = transform.forward; //eka ammutaan suoraan ja seuraavat vasemmalle ja oikeelle
                    Vector3 origin = transform.position;
                    
                    if (i == 1)
                    {
                        Quaternion rotation = Quaternion.AngleAxis(-10, transform.up);  //vittu jee! Tääkin toimii.
                        spread = rotation * spread;
                        origin = origin + new Vector3(-1, 0, 0) / 3;
                        target = right;
                    }
                    if (i == 2)
                    {                        
                        Quaternion rotation = Quaternion.AngleAxis(10, transform.up);                        
                        spread = rotation * spread;
                        origin = origin + new Vector3(1, 0, 0) / 3;
                        target = left;
                    }

                    friendlyRay.origin = origin;
                    friendlyRay.direction = spread;
                    Debug.DrawRay(origin, spread * 10, Color.red, 10, true);    //vittu jee toimii
                    if (Physics.Raycast(friendlyRay, out friendlyHit, range, shootableMask))
                    {
                        string friendly = friendlyHit.collider.gameObject.tag;
                        if (friendly == "Player" || friendly == "Soldier" || friendly == "Scientist" || friendly == "Tech")
                        {
                            friendlyInSight = true;                           
                            transform.parent.position = Vector3.MoveTowards(transform.parent.position, target.position, step);
                            //animointi vielä tähän
                        }
                        else
                        {
                            friendlyInSight = false;
                        }
                        
                    }
                    
                }
                
                if (Input.GetButton("Fire1"))
                {
                    timer += Time.deltaTime;
                    delay = Random.Range(3, 5);
                    delay = delay / 10;
                    if (timer >= delay && assaultClip > 0 && firstShot == true && timer >= timeBetweenBullets && Time.timeScale != 0 && friendlyInSight == false)
                    {
                        timer += Time.deltaTime;
                        Debug.Log(assaultClip);
                        Shoot(currentWeapon);
                        firstShot = false;
                    }
                    if (assaultClip > 0 && firstShot == false && timer >= timeBetweenBullets && Time.timeScale != 0 && friendlyInSight == false)
                    {
                        timer += Time.deltaTime;
                        Debug.Log(assaultClip);
                        Shoot(currentWeapon);
                    }
                }
                
            }
            
            if (timer >= timeBetweenBullets * effectsDisplayTime)
            {             
                DisableEffects();
            }

        }
        if (groupTargeting == true && (Input.GetMouseButtonUp(0)))
        {            
            DisableEffects();   //välillä jää päälle ja varmistetaan ettei jää
            firstShot = true;
        }
        if (groupTargeting == true && (Input.GetMouseButtonUp(1)))
        {
            gameObject.transform.parent.GetComponent<AiController>().aiming = false;
        }

        if (pelaaja==true)    //CurrentWeapon == "Pulse Rifle" && 
        {
           
            timer += Time.deltaTime;
           
            //if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
            if (Input.GetButton("Fire1") && Input.GetButton("Fire2") && timer >= timeBetweenBullets && Time.timeScale != 0)
            {               
                //timer = Time.deltaTime;
                Debug.Log(assaultClip);
                Shoot(currentWeapon);               
            }
            if (timer >= timeBetweenBullets * effectsDisplayTime)
            {
                DisableEffects();
            }
        }
  

    }
    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }



    void Shoot (string weapon)
    {
        if (weapon == "Pulse Rifle" && assaultClip > 0)
        {
            timer = 0f;

            timeBetweenBullets = 0.15f;
            gunAudio.clip = AssaultRifleAudio;
            gunAudio.Play();
            assaultClip--;

            gunLight.enabled = true;

            //gunParticles.Stop ();
            //gunParticles.Play ();

            gunLine.enabled = true;
            gunLine.SetPosition(0, transform.position);

            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                PlayerHealth playerHealth = shootHit.collider.GetComponent<PlayerHealth>();
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                }
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damagePerShot);
                }
                gunLine.SetPosition(1, shootHit.point);

            }
            else
            {
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
        }

        if (weapon == "Pump Shotgun" && shotGunClip > 0)
        {
            shotGunClip--;
            timeBetweenBullets = 1f;
            timer = 0f;
            gunAudio.clip = ShotgunAudio;
            gunAudio.Play();

            gunLight.enabled = true;
            for (int i = 0; i < 7; i++)
            {
                Vector3 spread1 = new Vector3(Random.Range(-5, 5), Random.Range(-10, 10), 0);
                var bullet1 = Instantiate(ShotGunAmmo, transform.position, transform.rotation);
                bullet1.transform.Rotate(spread1);
            }
        }

        if (weapon == "Impulse Cannon")
        {
            //Vector3 gunBarrelEnd.transform.position = new Vector3(GunBarrelEnd);
            //Instantiate(GravityGunAmmo) as GameObject; // ei toimi??
            //Instantiate(GravityGunAmmo, GunBarrelEnd, Quaternion.identity);
            timeBetweenBullets = 1f;
            timer = 0f;
            Instantiate(GravityGunAmmo, transform.position, transform.rotation);
            gunAudio.clip = GravityGunAudio;
            gunAudio.Play();
        }
    }
    //void ShootImpulseCannon()
    //{
    //    //Vector3 gunBarrelEnd.transform.position = new Vector3(GunBarrelEnd);
    //    //Instantiate(GravityGunAmmo) as GameObject; // ei toimi??
    //    //Instantiate(GravityGunAmmo, GunBarrelEnd, Quaternion.identity);
    //    Instantiate(GravityGunAmmo, transform.position, transform.rotation);
    //    gunAudio.Play();
    //}

    //void ShootShotgun()
    //{
    //    timer = 0f;
    //    gunAudio.Play();

    //    gunLight.enabled = true;
    //    for (int i = 0; i < 7; i++)
    //    {
    //        Vector3 spread1 = new Vector3(Random.Range(-5, 5), Random.Range(-10, 10), 0);
    //        var bullet1 = Instantiate(ShotGunAmmo, transform.position, transform.rotation);
    //        bullet1.transform.Rotate(spread1);
    //    }
    //}
    
    public void ActivateWeaponName(GameObject canvasGameObject, Vector3 location)
    {
        GameObject gameObject = new GameObject("Child"); //creating player name
        //gameObject.transform.SetParent(this.transform);
        gameObject.transform.SetParent(canvasGameObject.transform);
        //gameObject.transform.position = location;


        gameObject.AddComponent<Text>().text = currentWeapon;
        gameObject.GetComponent<Text>().fontSize = 15;
        gameObject.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        gameObject.GetComponent<RectTransform>().anchoredPosition3D = location;
        WeaponName = gameObject.GetComponent<Text>();


    }

    void ChangeWeaponName()
    {
        WeaponName.text = currentWeapon;
    }   
}
