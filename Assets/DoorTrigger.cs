using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorTrigger : MonoBehaviour
{
    public OpenDoor openDoor;
    public GameObject interactText;
    public GameObject explodeButtonObj;
    public Button explodeButton;
    public GameObject hardwireButtonObj;
    public Button hardwireButton;
    public GameObject hackButtonObj;
    public Button hackButton;
    public GameObject cancelButtonObj;
    public Button cancelButton;
    //private bool interacting = false;
    //private bool trigger = false;

 /*
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactText.SetActive(true);
            trigger = true;
            
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactText.SetActive(false);
            trigger = false;

            explodeButton.onClick.RemoveListener(ExplodeButton);
            hardwireButton.onClick.RemoveListener(HardwireButton);
            hackButton.onClick.RemoveListener(HackButton);
            cancelButton.onClick.RemoveListener(CancelButton);
            explodeButtonObj.SetActive(false);
            hardwireButtonObj.SetActive(false);
            hackButtonObj.SetActive(false);
            cancelButtonObj.SetActive(false);
        }
    }
    */

    public void Enter()
    {
        interactText.SetActive(true);
        //trigger = true;
    }
    public void Exit()
    {
        interactText.SetActive(false);
        //trigger = false;

        explodeButton.onClick.RemoveListener(ExplodeButton);
        hardwireButton.onClick.RemoveListener(HardwireButton);
        hackButton.onClick.RemoveListener(HackButton);
        cancelButton.onClick.RemoveListener(CancelButton);
        explodeButtonObj.SetActive(false);
        hardwireButtonObj.SetActive(false);
        hackButtonObj.SetActive(false);
        cancelButtonObj.SetActive(false);
    }
    /*
    public void Trigger()
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
    */

    public void Interact(string playerclass)
    {
        Debug.Log(playerclass);
        if (playerclass == "Tech")
        {
            cancelButtonObj.SetActive(true);
            explodeButtonObj.SetActive(true);
            hardwireButtonObj.SetActive(true);

            cancelButton.onClick.AddListener(CancelButton);
            explodeButton.onClick.AddListener(ExplodeButton);
            hardwireButton.onClick.AddListener(HardwireButton);
        }
        if (playerclass == "Soldier")
        {
            cancelButtonObj.SetActive(true);
            explodeButtonObj.SetActive(true);
            
            cancelButton.onClick.AddListener(CancelButton);
            explodeButton.onClick.AddListener(ExplodeButton);
        }
        if (playerclass == "Scientist")
        {
            cancelButtonObj.SetActive(true);
            hackButtonObj.SetActive(true);

            cancelButton.onClick.AddListener(CancelButton);
            hackButton.onClick.AddListener(HackButton);
        }
        else
        {
            Debug.Log("Could not find player class");
        }
        //trigger = false;  //turha poista
        //interacting = false;  //turha poista
        //explodeButtonObj.SetActive(true);
        //hardwireButtonObj.SetActive(true);
        //hackButtonObj.SetActive(true);
        //cancelButtonObj.SetActive(true);
        //explodeButton.onClick.AddListener(ExplodeButton);
        //hardwireButton.onClick.AddListener(HardwireButton);
        //hackButton.onClick.AddListener(HackButton);
        //cancelButton.onClick.AddListener(CancelButton);
    }

    void ExplodeButton()
    {
 
        explodeButton.onClick.RemoveListener(ExplodeButton);
        hardwireButton.onClick.RemoveListener(HardwireButton);
        hackButton.onClick.RemoveListener(HackButton);
        cancelButton.onClick.RemoveListener(CancelButton);
        explodeButtonObj.SetActive(false);
        hardwireButtonObj.SetActive(false);
        hackButtonObj.SetActive(false);
        cancelButtonObj.SetActive(false);
        //FindObjectOfType<Interact>().DoneInteracting();
        openDoor.Explode();
        interactText.SetActive(false);
        Destroy(gameObject);
        
    }

    void HardwireButton()
    {

        explodeButton.onClick.RemoveListener(ExplodeButton);
        hardwireButton.onClick.RemoveListener(HardwireButton);
        hackButton.onClick.RemoveListener(HackButton);
        cancelButton.onClick.RemoveListener(CancelButton);
        explodeButtonObj.SetActive(false);
        hardwireButtonObj.SetActive(false);
        hackButtonObj.SetActive(false);
        cancelButtonObj.SetActive(false);
        //FindObjectOfType<Interact>().DoneInteracting();
        openDoor.Open();
    }

    void HackButton()
    {
        explodeButton.onClick.RemoveListener(ExplodeButton);
        hardwireButton.onClick.RemoveListener(HardwireButton);
        hackButton.onClick.RemoveListener(HackButton);
        cancelButton.onClick.RemoveListener(CancelButton);
        explodeButtonObj.SetActive(false);
        hardwireButtonObj.SetActive(false);
        hackButtonObj.SetActive(false);
        cancelButtonObj.SetActive(false);
        //FindObjectOfType<Interact>().DoneInteracting();
        openDoor.Open();
    }

    void CancelButton()
    {
        explodeButton.onClick.RemoveListener(ExplodeButton);
        hardwireButton.onClick.RemoveListener(HardwireButton);
        hackButton.onClick.RemoveListener(HackButton);
        cancelButton.onClick.RemoveListener(CancelButton);
        explodeButtonObj.SetActive(false);
        hardwireButtonObj.SetActive(false);
        hackButtonObj.SetActive(false);
        cancelButtonObj.SetActive(false);
        //FindObjectOfType<Interact>().DoneInteracting();
    }

    // Start is called before the first frame update
    void Start()
    {
        //explodeButton.onClick.AddListener(ExplodeButton);
        //hardwireButton.onClick.AddListener(HardwireButton);
        //hackButton.onClick.AddListener(HackButton);
    }

    /*
    // Update is called once per frame
    void FixedUpdate()
    {
        if (interacting == true)
            Interact();
        if (trigger == true)
            Trigger();
    }
    */
}
