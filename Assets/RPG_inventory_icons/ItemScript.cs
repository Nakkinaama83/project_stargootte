using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {HEALTH, AMMO, CONSUMEABLE, WEAPON, EQUIPMENT, COLLECTABLE, GENERIC};  //laita tähän lisää jos haluat


public class ItemScript : MonoBehaviour
{

    //public ItemType type; poista

    public Sprite spriteNeutral;

    public Sprite spriteHighLighted;

    private Item item;

    public Item Item
    {
        get => item;

        set
        {
            item = value;

            spriteHighLighted = Resources.Load<Sprite>(value.SpriteHighlighted);
            spriteNeutral = Resources.Load<Sprite>(value.SpriteNeutral);
        }
    }

    //public int maxSize; poista

    //public string itemName; poista

    //public string description; poista

    public void Use(GameObject pelaaja, Slot slot)
    {
        item.Use(pelaaja, slot, this);   //tähän täytyy lisätä tuo pelaaja
        //switch (type) voi poistaa mut mites pelaaja?
        //{
        //    case ItemType.AMMO:
        //        Debug.Log("Reload" + pelaaja);
        //        pelaaja.GetComponentInChildren<PlayerShooting>().AssaultClip = 30;
        //        break;
        //    case ItemType.HEALTH:
        //        Debug.Log("You just used a health kit");
        //        pelaaja.GetComponent<PlayerHealth>().currentHealth = 100;
        //        pelaaja.GetComponent<PlayerHealth>().HealthKit();
        //        break;
            
        //}
    }

    public string GetTooltip()
    {
        return item.GetToolTip();
        //string newLine = string.Empty;

        //if (description != string.Empty)
        //{
        //    newLine = "\n"; // \n takoittaa uutta riviä, sama kuin painasi enteriä teksti kentässä
        //}
        //return string.Format("{0}\n{1}", itemName, description); //return string.Format("<color=white><size=16>{0}</size></color><size14><i><color=lime>" + newLine + "{1}</color></i></size>", itemName, description); //return string.Format("<size=16>{0}</size><size14><i><color=lime>" + newLine + "{1}</color></size>", itemName, description);

    }

    //public void SetStats(ItemScript item) poistetaan kokonaan
    //{
    //    this.type = item.type;
    //    this.spriteNeutral = item.spriteNeutral;
    //    this.spriteHighLighted = item.spriteHighLighted;
    //    this.maxSize = item.maxSize;
    //    this.itemName = item.itemName;
    //    this.description = item.description;
    //}
}
