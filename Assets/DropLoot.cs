using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public Inventory Inventory;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            Inventory = collision.gameObject.GetComponent<PlayerInventory>().inventory;
            if (Inventory.AddItem(GetComponent<ItemScript>()))
            {
                Destroy(gameObject);
            }
            //Inventory.AddItem(GetComponent<ItemScript>());
            //Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
