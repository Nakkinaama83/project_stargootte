using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public enum Category { EQUIPMENT, WEAPON, CONSUMEABLE }

public class ItemManager : MonoBehaviour
{
    public ItemType itemType;
    //public Quality quality;
    public Category category;
    public string spriteNeutral;
    public string spriteHighLighted;    
    public string itemName;
    public string description;
    public int maxSize;
    //public int intellect;
    //public int agility;
    //public int stamina;
    //public int strength;
    public int clipSize;
    public float attackSpeed;
    public int health;
    public int ammo;

    public void CreateItem()
    {
        ItemContainer itemContainer = new ItemContainer();

        Type[] itemTypes = { typeof(Equipment), typeof(Weapon), typeof(Consumeable) };

        FileStream fs = new FileStream(Path.Combine(Application.streamingAssetsPath, "Items.xml"), FileMode.Open);

        XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer), itemTypes);

        itemContainer = (ItemContainer)serializer.Deserialize(fs);

        serializer.Serialize(fs, itemContainer);

        fs.Close();

        switch (category)
        {
            case Category.EQUIPMENT:
                itemContainer.Equipment.Add(new Equipment(itemName, description, itemType, spriteNeutral, spriteHighLighted, maxSize)); //lisää tähän muita juttuja jos haluaa, esim quality,intellect etc..
                break;
            case Category.WEAPON:
                itemContainer.Weapons.Add(new Weapon(itemName, description, itemType, spriteNeutral, spriteHighLighted, maxSize, clipSize, attackSpeed)); //tähän täsmälleen samat kuin equipmentissa + attackSpeed
                break;
            case Category.CONSUMEABLE:
                itemContainer.Consumeables.Add(new Consumeable(itemName, description, itemType, spriteNeutral, spriteHighLighted, maxSize, health, ammo));
                break;
        }

        fs = new FileStream(Path.Combine(Application.streamingAssetsPath, "Items.xml"), FileMode.Create);
        serializer.Serialize(fs, itemContainer);
        fs.Close();
    }
}
