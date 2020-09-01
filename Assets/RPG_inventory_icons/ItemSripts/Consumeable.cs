using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumeable : Item
{
    public int Health { get; set; }

    public int Ammo { get; set; }

    public Consumeable()
    {

    }

    public Consumeable(string itemName, string description, ItemType itemType, string spriteNeutral, string spriteHighLighted, int maxSize, int health, int ammo) : base(itemName,description,itemType,spriteNeutral,spriteHighLighted,maxSize)
    {
        this.Health = health;
        this.Ammo = ammo;
    }

    public override void Use(GameObject pelaaja, Slot slot, ItemScript item)
    {
        switch (ItemType)
        {
            case ItemType.HEALTH:
                Debug.Log("You just used a " + ItemName);
                pelaaja.GetComponent<PlayerHealth>().currentHealth = 100;
                pelaaja.GetComponent<PlayerHealth>().HealthKit();
                slot.RemoveItem();
                if (slot.IsEmpty)
                {
                    slot.inventory.emptySlots++;
                }
                break;
            case ItemType.AMMO:
                Debug.Log("You just used a " + ItemName);
                break;
        }
        //Debug.Log("You just used a " + ItemName);
    }

    public override string GetToolTip()
    {
        string stats = string.Empty;

        if (Health > 0)
        {
            stats += "\nRestores " + Health.ToString() + " Health";
        }
        if (Ammo > 0)
        {
            stats += "\nUse to reload your Weapon";
        }

        string itemTip = base.GetToolTip();

        return string.Format("{0}" + "{1}", itemTip, stats);    //"{0}" + "<size=14>{1}</size>" toimii tuo koon muoks
    }
}
