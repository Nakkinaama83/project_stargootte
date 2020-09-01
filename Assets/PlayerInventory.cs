using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;
    public bool isInSquad = false;
    private string playerName;
    private string playerClass;
    //private GameObject pelaaja; //tehdään suoraan inventaarioon

    public void ActiveInventory()   //siirtelee ensimmäisen hahmon inventaariota
    {
        playerName = GetComponent<PlayerHealth>().PlayerName;
        playerClass = gameObject.name.ToString();
        inventory.ActiveInventory(Name: playerName, Type: playerClass);
        FindObjectOfType<InventoryMover>().SquadUpdate();
    }
    public void InActiveInventory()
    {
        playerName = GetComponent<PlayerHealth>().PlayerName;
        playerClass = gameObject.name.ToString();
        inventory.InActiveInventory(Name: playerName, Type: playerClass);
    }
    
    public void IsInSquad() //siirtelee kaikki loput inventaariot
    {
        if (isInSquad == true)
        {
            playerName = GetComponent<PlayerHealth>().PlayerName;
            playerClass = gameObject.name.ToString();
            inventory.InActiveInventory(Name: playerName, Type: playerClass);
            FindObjectOfType<InventoryMover>().SquadUpdate();
        }
        if (isInSquad == false)
        {
            inventory.NotInSquadInventory();
        }
    }

    private void Awake()
    {
        //playerClass = gameObject.name.ToString(); //tapahtuu liian lopeesti, ei ole vielä nimeä. Thymä, käytä Awake ja Start Funktioita. Awake tapahtuu aina ensin.
        //pelaaja = gameObject;
        //inventory.pelaaja = pelaaja;
    }

}
