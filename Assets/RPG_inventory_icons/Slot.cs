using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Inventory inventory;

    public GameObject pelaaja;

    private Stack<ItemScript> items;

    public Stack<ItemScript> Items { get => items; set => items = value; }

    public Text stackTxt;

    public Sprite slotEmpty;
    public Sprite slotHighlight;

    public ItemType canContain; //tsekkaa voiko itemin equipata

    public bool IsEmpty
    {
        get { return items.Count == 0; }
    }

    public bool IsAvailable
    {
        get { return CurrentItem.Item.MaxSize > items.Count; }
    }

    public ItemScript CurrentItem
    {
        get { return items.Peek(); }
    }

    void Awake()
    {
        items = new Stack<ItemScript>();  //tämä oli aiemmin start-funktiossa
    }

    // Start is called before the first frame update
    void Start()    // oli start aiemmin
    {
        
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = stackTxt.GetComponent<RectTransform>();

        int txtScleFactor = (int)(slotRect.sizeDelta.x * 0.60);
        stackTxt.resizeTextMaxSize = txtScleFactor;
        stackTxt.resizeTextMinSize = txtScleFactor;

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(ItemScript item)
    {
        if (IsEmpty)    //tää lisättiin jotta ladatessa tavaroita inventoryyn niin slottien määräkin muuttuu
        {
            transform.parent.GetComponent<Inventory>().EmptySlots--;
        }

        items.Push(item);

        if (items.Count > 1)
        {
            stackTxt.text = items.Count.ToString();
        }

        ChangeSprite(item.spriteNeutral, item.spriteHighLighted);
    }

    public void AddItems(Stack<ItemScript> items) //tallentaa siirretyn stackin uuteen slottiin
    {
        this.items = new Stack<ItemScript>(items);

        stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighLighted);
    }

    private void ChangeSprite(Sprite neutral, Sprite highlight)
    {
        GetComponent<Image>().sprite = neutral;

        SpriteState st = new SpriteState();
        st.highlightedSprite = highlight;
        st.pressedSprite = neutral;

        GetComponent<Button>().spriteState = st;
    }

    private void UseItem()
    {
        if (!IsEmpty)
        {
            items.Peek().Use(pelaaja, this); //tässä syötetään pelaaja ja slotti ItemScriptille

            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            if (IsEmpty)
            {
                ChangeSprite(slotEmpty, slotHighlight);
                //inventory.EmptySlots++; //tää antaa välillä herjaa, tsekkaa muuttuuko oikeesti emptyslotit. muuten jees mutta consumeablesien kans ei toimi
            }
        }
    }

    public void ClearSlot()
    {
        items.Clear();
        ChangeSprite(slotEmpty, slotHighlight);
        stackTxt.text = string.Empty;
        if (transform.parent != null)
        {
            transform.parent.GetComponent<Inventory>().EmptySlots++;
        }
        
    }

    public Stack<ItemScript> RemoveItems(int amount)  //useempi itemi
    {
        Stack<ItemScript> tmp = new Stack<ItemScript>();

        for (int i = 0; i < amount; i++)
        {
            tmp.Push(items.Pop());
        }

        stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        return tmp;
    }

    public ItemScript RemoveItem()    //yksi itemi
    {
        ItemScript tmp;

        tmp = items.Pop();

        stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        return tmp;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover")) //tähän valikko mitä tehdään?
        {
            UseItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !IsEmpty && !GameObject.Find("Hover")) //tässä luodaan split-stack laatikko
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition, InventoryManager.Instance.canvas.worldCamera, out position);

            InventoryManager.Instance.selectStackSize.SetActive(true);  //Inventory.Instance.

            InventoryManager.Instance.selectStackSize.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(position);   //Inventory.Instance.

            InventoryManager.Instance.SetStackInfo(items.Count);   //Inventory.Instance.
        }
    }

    public static void SwapItems(Slot from, Slot to)    //tapahtuu molemmilla kerroilla, oikeella nappulalla ja manuaalisesti
    {
        ItemType movingType = from.CurrentItem.Item.ItemType;
        if (to != null && from != null)
        {
            
            if (to.canContain == ItemType.GENERIC || movingType == to.canContain)   //
            {
                Stack<ItemScript> tmpTo = new Stack<ItemScript>(to.Items);  //stores the items from the "to" slot, so we can do a swap
                to.AddItems(from.Items);    //stores the items in the "from" slot in to the "to" slot
                if (tmpTo.Count == 0)   //if the "to" slot is 0 then we don't need to move anything to the "from" slot
                {
                    to.transform.parent.GetComponent<Inventory>().EmptySlots--;
                    from.ClearSlot();   //clears the from slot
                }
                else
                {
                    from.AddItems(tmpTo);   //if the to-slot contains items then we need to move to the from-slot
                }
            }
            if (movingType == ItemType.WEAPON)  // && !CharacterPanel.Instance.WeaponSlot1.IsEmpty
            {
                CharacterPanel toParent = to.transform.parent.GetComponent<CharacterPanel>();
                CharacterPanel fromParent = from.transform.parent.GetComponent<CharacterPanel>();
                if (toParent != null)
                {
                    Debug.Log("You changed your weapons");
                    toParent.PushWeaponsToPlayerShooting();
                }
                if (fromParent != null)
                {
                    Debug.Log("You changed your weapons");
                    fromParent.PushWeaponsToPlayerShooting();
                }

            }
            if (movingType == ItemType.EQUIPMENT)
            {
                CharacterPanel toParent = to.transform.parent.GetComponent<CharacterPanel>();
                CharacterPanel fromParent = from.transform.parent.GetComponent<CharacterPanel>();
                if (toParent != null)
                {
                    Debug.Log("You changed your gadget");
                    toParent.PushGadgetToEquipGadget();
                }
                if (fromParent != null)
                {
                    Debug.Log("You changed your gadget");
                    fromParent.PushGadgetToEquipGadget();
                }
            }

        }
        
    }
}
