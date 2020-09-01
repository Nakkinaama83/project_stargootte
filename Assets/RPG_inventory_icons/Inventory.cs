using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Inventory inventory;

    public GameObject pelaaja;
    
    public RectTransform inventoryRect;

    public float inventoryWidth, inventoryHeight;

    public int slots;

    public int rows;

    public float slotPaddingLeft, slotPaddingTop;

    public float slotSize;

    public List<GameObject> allSlots;

    private float hoverYOffset;

    //private static Inventory instance;    voi poistaa, jäi pyörimään aiemmasta häröilystä

    //public static Inventory Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = GameObject.FindObjectOfType<Inventory>();
    //        }
    //        return Inventory.instance;
    //    }
    //}

    public int emptySlots; //oli static, ja äsken oli private

    public int EmptySlots { get => emptySlots; set => emptySlots = value; } //oli myös static
    
    
    // Start is called before the first frame update
    void Start()
    {
       
        
        inventory = gameObject.GetComponentInParent<Inventory>();
        CreateLayout();

        InventoryManager.Instance.MovingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
    }

    private void Update()   //pitäsikö siirtää inventoryManageriin?
    {
        if(Input.GetMouseButtonUp(0))   //checks if mouse putton has lifted
        {
            if (!InventoryManager.Instance.eventSystem.IsPointerOverGameObject(-1) && InventoryManager.Instance.From != null && InventoryManager.Instance.From.pelaaja == pelaaja)   //If we click outside of the inventory and we have picked up an item with the whole stack
            {
                InventoryManager.Instance.From.GetComponent<Image>().color = Color.white;     //resets the slot color

                foreach (ItemScript item in InventoryManager.Instance.From.Items)
                {                                      
                    float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2); //laskee ympyrästä randomi luvun asteina
                    Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)); //laskee koordinaatit x,y,z joista x ja z on randomeita
                    v *= 1;
                    GameObject tmpDrp = GameObject.Instantiate(InventoryManager.Instance.dropItem, pelaaja.transform.position - v, Quaternion.identity);  //luo dropItem prefabin, GameObject.Instantiate(mikä, minne, rotaatio);
                    tmpDrp.AddComponent<ItemScript>(); //jostain syystä pelaaja-gameobjecti on väärä hahmo, ekan hahmon inv kerkee tehä eikä haluttu hahmo tee sitä. ehkä siirto inventoryManageriin ratkaisee
                    tmpDrp.GetComponent<ItemScript>().Item = item.Item;
                    Debug.Log(pelaaja + " dropped an item");                
                }
                    

                InventoryManager.Instance.From.ClearSlot();   //removes the item from the slot
                Destroy(GameObject.Find("Hover"));

                InventoryManager.Instance.To = null;
                InventoryManager.Instance.From = null;
                InventoryManager.Instance.HoverObject = null;
                //emptySlots++; //ei tarvii enään tai muuten tulee liikaa slotteja. tapahtuu jo slot-scriptissä
            }
            else if (!InventoryManager.Instance.eventSystem.IsPointerOverGameObject(-1) && !InventoryManager.Instance.MovingSlot.IsEmpty && pelaaja != null)   //checks if we splitted stack we are carrying
            {
                foreach (ItemScript item in InventoryManager.Instance.MovingSlot.Items)
                {
                    float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2); //laskee ympyrästä randomi luvun asteina
                    Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)); //laskee koordinaatit x,y,z joista x ja z on randomeita
                    v *= 1;
                    GameObject tmpDrp = GameObject.Instantiate(InventoryManager.Instance.dropItem, pelaaja.transform.position - v, Quaternion.identity);  //luo dropItem prefabin, GameObject.Instantiate(mikä, minne, rotaatio);
                    tmpDrp.AddComponent<ItemScript>(); //jostain syystä pelaaja-gameobjecti on väärä hahmo, ekan hahmon inv kerkee tehä eikä haluttu hahmo tee sitä
                    tmpDrp.GetComponent<ItemScript>().Item = item.Item;
                }
                InventoryManager.Instance.MovingSlot.ClearSlot(); //removes items from movingSlot
                Destroy(GameObject.Find("Hover"));
            }
        }
        if (InventoryManager.Instance.HoverObject != null)    //checks if the hoverobject exists
        {
            Vector2 position;   //hoverobject position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition, InventoryManager.Instance.canvas.worldCamera, out position);  //translates the mouse screen position into a local position and stores it in the position
            position.Set(position.x, position.y - hoverYOffset);    //adds the offset to the position
            InventoryManager.Instance.HoverObject.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(position); //sets the hoverobject's position
        }
    }

    public void ShowToolTip(GameObject slot)    //nimi kertoo kaiken, luo näkyviin infolaatikon
    {
        Slot tmpSlot = slot.GetComponent<Slot>();

        if (!tmpSlot.IsEmpty && InventoryManager.Instance.HoverObject == null && !InventoryManager.Instance.selectStackSize.activeSelf)    //&& !selectStackSizeStatic.activeSelf) jos slotissa on jotain ja kädessä ei ole mitään ja itemien valinta ei ole käynnissä
        {
            InventoryManager.Instance.visualTextObject.text = tmpSlot.CurrentItem.GetTooltip();
            InventoryManager.Instance.sizeTextObject.text = InventoryManager.Instance.visualTextObject.text;

            InventoryManager.Instance.tooltipObject.SetActive(true);

            float xPos = slot.transform.position.x + slotPaddingLeft;   //lasketaan koordinaatit infolaatikolle
            float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;

            InventoryManager.Instance.tooltipObject.transform.position = new Vector2(xPos, yPos);   //asetetaan infolaatikon paikka
        }
        
    }

    public void HideToolTip()   //piilottaa infolaatikon
    {
        InventoryManager.Instance.tooltipObject.SetActive(false);
    }

    public virtual void SaveInventory()
    {
        string content = string.Empty;  //tallentaa tähän slotin tiedot

        for (int i = 0; i < allSlots.Count; i++)
        {
            Slot tmp = allSlots[i].GetComponent<Slot>();

            if (!tmp.IsEmpty)
            {
                content += i + "-" + tmp.CurrentItem.Item.ItemName.ToString() + "-" + tmp.Items.Count.ToString() + ";";  //tallentaa slotin tiedot content-sanaan
            }
        }

        PlayerPrefs.SetString(gameObject.name + "content", content);  //tässä varsinaisesti tallennetaan kontetti
        PlayerPrefs.SetInt(gameObject.name + "slots", slots); //tässä tallennetaan inventaarion koko
        PlayerPrefs.SetInt(gameObject.name + "rows", rows);   //tässä tallennetaan inventaarion koko
        PlayerPrefs.SetFloat(gameObject.name + "slotPaddingLeft", slotPaddingLeft);
        PlayerPrefs.SetFloat(gameObject.name + "slotPaddingTop", slotPaddingTop);
        PlayerPrefs.SetFloat(gameObject.name + "slotSize", slotSize);
        PlayerPrefs.Save();
    }

    public virtual void LoadInventory()
    {
        string content = PlayerPrefs.GetString(gameObject.name + "content");  //ladataan inventaarion contetti
        if (content != string.Empty)
        {


            slots = PlayerPrefs.GetInt(gameObject.name + "slots");    //näissä asetetaan inventaarion koko uudestaan
            rows = PlayerPrefs.GetInt(gameObject.name + "rows");
            slotPaddingLeft = PlayerPrefs.GetFloat(gameObject.name + "slotPaddingLeft");
            slotPaddingTop = PlayerPrefs.GetFloat(gameObject.name + "slotPaddingTop");
            slotSize = PlayerPrefs.GetFloat(gameObject.name + "slotSize");

            CreateLayout(); //luodaan inventaario uudestaan

            string[] splitContent = content.Split(';'); //jakaa contentin osiin ; -merkin välein. esim. [0]0-AMMO-3;[1]2-HEALTH-2";

            for (int x = 0; x < splitContent.Length - 1; x++)     //length-1 siksi koska arrayssä on aina yksi ylimääräinen slotti joka on tyhjä, tai siis length on yksi mutta arraysä se on [0]
            {
                string[] splitValues = splitContent[x].Split('-');  //tämä jakaa ; -merkkien välistä saadut jutut. esim [0]0-AMMO-3; tuosta ottaa nuo - -merkkien välistä tyyliin 0,AMMO,3
                int index = Int32.Parse(splitValues[0]);    //tämä muuttaa sanan "0" numeroksi 0 eli "0" = 0
                string itemName = splitValues[1];     //tämä muuttaa sanan AMMO itemityypiksi. esim. "AMMO" = ItemType.AMMO
                int amount = Int32.Parse(splitValues[2]);   //tämä muuttaa sanan "3" numeroksi 3. eli amount = ("3" = 3)

                Item tmp = null;

                for (int i = 0; i < amount; i++)
                {
                    GameObject loadedItem = Instantiate(InventoryManager.Instance.itemObject);
                    if (tmp == null)
                    {
                        tmp = InventoryManager.Instance.ItemContain.Consumeables.Find(item => item.ItemName == itemName);
                    }
                    if (tmp == null)
                    {
                        tmp = InventoryManager.Instance.ItemContain.Equipment.Find(item => item.ItemName == itemName);
                    }
                    if (tmp == null)
                    {
                        tmp = InventoryManager.Instance.ItemContain.Weapons.Find(item => item.ItemName == itemName);
                    }

                    loadedItem.AddComponent<ItemScript>();
                    loadedItem.GetComponent<ItemScript>().Item = tmp;
                    allSlots[index].GetComponent<Slot>().AddItem(loadedItem.GetComponent<ItemScript>());
                    Destroy(loadedItem);
                }

            }
        }
    }

    public virtual void CreateLayout()
    {
        if (allSlots != null)   //jos allslots ei ole tyhjä
        {
            foreach (GameObject go in allSlots) //hakee kaikki slotit ja tuhoaa ne ennenkuin luodaan uusi inventaario
            {
                Destroy(go);
            }
        }

        allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.01f;

        emptySlots = slots;

        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;

        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        int columns = slots / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = (GameObject)Instantiate(InventoryManager.Instance.slotPrefab);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.name = "Slot";

                newSlot.transform.SetParent(gameObject.transform); //(this.transform.parent)

                newSlot.GetComponent<Slot>().pelaaja = pelaaja;  //tähän pitäis syöttää slotin omistaja eli pelaaja-gameobject.

                newSlot.GetComponent<Slot>().inventory = inventory;

                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * InventoryManager.Instance.canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor);

                allSlots.Add(newSlot);
            }
        }

    }

    public bool AddItem(ItemScript item)
    {
        if (item.Item.MaxSize == 1)  //if the item isn't stackable
        {
            return PlaceEmpty(item);   //places the item on an empty slot
            
        }
        else    //if the item is stackable
        {
            foreach (GameObject slot in allSlots)   //runs through all the slots in the inventory
            {
                Slot tmp = slot.GetComponent<Slot>();   //creates a reference to the slot

                if (!tmp.IsEmpty)   //if the item isn't empty
                {
                    if (tmp.CurrentItem.Item.ItemName == item.Item.ItemName && tmp.IsAvailable)   //checks if the item is the same type as the item we want to pick up
                    {
                        tmp.AddItem(item);  //adds the item to inventory, stackkää saman itemin samaan slottiin
                        return true;
                    }
                }
            }
            if (emptySlots > 0) //places the item on empty slots
            {
                return PlaceEmpty(item);    //tää oli ilman returina, kokeile toimiiko? Taitaa toimia
            }
        }

        return false;
    }
    
    private bool PlaceEmpty(ItemScript item)  //lisää itemin tyhjään slottin
    {
        if (emptySlots > 0) //if we have at least 1 emptyslot
        {
            foreach (GameObject slot in allSlots)   //runs through all slots
            {
                Slot tmp = slot.GetComponent<Slot>();   //creates reference to the slot

                if (tmp.IsEmpty)    //if the slot is empty
                {
                    tmp.AddItem(item);  //adds the item
                    //emptySlots--; //tää siirrettiin slot.cs additemin alle
                    return true;
                }
            }
        }

        return false;
    }
    public void MoveItem(GameObject clicked)    //jos liikutellaan koko stackkiä, hmmm.. eipäs
    {
        InventoryManager.Instance.Clicked = clicked;

        if (!InventoryManager.Instance.MovingSlot.IsEmpty)    //jos movingslot ei ole tyhjä
        {
            Slot tmp = clicked.GetComponent<Slot>();

            if (tmp.IsEmpty)
            {
                tmp.AddItems(InventoryManager.Instance.MovingSlot.Items);
                InventoryManager.Instance.MovingSlot.Items.Clear();
                Destroy(GameObject.Find("Hover"));
            }
            else if (!tmp.IsEmpty && InventoryManager.Instance.MovingSlot.CurrentItem.Item.ItemName == tmp.CurrentItem.Item.ItemName && tmp.IsAvailable)
            {
                MergeStacks(InventoryManager.Instance.MovingSlot, tmp);
            }
        }

        else if (InventoryManager.Instance.From == null && !Input.GetKey(KeyCode.LeftShift)) // if we haven't picked up an item
        {
            if (!clicked.GetComponent<Slot>().IsEmpty && !GameObject.Find("Hover")) //if the slot we clicked isn't empty and we aren't carrying anything
            {
                InventoryManager.Instance.From = clicked.GetComponent<Slot>();
                InventoryManager.Instance.From.GetComponent<Image>().color = Color.gray;  //sets item color to gray to indicate which slot we are moving
                CreateHoverIcon();
            }
        }
        else if (InventoryManager.Instance.To == null && !Input.GetKey(KeyCode.LeftShift))    //selects the slot we are moving to
        {
            InventoryManager.Instance.To = clicked.GetComponent<Slot>();  //sets the to object
            Destroy(GameObject.Find("Hover"));
        }
        if (InventoryManager.Instance.To != null && InventoryManager.Instance.From != null) //if both to and from are null then we are done moving, tänne pitää jonnekkin lisätä mergestacks
        {
            if (!InventoryManager.Instance.To.IsEmpty && InventoryManager.Instance.From.CurrentItem.Item.ItemName == InventoryManager.Instance.To.CurrentItem.Item.ItemName && InventoryManager.Instance.To.IsAvailable)
            {
                MergeStacks(InventoryManager.Instance.From, InventoryManager.Instance.To);
            }
            else
            {
                Slot.SwapItems(InventoryManager.Instance.From, InventoryManager.Instance.To);
            }
           
            InventoryManager.Instance.From.GetComponent<Image>().color = Color.white; //reset all the values
            InventoryManager.Instance.To = null;
            InventoryManager.Instance.From = null;
            Destroy(GameObject.Find("Hover"));
            //hoverObject = null;
            
        }
    }

    private void CreateHoverIcon()
    {
        InventoryManager.Instance.HoverObject = (GameObject)Instantiate(InventoryManager.Instance.iconPrefab);
        InventoryManager.Instance.HoverObject.GetComponent<Image>().sprite = InventoryManager.Instance.Clicked.GetComponent<Image>().sprite;
        InventoryManager.Instance.HoverObject.name = "Hover";
        InventoryManager.Instance.HoverObject.GetComponent<Image>().raycastTarget = false;

        RectTransform hoverTransform = InventoryManager.Instance.HoverObject.GetComponent<RectTransform>();
        RectTransform clickedTransform = InventoryManager.Instance.Clicked.GetComponent<RectTransform>();

        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

        InventoryManager.Instance.HoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
        InventoryManager.Instance.HoverObject.transform.localScale = InventoryManager.Instance.Clicked.gameObject.transform.localScale;

        InventoryManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = InventoryManager.Instance.MovingSlot.Items.Count > 1 ? InventoryManager.Instance.MovingSlot.Items.Count.ToString() : string.Empty;
    }

    

    public void SplitStack()    //ok-nappula stackki laatikossa
    {
        InventoryManager.Instance.selectStackSize.SetActive(false);

        if (InventoryManager.Instance.SplitAmount == InventoryManager.Instance.MaxStackCount)
        {
            MoveItem(InventoryManager.Instance.Clicked);
        }

        else if (InventoryManager.Instance.SplitAmount > 0)
        {
            InventoryManager.Instance.MovingSlot.Items = InventoryManager.Instance.Clicked.GetComponent<Slot>().RemoveItems(InventoryManager.Instance.SplitAmount);
            
            CreateHoverIcon();
        }
        InventoryManager.Instance.SplitAmount = 0;    //piti laittaa tää koska maxSctackCount on statickkinä niin tääkin jää muistiin
    }

    public void ChangeStackText(int i)  //varmistaa ettei mennä valittavien itemien stackin yli, nuoli nappulat stackki laatikossa
    {
        InventoryManager.Instance.SplitAmount += i;

        if (InventoryManager.Instance.SplitAmount < 0)
        {
            InventoryManager.Instance.SplitAmount = 0;
        }
        if (InventoryManager.Instance.SplitAmount > InventoryManager.Instance.MaxStackCount)
        {
            InventoryManager.Instance.SplitAmount = InventoryManager.Instance.MaxStackCount;
        }

        InventoryManager.Instance.stackText.text = InventoryManager.Instance.SplitAmount.ToString();
    }

    public void MergeStacks(Slot source, Slot destination)  //stackkää itemit päällekkäin, source on kädessä olevat itemit
    {
        int max = destination.CurrentItem.Item.MaxSize - destination.Items.Count;

        int count = source.Items.Count < max ? source.Items.Count : max;

        for (int i = 0; i < count; i++)
        {
            destination.AddItem(source.RemoveItem());
            InventoryManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = InventoryManager.Instance.MovingSlot.Items.Count.ToString();
        }
        if (source.Items.Count == 0)
        {
            source.ClearSlot();
            Destroy(GameObject.Find("Hover"));
        }
    }

    public void ActiveInventory(string Name, string Type)
    {
        GameObject playerName = gameObject.transform.Find("PlayerName").gameObject;        
        playerName.GetComponent<TextMeshProUGUI>().text = Name; //tämä kuntoon

        GameObject playerClass = gameObject.transform.Find("PlayerClass").gameObject;
        playerClass.GetComponent<TextMeshProUGUI>().text = Type;

        transform.parent.tag = "ActiveInventory";   //vittu jee, tää toimii, vielä vähän.. :D
        //GameObject parent = GetComponentInParent<GameObject>(); //ei toimi, antaa erroria
        //parent.tag = "ActiveInventory";
        //gameObject.tag = "ActiveInventory";
    }
    public void InActiveInventory(string Name, string Type)
    {
        GameObject playerName = gameObject.transform.Find("PlayerName").gameObject;
        playerName.GetComponent<TextMeshProUGUI>().text = Name;

        GameObject playerClass = gameObject.transform.Find("PlayerClass").gameObject;
        playerClass.GetComponent<TextMeshProUGUI>().text = Type;

        transform.parent.tag = "InActiveInventory";
        //gameObject.tag = "InActiveInventory";
    }
    public void NotInSquadInventory()
    {
        transform.parent.tag = "NotInSquadInventory";
    }
    /*
     So I've been following along these tutorials trying to implement a similar (but not the same) inventory system into my game. When you reached the "movingSlot" part, I realised... why not just use movingSlot for ALL item movements within the inventory?
     Instead of using from/to/tempSlots, just empty the slot you click on, and move the items into movingSlot?
     It would be way simpler and more robust imo, and you'd be able to do everything more easily- for example, if you're holding an item, and you click on a different item in your inventory,
     the item in your hand could go into the clicked slot, and the items in the clicked slot could swap into your hand. Also, you don't have to worry about clearing the slot after you've moved an item etc. etc.
Just a thought! :)
liam robertson
liam robertson
3 vuotta sitten
Yep! I tried changing everything to be based on movingSlot, and everything is so much easier to work with. 
Previously, MoveItem() was a big, ugly, confusing script. But now it's super short and simple, does the exact same thing, and makes a lot more sense. 
*/
}
