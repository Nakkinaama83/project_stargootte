using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryMover : MonoBehaviour
//public class InventoryController : MonoBehaviour
{
    public Transform inventoryOpen;
    public Transform inventoryClosed;
    private float smoothing = 5f;
    private bool inventoryEnabled;    
    private Transform target;

    private GameObject activeInventory;
    private List<GameObject> inActiveInventories;
    private List<GameObject> notInSquad;

    public void SquadUpdate()
    {
        
        Debug.Log("ReArrange inventory");
        
        activeInventory = GameObject.FindGameObjectWithTag("ActiveInventory");
        activeInventory.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -20, 0);
        inActiveInventories = new List<GameObject>(GameObject.FindGameObjectsWithTag("InActiveInventory"));
        notInSquad = new List<GameObject>(GameObject.FindGameObjectsWithTag("NotInSquadInventory"));
        Vector3 newPosition = new Vector3(0, -70, 0);
        if (inActiveInventories.Count > 0)
        {
            foreach (GameObject i in inActiveInventories)
            {
                i.GetComponent<RectTransform>().anchoredPosition = newPosition;
                //i.transform.position = i.transform.position + newPosition;
                newPosition = newPosition + new Vector3(0, -50, 0);
            }
        }
        Vector3 notInScreenPosition = new Vector3(0, -500, 0);
        if (notInSquad.Count > 0)
        {
            foreach (GameObject a in notInSquad)
            {
                a.GetComponent<RectTransform>().anchoredPosition = notInScreenPosition;
            }
        }
        notInSquad.Clear();
        inActiveInventories.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        //inventoryOpen = new Vector3(0, 500, 0);
        //Squad = GameObject.FindObjectOfType<PlayerChange>().players;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
        }
        if (inventoryEnabled == true)
        {

            target = inventoryOpen;
            transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
            //inventoryOpen.position = inventoryOpen.position + new Vector3(0, 100, 0);
        
            //inventory.SetActive(true);
        }
        else
        {

            target = inventoryClosed;     
            transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
         
            
            //inventory.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        //activeInventory.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -20, 0);
        //activeInventory.transform.position = new Vector3(-370, -25, 0);
    }
}
