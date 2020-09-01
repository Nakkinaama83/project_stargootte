using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private Transform ActivePlayerInCanvas;
    public bool player;
    public GameObject interactText;
    private bool interacting = false;
    private bool waitingInput = false;
    public string playerClass;
    

    public void Interacting()
    {
        //FindObjectOfType<PlayerMovement>().enabled = false;
        //FindObjectOfType<PlayerShooting>().enabled = false;
        
        //gameObject.GetComponent<PlayerMovement>().enabled = false;
        //gameObject.GetComponentInChildren<PlayerShooting>().enabled = false;
    }
    
    public void DoneInteracting()
    {
        //FindObjectOfType<PlayerMovement>().enabled = true;
        //FindObjectOfType<PlayerShooting>().enabled = true;

        //gameObject.GetComponent<PlayerMovement>().enabled = true;
        //gameObject.GetComponentInChildren<PlayerShooting>().enabled = true;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        player = false;
        //GetComponentInChildren<Canvas>().enabled = false;  //poista tää kun inventaarion saa toimimaan
    }

    public void PlayerActiveInCanvas(GameObject charParent) //tässä yritetään siirtää paneelia aktiiviseen pelaajaan UI:ssa
    {
        ActivePlayerInCanvas = charParent.transform;
        //FindObjectOfType<PlayerChange>().ActivePlayerInCanvas = charParent.transform;
        //FindObjectOfType<ActivePlayerInCanvas>().activePlayerInCanvas = charParent.transform;   //kele, täähän toimii. Jatka vielä vähän :)
    }
    public void MovePanelInCanvas()
    {
        FindObjectOfType<ActivePlayerInCanvas>().activePlayerInCanvas = ActivePlayerInCanvas;
    }
    public void PlayerActive()
    {
        player = true;
        MovePanelInCanvas();
        GetComponent<PlayerInventory>().ActiveInventory();
        //GetComponentInChildren<Canvas>().enabled = true;  //poista tää kun inventaarion saa toimimaan
    }
   
    public void PlayerNonActive()
    {
        player = false;
        GetComponent<PlayerInventory>().InActiveInventory();
        //GetComponentInChildren<Canvas>().enabled = false;  //poista tää kun inventaarion saa toimimaan
    }

    void OnTriggerStay(Collider other)
    {
        playerClass = gameObject.name.ToString();
        if (player == true && other.gameObject.tag == "Door" ) //(other.gameObject.tag == "Shootable")
        {
            interactText.SetActive(true);
            waitingInput = true;
            //other.gameObject.GetComponent<DoorTrigger>().Enter();
            if (interacting == true)
            {
                other.gameObject.GetComponent<DoorTrigger>().Interact(playerclass: playerClass);
                interacting = false;
            }
        }
        if (player == false && other.gameObject.tag == "Door")
        {
            interactText.SetActive(false);
            waitingInput = false;
            //other.gameObject.GetComponent<DoorTrigger>().Exit();
        }
            //hit = true;
        //collision = true;
    }
    void OnTriggerExit(Collider other)

    {
        if (other.gameObject.tag == "Door" && player == true )
        {
            other.gameObject.GetComponent<DoorTrigger>().Exit();
            
        }
    }

    public void WaitingInput()
    {
        if (Input.GetKeyDown("e"))
        {
            interactText.SetActive(false);
            //FindObjectOfType<Interact>().Interacting();
            interacting = true;
            //openDoor.Open();
            //FindObjectOfType<OpenDoor>().Open(); -muuten hyvä mutta avaa kaikki ovet
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (waitingInput == true)
            WaitingInput();
    }
    
}
