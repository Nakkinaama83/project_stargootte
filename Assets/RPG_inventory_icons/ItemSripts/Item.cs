using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{ 
    public ItemType ItemType { get; set; }

    //public Quality Quality { get; set; }     //tuskin tarviin qualitya

    public string SpriteNeutral { get; set; }

    public string SpriteHighlighted { get; set; }

    public int MaxSize { get; set; }

    public string ItemName { get; set; }

    public string Description { get; set; }


    public Item()
    {

    }

    public Item(string itemName, string description, ItemType itemType, string spriteNeutral, string spriteHighLighted, int maxSize)    //jos haluaa käyttää qualitya niin se tulisi myös tuonne sulkujen väliin
    {

        this.ItemName = itemName;
        this.Description = description;
        this.ItemType = itemType;
        //this.Quality = quality;       //esimerkkinä jos käyttää qualitya
        this.SpriteNeutral = spriteNeutral;
        this.SpriteHighlighted = spriteHighLighted;
        this.MaxSize = maxSize;
    }

    public abstract void Use(GameObject pelaaja, Slot slot, ItemScript item);

    public virtual string GetToolTip()
    {
        string newLine = string.Empty;

        if (Description != string.Empty)
        {
            newLine = "\n"; // \n takoittaa uutta riviä, sama kuin painasi enteriä teksti kentässä
        }
        return string.Format("{0}\n{1}", ItemName, Description); //return string.Format("<color=white><size=16>{0}</size></color><size14><i><color=lime>" + newLine + "{1}</color></i></size>", itemName, description); //return string.Format("<size=16>{0}</size><size14><i><color=lime>" + newLine + "{1}</color></size>", itemName, description);
        
    }
}
