using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public Inventory Inventory;
    //public LootInventory LootInventory;   poista
    public GameObject box;
    //private List<GameObject> items;

    // Start is called before the first frame update
    void Start()
    {
        //Inventory.AddItem(GetComponent<Item>());  //tapahtuu liian nopeesti eikä stackin numeron skaalaus vielä toimi
        //Inventory.AddItem(GetComponent<Item>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //list items
            //items = new list(getcomponents<item>)
            //foreach item in list, inventory.additem   //looppi jollai idein tuoda kaikki itemit inventaarioon
            //Inventory.GetComponent<LootInventory>().AddItem(GetComponent<Item>());    //jotain skeidaa, poista
            //Inventory.AddItem(GetComponent<ItemScript>());  //lisää itemin inventaarioon, toimii
            int randomType = UnityEngine.Random.Range(0, 3);
            GameObject tmp = Instantiate(InventoryManager.Instance.itemObject);
            int randomItem;
            tmp.AddComponent<ItemScript>();
            ItemScript newItem = tmp.GetComponent<ItemScript>();
            switch (randomType)
            {
                case 0:
                    
                   
                    randomItem = UnityEngine.Random.Range(0, InventoryManager.Instance.ItemContain.Consumeables.Count);
                    newItem.Item = InventoryManager.Instance.ItemContain.Consumeables[randomItem];
                    
                    
                    break;
                case 1:
                    
                   
                    randomItem = UnityEngine.Random.Range(0, InventoryManager.Instance.ItemContain.Weapons.Count);
                    newItem.Item = InventoryManager.Instance.ItemContain.Weapons[randomItem];
                    
                    
                    break;
                case 2:
                    
                    
                    randomItem = UnityEngine.Random.Range(0, InventoryManager.Instance.ItemContain.Equipment.Count);
                    newItem.Item = InventoryManager.Instance.ItemContain.Equipment[randomItem];
                    
                    
                    break;
            }
            Inventory.AddItem(newItem);
            Destroy(tmp);
            box.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);  //tuo inventaarion ruutuun
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            box.GetComponent<RectTransform>().anchoredPosition = new Vector3(500, 0, 0);    //vie inventaarion ulos ruudusta
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
