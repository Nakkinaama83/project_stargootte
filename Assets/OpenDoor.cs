using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public float lerpSpeed = 10f;
    
    public bool exploded = false;
    public Transform closePosition;
    public Transform openPosition;
    private bool isDoorOpen = false;
    public float thrust = 20;
    public Rigidbody door;
    public GameObject playerPos;
    public Vector3 dir;
    public GameObject explosion;

    public void Open()
    {
        if (isDoorOpen == false)
        {

            //transform.position = Vector3.Lerp(openPosition.position, closePosition.position, lerpSpeed * Time.deltaTime);
            
            isDoorOpen = true;
            //transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * lerpSpeed);
            //transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
            //MoveDoor(openPosition: openPosition.position, closePosition: closePosition.position);
        }
        else if (isDoorOpen == true)
        {
            Debug.Log("Close");
            //transform.position = Vector3.Lerp(closePosition.position, openPosition.position, lerpSpeed * Time.deltaTime);
            //MoveDoor(openPosition: closePosition.position, closePosition: openPosition.position);
            
            isDoorOpen = false;
        }
    }
    
    
    
    public void Explode()
    {
        playerPos = FindObjectOfType<PlayerChange>().CurrentPlayer;
        explosion.SetActive(true);
        //Instantiate(explosion, transform.position, Quaternion.identity);
        door.isKinematic = false;
        //dir = playerPos.transform.position - transform.position; - ovi tulee pelaajaa kohti
        dir = transform.position - playerPos.transform.position;    //ovi menee poispäin pelaajasta
        dir = dir.normalized;
        door.AddForce(dir * thrust, ForceMode.Impulse);
        //door.AddForce(0, 0, thrust, ForceMode.Impulse);
        exploded = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        door = GetComponent<Rigidbody>();
        //playerPos = GameObject.FindWithTag("Player");
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        if (isDoorOpen == true && exploded == false)
        {
           
            float step = lerpSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, openPosition.position, step);
        
            
        }
        


        if (isDoorOpen == false && exploded == false)
        {
            
            float step = lerpSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, closePosition.position, step);
            
            
            
        }
        
        

        //float step = lerpSpeed * Time.deltaTime; // calculate distance to move
        //transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
