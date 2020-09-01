using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public int ClipSize { get; set; }
    public float AttackSpeed { get; set; }
    CharacterPanel characterPanel;

    public Weapon()
    {

    }

    public Weapon(string itemName, string description, ItemType itemType, string spriteNeutral, string spriteHighLighted, int maxSize, int clipSize, float attackSpeed) : base(itemName, description, itemType, spriteNeutral, spriteHighLighted, maxSize) //jos käytät noita ylhäällä olevia statteja niin syötä ne myös tähän
    {
        //: base(itemName, description, itemType, spriteNeutral, spriteHighLighted, maxSize, intellect, agility, stamina, strength) //jos käytät statteja niin silloin tällä tavalla
        this.ClipSize = clipSize;
        this.AttackSpeed = attackSpeed;
    }

    public override void Use(GameObject pelaaja, Slot slot, ItemScript item)
    {
        characterPanel = slot.transform.parent.parent.GetComponentInChildren<CharacterPanel>();
        //characterPanel = characterPanel.GetComponentInParent<CharacterPanel>();
        characterPanel.EquipItem(slot, item);
        //CharacterPanel.Instance.EquipItem(slot, item);    //tää oli alunperin
        Debug.Log("You equipped your " + ItemName);
    }

    public override string GetToolTip()
    {
        string equipmentTip = base.GetToolTip();
        return string.Format("{0} \nRate of fire: {1} \nClip Size: {2}", equipmentTip, AttackSpeed, ClipSize);
        
    }
}
