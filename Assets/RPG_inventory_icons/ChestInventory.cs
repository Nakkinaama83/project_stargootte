using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventory : Inventory
{
    public bool isOpen = false;
    private List<Stack<ItemScript>> chestItems;
    private int chestSlots;
    public int maxSlot;

    public override void CreateLayout()
    {
        allSlots = new List<GameObject>();

        for (int i = 0; i < maxSlot; i++)
        {
            GameObject newSlot = Instantiate(InventoryManager.Instance.slotPrefab);

            newSlot.name = "Slot";

            newSlot.transform.SetParent(this.transform);

            allSlots.Add(newSlot);

            newSlot.SetActive(false);
        }
    }

    public void UpdateLayout(List<Stack<ItemScript>> items, int rows, int slots)
    {
        this.chestItems = items;
        this.chestSlots = slots;

        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;

        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        int columns = slots / rows;

        int index = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = allSlots[index];

                RectTransform slotRect = newSlot.GetComponent<RectTransform>(); 

                newSlot.transform.SetParent(gameObject.transform); //(this.transform.parent)

                //newSlot.GetComponent<Slot>().pelaaja = pelaaja;  //tähän pitäis syöttää slotin omistaja eli pelaaja-gameobject. Jaa, tarviikohan tätä?

                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * InventoryManager.Instance.canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor);

                newSlot.transform.SetParent(this.transform);


                if (items.Count != 0 && items.Count >= index && items[index].Count > 0)
                {
                    newSlot.GetComponent<Slot>().AddItems(items[index]);
                }

                index++;
            }
        }
    }

    public void MoveItemsToChest()
    {
        chestItems.Clear();

        for (int i = 0; i < chestSlots; i++)
        {
            Slot tmpSlot = allSlots[i].GetComponent<Slot>();

            if (!tmpSlot.IsEmpty)
            {
                chestItems.Add(new Stack<ItemScript>(tmpSlot.Items));

                if (isOpen == false)   //inventaario ei ole auki
                {
                    tmpSlot.ClearSlot();
                }
            }

            else
            {
                chestItems.Add(new Stack<ItemScript>());
            }
            if (isOpen == false)
            {
                allSlots[i].SetActive(false);
            }
        }
    }
    public void MoveItemsFromChest()
    {
        for (int i = 0; i < chestSlots; i++)
        {
            if (chestItems.Count !=0 && chestItems.Count >= i && chestItems[i] != null && chestItems[i].Count > 0)
            {
                GameObject newSlot = allSlots[i];
                newSlot.GetComponent<Slot>().AddItems(chestItems[i]);
            }
        }
        for (int i = 0; i < chestSlots; i++)
        {
            allSlots[i].SetActive(true);
        }
    }
    public void Open()
    {
        if (isOpen == true)
        {
            MoveItemsFromChest();
        }
    }

    public override void SaveInventory()    //tyhjäksi siksi ettei suorita uudestaan koska tehdään jo chestscriptissä
    {
        
    }
    public override void LoadInventory()    //tyhjäksi siksi ettei suorita uudestaan koska tehdään jo chestscriptissä
    {
        
    }
}
