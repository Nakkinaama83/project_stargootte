using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInventories : MonoBehaviour
{
    public GameObject prefabPockets;
    public List<GameObject> existingPockets;
    public GameObject firstInActivePocket;
    //public GameObject firstActivePocket;

    // Start is called before the first frame update
    void Awake()
    {
        
        prefabPockets = Resources.Load("Prefabs/Pockets") as GameObject;
        firstInActivePocket = GameObject.FindWithTag("InActiveInventory");
        //firstActivePocket = GameObject.FindWithTag("ActiveInventory");
        existingPockets = new List<GameObject>(GameObject.FindGameObjectsWithTag("NotInSquadInventory"));
        if (firstInActivePocket == null)
        {
            Debug.Log(gameObject.name + " is the first one");
        }
        
         else if (existingPockets.Count > 0)
        {
            Debug.Log("I am " + gameObject.name + " and I am no. " + (existingPockets.Count+2));
            GameObject pockets = Instantiate<GameObject>(prefabPockets);
            pockets.tag = "NotInSquadInventory";
            pockets.transform.SetParent(GameObject.Find("AllInventories").transform, false);
            pockets.GetComponentInChildren<Inventory>().pelaaja = gameObject;
            pockets.GetComponentInChildren<CharacterPanel>().pelaaja = gameObject;
            pockets.GetComponentInChildren<Inventory>().name = "inventory" + existingPockets.Count;
            gameObject.GetComponent<PlayerInventory>().inventory = pockets.GetComponentInChildren<Inventory>();
        }
        else if (firstInActivePocket != null)
        {
            Debug.Log(gameObject.name + " is the second one");
            GameObject pockets = Instantiate<GameObject>(prefabPockets);
            pockets.tag = "NotInSquadInventory";
            pockets.transform.SetParent(GameObject.Find("AllInventories").transform, false);
            pockets.GetComponentInChildren<Inventory>().pelaaja = gameObject;
            pockets.GetComponentInChildren<CharacterPanel>().pelaaja = gameObject;
            pockets.GetComponentInChildren<Inventory>().name = "inventorySecond";
            gameObject.GetComponent<PlayerInventory>().inventory = pockets.GetComponentInChildren<Inventory>();
        }
    }

    
}
