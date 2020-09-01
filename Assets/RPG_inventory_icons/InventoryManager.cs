using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    private static InventoryManager instance;

    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryManager>();
            }
            return instance;
        }
    }

    public GameObject itemObject;

    public GameObject slotPrefab;

    public GameObject iconPrefab;

    private GameObject hoverObject;
    public GameObject HoverObject { get => hoverObject; set => hoverObject = value; }

    //public GameObject ammo; //näitä käytetään inventaarion lataamiseen, voi nytten poistaa
    //public GameObject health;

    public GameObject dropItem;     //droItem prefab, reppu

    public GameObject tooltipObject;//näitä käytetään info laatikoihin
    public Text sizeTextObject;
    public Text visualTextObject;

    public Canvas canvas;

    private Slot from;
    public Slot From { get => from; set => from = value; }
    
    private Slot to;
    public Slot To { get => to; set => to = value; }
    

    private GameObject clicked;
    public GameObject Clicked { get => clicked; set => clicked = value; }
    

    public Text stackText;

    public GameObject selectStackSize;

    private int splitAmount;
    public int SplitAmount { get => splitAmount; set => splitAmount = value; }
    

    private int maxStackCount;
    public int MaxStackCount { get => maxStackCount; set => maxStackCount = value; }

    private Slot movingSlot;
    public Slot MovingSlot { get => movingSlot; set => movingSlot = value; }
    

    public EventSystem eventSystem;

    private ItemContainer itemContain = new ItemContainer();
    public ItemContainer ItemContain { get => itemContain; set => itemContain = value; }

    public void Start()
    {
        Type[] itemTypes = { typeof(Equipment), typeof(Weapon), typeof(Consumeable) };
        XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer), itemTypes);
        TextReader textReader = new StreamReader(Application.streamingAssetsPath + "/Items.xml");
        itemContain = (ItemContainer)serializer.Deserialize(textReader);
        textReader.Close();
    }

    public void SetStackInfo(int maxstackCount)
    {
        selectStackSize.SetActive(true);
        tooltipObject.SetActive(false);
        splitAmount = 0;
        maxStackCount = maxstackCount;
        //this.maxStackCount = maxStackCount;
        stackText.text = splitAmount.ToString();
    }

    public void Save()
    {
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");

        foreach (GameObject inventory in inventories)
        {
            inventory.GetComponent<Inventory>().SaveInventory();
        }
        foreach (GameObject chest in chests)
        {
            chest.GetComponent<ChestScript>().SaveInventory();
        }
    }

    public void Load()
    {
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");

        foreach (GameObject inventory in inventories)
        {
            inventory.GetComponent<Inventory>().LoadInventory();
        }
        foreach (GameObject chest in chests)
        {
            chest.GetComponent<ChestScript>().LoadInventory();
        }
    }
}
