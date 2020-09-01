using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject box;

    public ChestInventory chestInventory;

    public int rows, slots;

    private List<Stack<ItemScript>> allSlots;

    private void Start()
    {
        allSlots = new List<Stack<ItemScript>>(slots);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            chestInventory.isOpen = true;
            chestInventory.UpdateLayout(allSlots, rows, slots);
            chestInventory.Open();
            box.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);  //tuo inventaarion ruutuun
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            chestInventory.isOpen = false;
            box.GetComponent<RectTransform>().anchoredPosition = new Vector3(500, 0, 0);    //vie inventaarion ulos ruudusta
            chestInventory.MoveItemsToChest();  //siirtää itemit tänne
        }
    }

    public void SaveInventory()
    {
        string content = string.Empty;  //tallentaa tähän slotin tiedot

        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i] != null && allSlots[i].Count > 0)
            {
                content += i + "-" + allSlots[i].Peek().Item.ItemName + "-" + allSlots[i].Count.ToString() + ";";
            }            
        }

        PlayerPrefs.SetString(gameObject.name + "content", content);  //tässä varsinaisesti tallennetaan kontetti
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        string content = PlayerPrefs.GetString(gameObject.name + "content");  //ladataan inventaarion contetti
        allSlots = new List<Stack<ItemScript>>();

        for (int i = 0; i < slots; i++)
        {
            allSlots.Add(new Stack<ItemScript>());
        }

        if (content != string.Empty)
        {
            string[] splitContent = content.Split(';'); //jakaa contentin osiin ; -merkin välein. esim. [0]0-AMMO-3;[1]2-HEALTH-2";

            for (int x = 0; x < splitContent.Length - 1; x++)     //length-1 siksi koska arrayssä on aina yksi ylimääräinen slotti joka on tyhjä, tai siis length on yksi mutta arraysä se on [0]
            {
                string[] splitValues = splitContent[x].Split('-');  //tämä jakaa ; -merkkien välistä saadut jutut. esim [0]0-AMMO-3; tuosta ottaa nuo - -merkkien välistä tyyliin 0,AMMO,3
                int index = Int32.Parse(splitValues[0]);    //tämä muuttaa sanan "0" numeroksi 0 eli "0" = 0
                string itemName = splitValues[1];     //tämä muuttaa sanan AMMO itemityypiksi. esim. "AMMO" = ItemType.AMMO
                int amount = Int32.Parse(splitValues[2]);   //tämä muuttaa sanan "3" numeroksi 3. eli amount = ("3" = 3)

                Item tmp = null;

                for (int i = 0; i < amount; i++)
                {
                    GameObject loadedItem = Instantiate(InventoryManager.Instance.itemObject);
                    if (tmp == null)
                    {
                        tmp = InventoryManager.Instance.ItemContain.Consumeables.Find(item => item.ItemName == itemName);
                    }
                    if (tmp == null)
                    {
                        tmp = InventoryManager.Instance.ItemContain.Equipment.Find(item => item.ItemName == itemName);
                    }
                    if (tmp == null)
                    {
                        tmp = InventoryManager.Instance.ItemContain.Weapons.Find(item => item.ItemName == itemName);
                    }

                    loadedItem.AddComponent<ItemScript>();
                    loadedItem.GetComponent<ItemScript>().Item = tmp;
                    allSlots[index].Push(loadedItem.GetComponent<ItemScript>());
                    Destroy(loadedItem);
                }

            }
        }
        if (chestInventory.isOpen == true)  //ihan vaan siltä varalta jos chesti on jo auki kun ladataan itemit playerprafabeistä
        {
            chestInventory.UpdateLayout(allSlots, rows, slots);
        }
    }
}
