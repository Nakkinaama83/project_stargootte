using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : Inventory
{
    public Slot[] equipmentSlots;

    

    //private static CharacterPanel instance;

    //public static CharacterPanel Instance   //tulee ongelmia staticin kanssa kun on useempi hahmo, poistaminen ratkaisi osan
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
                
    //            instance = GameObject.FindObjectOfType<CharacterPanel>();
    //        }
    //        return CharacterPanel.instance;
    //    }
    //}

    public Slot WeaponSlot1
    {
        get { return equipmentSlots[0]; }
    }
    public Slot WeaponSlot2
    {
        get { return equipmentSlots[1]; }
    }
    public Slot EquipmentSlot
    {
        get { return equipmentSlots[2]; }
    }

    private void Awake()
    {
        equipmentSlots = transform.GetComponentsInChildren<Slot>();
    }

    public override void CreateLayout()
    {
        
    }

    public void EquipItem(Slot slot, ItemScript item)   //UseItem eli oikeella nappulalla tapahtuu tämä
    {
        if (item.Item.ItemType == ItemType.WEAPON && WeaponSlot1.IsEmpty)  //&& slot.isempty
        {
            Slot.SwapItems(slot, WeaponSlot1);
        }
        else if (item.Item.ItemType == ItemType.WEAPON && WeaponSlot2.IsEmpty)
        {
            Slot.SwapItems(slot, WeaponSlot2);
        }
        else
        {
            Slot.SwapItems(slot, Array.Find(equipmentSlots, x => x.canContain == item.Item.ItemType));  //varmaankin pakko käyttää ilman if-käskyjä
        }

    }
    public override void SaveInventory()
    {
        string content = string.Empty;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (!equipmentSlots[i].IsEmpty)
            {
                content += i + "-" + equipmentSlots[i].Items.Peek().Item.ItemName + ";";    //0-AssaultRifle; 0-on slotti ja AssaultRifle on itemin nimi
            }
        }
        PlayerPrefs.SetString("CharPanel", content);
        PlayerPrefs.Save();
    }

    public override void LoadInventory()
    {
        foreach (Slot slot in equipmentSlots)
        {
            slot.ClearSlot();
        }

        string content = PlayerPrefs.GetString("CharPanel");
        string[] splitContent = content.Split(';');

        for (int i = 0; i < splitContent.Length-1; i++) //-1 siksi kun aina tulee ylim "tyhjä" kun lasku aloitetaan nollasta, eli nolla on ykkönen ja niin eespäin. Vähennetään yhellä jotta numerot mätsää
        {
            string[] splitValues = splitContent[i].Split('-');  //[0]1 [1]AssaultRifle
            int index = Int32.Parse(splitValues[0]);
            string itemName = splitValues[1];

            GameObject loadedItem = Instantiate(InventoryManager.Instance.itemObject);

            loadedItem.AddComponent<ItemScript>();

            if (index == 0 || index == 1)
            {
                loadedItem.GetComponent<ItemScript>().Item = InventoryManager.Instance.ItemContain.Weapons.Find(x => x.ItemName == itemName);
            }
            else
            {
                loadedItem.GetComponent<ItemScript>().Item = InventoryManager.Instance.ItemContain.Equipment.Find(x => x.ItemName == itemName);
            }

            equipmentSlots[index].AddItem(loadedItem.GetComponent<ItemScript>());

            Destroy(loadedItem);
        }
    }

    public void PushWeaponsToPlayerShooting()
    {
        pelaaja.GetComponentInChildren<PlayerShooting>().weapons.Clear();
        if (!WeaponSlot1.IsEmpty)
        {
            pelaaja.GetComponentInChildren<PlayerShooting>().weapons.Add(WeaponSlot1.GetComponent<Slot>().CurrentItem.Item.ItemName.ToString());    //toimii muuten mut ei vielä ole slotissa mitään
        }
        if (!WeaponSlot2.IsEmpty)
        {
            pelaaja.GetComponentInChildren<PlayerShooting>().weapons.Add(WeaponSlot2.GetComponent<Slot>().CurrentItem.Item.ItemName.ToString());
        }
        
    }
    public void PushGadgetToEquipGadget()
    {
        if (!EquipmentSlot.IsEmpty)
        {
            pelaaja.GetComponent<PlayerMovement>().gadget = EquipmentSlot.GetComponent<Slot>().CurrentItem.Item.ItemName.ToString();
        }
        if (EquipmentSlot.IsEmpty)
        {
            pelaaja.GetComponent<PlayerMovement>().gadget = null;
        }
    }
}
