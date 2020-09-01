using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    //public int Intellect { get; set; }  //en käytä näitä
    //public int Agility { get; set; }  //en käytä näitä
    //public int Stamina { get; set; }  //en käytä näitä
    //public int Strength { get; set; }  //en käytä näitä
    CharacterPanel characterPanel;

    public Equipment()
    {

    }

    public Equipment(string itemName, string description, ItemType itemType, string spriteNeutral, string spriteHighLighted, int maxSize) : base(itemName, description, itemType, spriteNeutral, spriteHighLighted, maxSize) //jos käytät noita ylhäällä olevia statteja niin syötä ne myös tähän
    {
        //this.Intellect = intellect;   //nää vain esimerkkinä jos haluaa käyttää näitä
        //this.Agility = agility;
        //this.Stamina = stamina;
        //this.Strength = strength;
    }

    public override void Use(GameObject pelaaja, Slot slot, ItemScript item)
    {

        characterPanel = slot.transform.parent.parent.GetComponentInChildren<CharacterPanel>();
        characterPanel.EquipItem(slot, item);
        //CharacterPanel.Instance.EquipItem(slot, item);    //tää oli alunperin
        Debug.Log("You just ate your collectable!");
    }

    public override string GetToolTip()
    {
        //string stats = string.Empty;
        //if (true)
        //{

        //}
        return base.GetToolTip();
    }
}
