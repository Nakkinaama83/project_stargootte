using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer
{
    private List<Item> weapons = new List<Item>();
    private List<Item> equipment = new List<Item>();
    private List<Item> consumeables = new List<Item>();

    public List<Item> Weapons { get => weapons; set => weapons = value; }
    public List<Item> Equipment { get => equipment; set => equipment = value; }
    public List<Item> Consumeables { get => consumeables; set => consumeables = value; }

    public ItemContainer()
    {

    }
}
