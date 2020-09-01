using System.Collections;
using System.Collections.Generic;
//using System.Collections.Generic.IEnumerable;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;  //käytä takewhile ja skipwhilen kanssa

public class PlayerChange : MonoBehaviour
{
    public List<GameObject> soldiers;
    public List<GameObject> scientists;
    public List<GameObject> techs;
    public List<GameObject> players;
    public List<GameObject> reserve;
    private List<string> playerNames;
    private List<GameObject> playerInfoParents;
    public GameObject CurrentPlayer;   //käytä takewhile ja skipwhilen kanssa
    public GameObject NextPlayer; //käytä takewhile ja skipwhilen kanssa
    private Vector3 AddSquadButtonLocation; //pikku nappulan sijainti jota muutetaan koodissa
    public Transform PlayerCamera;
    public Button AddSquadMemberButton; //pitää olla public, lisätään koodissa kuuntelija, pikkunappula
    public Transform AddSquadMember; // lisää hahmo squadiin, pikkunappulan sijainti
    public Button RemSquadMember; //poista hahmo squadista, kesken pikkunappula
    public GameObject canvasGameObject;
    public Vector3 playerTextLocation;
    public Vector3 playerHealthBarLocation;
    public Vector3 playerClassLocation;
    public Vector3 playerWeaponNameLocation;
    private Vector3 RemSquadButtonLocation;
    private Vector3 CharacterParentLocation;
    public Button ReserveButton; // lisää hahmon reservistä, pitää olla public
    private Vector3 MoveReserveButtonLocation; //lisää hahmon reservistä, sijainti jota siirretään koodissa
    private string playerClass;
    private bool oneMindIsActive;
    //public Transform ActivePlayerInCanvas;  //kokeillaan jotain UI:ssa, siiretäänkö itse paneeliin tää?

    //private int playerNo = 0;

    //public List<GameObject> players = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(this);  //säilyttää koko parentin kaikkineen, se on liikaa. Eiku just hyvä
        //FindObjectOfType<CanvasHandler>().scaleCanvas();  //ei käytössä, poista
        //players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        players = new List<GameObject>();
        reserve = new List<GameObject>(GameObject.FindGameObjectsWithTag("Reserve"));
        techs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Tech"));
        scientists = new List<GameObject>(GameObject.FindGameObjectsWithTag("Scientist"));
        soldiers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Soldier"));
        playerNames = new List<string> { "Alaric", "PuuperttiRuma", "Brujah", "TRansu", "Olli", "Corvus", "Kekkonen", "Zaego", "Donald Dumb" };
        playerInfoParents = new List<GameObject>();
        
        //CurrentPlayer = players[0];    //käytä takewhile ja skipwhilen kanssa
        //turha poista//NextPlayer = NextPlayer = players.SkipWhile(x => x != CurrentPlayer).Skip(1).DefaultIfEmpty(players[0]).FirstOrDefault();  //käytä takewhile ja skipwhilen kanssa
        //PlayerCamera = players[0].gameObject.transform;
        
        

        canvasGameObject = GameObject.Find("Canvas");
        playerTextLocation = new Vector3(0, 0, 0);
        playerHealthBarLocation = new Vector3(0, -10, 0);
        playerClassLocation = new Vector3(0, -20, 0);
        playerWeaponNameLocation = new Vector3(0, -50, 0);
        //AddSquadButtonLocation = AddSquadMember.transform.position;
        AddSquadButtonLocation = new Vector3(-350, 190, 0);
        AddSquadMemberButton.onClick.AddListener(SquadMemberAddButton);
        RemSquadButtonLocation = new Vector3(0, -45, 0);
        CharacterParentLocation = new Vector3(-358, 190, 0);
        //MoveReserveButtonLocation = AddSquadButtonLocation + new Vector3(100, 0, 0);


        //AddTextToCanvas(textString: "Hey", canvasGameObject: canvasGameObject, location: playerTextLocation);

        foreach (GameObject c in techs) //eka pelaaja, players[0]
        {
            c.name = "Tech";
            reserve.Add(c);
            //techs.Remove(c);
        }

        foreach (GameObject d in soldiers)
        {
            d.name = "Soldier";
            reserve.Add(d);
            //soldiers.Remove(d);
        }

        foreach (GameObject e in scientists)    //viimeinen listassa
        {
            e.name = "Scientist";
            reserve.Add(e);
            //scientists.Remove(e);
        }

        players.Add(reserve[0]);  //iiiiso ongelma tän kanssa!!
        reserve.Remove(reserve[0]); //tää korjas iiiison ongelman, thymä minä
        CurrentPlayer = players[0];    //käytä takewhile ja skipwhilen kanssa
        //NextPlayer = players.SkipWhile(x => x != CurrentPlayer).Skip(1).DefaultIfEmpty(players[0]).FirstOrDefault();  //käytä takewhile ja skipwhilen kanssa
        PlayerCamera = players[0].gameObject.transform;
        //PlayerCamera = CurrentPlayer.gameObject.transform;

        foreach (GameObject i in players)
        {
            if (players.Count == 1)
            {
                i.tag = "Player";
                //aktivoidaan pelaajan nimi ja hiparipalkki
                string playerName = playerNames[Random.Range(0, playerNames.Count)];
                playerNames.Remove(playerName);
                //Vector3 newTextLocation = new Vector3(50, 0, 0);
                Vector3 newAddSquadButtonLocation = new Vector3(50, 0, 0);
                Vector3 newCharacterParentLocation = new Vector3(50, 0, 0);
                //GameObject characterParent = new GameObject("CharacterParent");
                GameObject characterParent = new GameObject(playerName);
                characterParent.transform.SetParent(canvasGameObject.transform, false);
                characterParent.AddComponent<RectTransform>();
                characterParent.GetComponent<RectTransform>().anchoredPosition3D = CharacterParentLocation;
                playerInfoParents.Add(characterParent);
                //ActivePlayerInCanvas = characterParent.transform;   //Tässä kokeillaan sitä juttua UI:ssa
                i.gameObject.GetComponent<PlayerHealth>().ActivatePlayerClass(parent: characterParent, location: playerClassLocation);
                i.gameObject.GetComponent<PlayerHealth>().ActivatePlayerName(textString: playerName, location: playerTextLocation, parent: characterParent);
                i.gameObject.GetComponent<PlayerHealth>().ActivateHealthBar(canvasGameObject: characterParent, location: playerHealthBarLocation);
                i.gameObject.GetComponentInChildren<PlayerShooting>().ActivateWeaponName(canvasGameObject: characterParent, location: playerWeaponNameLocation);
                AddSquadButtonLocation = AddSquadButtonLocation + newAddSquadButtonLocation;
                CharacterParentLocation = CharacterParentLocation + newCharacterParentLocation;
                //playerTextLocation = playerTextLocation + newTextLocation;
                //playerHealthBarLocation = playerHealthBarLocation + newTextLocation;
                //i.gameObject.GetComponent<PlayerHealth>().ActivateHealthBar();

                Button removeFromSquad = Instantiate(RemSquadMember) as Button;
                removeFromSquad.transform.SetParent(characterParent.transform, false);
                removeFromSquad.GetComponent<RectTransform>().anchoredPosition3D = RemSquadButtonLocation;
                removeFromSquad.onClick.AddListener(delegate { SquadMemberRemoveButton(i, characterParent);  });    //Destroy(removeFromSquad.gameObject);

                //players[0].gameObject.GetComponent<PlayerMovement>().enabled = true;
                //players[0].gameObject.GetComponentInChildren<PlayerShooting>().enabled = true;
                players[0].gameObject.GetComponent<PlayerMovement>().pelaaja = true;
                players[0].gameObject.GetComponentInChildren<PlayerShooting>().pelaaja = true;
                players[0].gameObject.GetComponent<AiController>().pelaaja = true;
                players[0].gameObject.GetComponent<AiController>().isInSquad = true;
                players[0].gameObject.GetComponent<Interact>().PlayerActive();
                players[0].gameObject.GetComponent<Interact>().PlayerActiveInCanvas(charParent: characterParent);   //kokeillaan taas jotain tyhmää UI:ssa
                FindObjectOfType<InventoryMover>().SquadUpdate();  //voikohan tänki jo poistaa
            }
            else
            {
                //varmistetaan ettei ohjaus-scriptit ole aktiivisia
                Debug.Log(players.Count);
                //i.gameObject.GetComponent<PlayerMovement>().enabled = false;  //tässä kokeillaan uutta ohjausta groupTargetingille
                //players[0].gameObject.GetComponent<PlayerMovement>().enabled = true;
                //i.gameObject.GetComponentInChildren<PlayerShooting>().enabled = false;
                //players[0].gameObject.GetComponentInChildren<PlayerShooting>().enabled = true;
                players[0].gameObject.GetComponent<PlayerMovement>().pelaaja = true;
                players[0].gameObject.GetComponentInChildren<PlayerShooting>().pelaaja = true;
                players[0].gameObject.GetComponent<AiController>().pelaaja = true;
                players[0].gameObject.GetComponent<AiController>().isInSquad = true;

                //i.gameObject.GetComponent<Interact>().enabled = false;    //tätä ei tarvita poista

                i.gameObject.GetComponent<Interact>().PlayerNonActive();
                //players[0].gameObject.GetComponent<Interact>().enabled = true;
                players[0].gameObject.GetComponent<Interact>().PlayerActive();



                i.tag = "Player";
                //aktivoidaan pelaajien nimet ja hiparipalkit
                string playerName = playerNames[Random.Range(0, playerNames.Count)];
                playerNames.Remove(playerName);
                //playerNo = playerNo +1;
                //Vector3 newTextLocation = new Vector3(50, 0, 0);
                Vector3 newAddSquadButtonLocation = new Vector3(50, 0 , 0);
                Vector3 newCharacterParentLocation = new Vector3(50, 0, 0);
                GameObject characterParent = new GameObject(playerName);
                characterParent.transform.SetParent(canvasGameObject.transform, false);
                characterParent.AddComponent<RectTransform>();
                characterParent.GetComponent<RectTransform>().anchoredPosition3D = CharacterParentLocation;
                playerInfoParents.Add(characterParent);
                //i.gameObject.GetComponent<PlayerHealth>().ActivatePlayerName(textString: playerName, canvasGameObject: canvasGameObject, location: playerTextLocation, parent: characterParent);
                i.gameObject.GetComponent<PlayerHealth>().ActivatePlayerClass(parent: characterParent, location: playerClassLocation);
                i.gameObject.GetComponent<PlayerHealth>().ActivatePlayerName(textString: playerName, location: playerTextLocation, parent: characterParent);
                i.gameObject.GetComponent<PlayerHealth>().ActivateHealthBar(canvasGameObject: characterParent, location: playerHealthBarLocation);
                i.gameObject.GetComponentInChildren<PlayerShooting>().ActivateWeaponName(canvasGameObject: characterParent, location: playerWeaponNameLocation);
                //AddSquadMember.transform.position = AddSquadButtonLocation;
                AddSquadButtonLocation = AddSquadButtonLocation + newAddSquadButtonLocation;
                AddSquadMember.GetComponent<RectTransform>().anchoredPosition3D = AddSquadButtonLocation;
                //AddSquadButtonLocation = AddSquadButtonLocation + newAddSquadButtonLocation;
                CharacterParentLocation = CharacterParentLocation + newCharacterParentLocation;

                Button removeFromSquad = Instantiate(RemSquadMember) as Button;
                removeFromSquad.transform.SetParent(characterParent.transform, false);
                removeFromSquad.GetComponent<RectTransform>().anchoredPosition3D = RemSquadButtonLocation;
                removeFromSquad.onClick.AddListener(delegate { SquadMemberRemoveButton(i, characterParent);  });    //Destroy(removeFromSquad.gameObject);
                //RemSquadButtonLocation = RemSquadButtonLocation + newTextLocation;

                //playerTextLocation = playerTextLocation + newTextLocation;
                //playerHealthBarLocation = playerHealthBarLocation + newTextLocation;
                //playerWeaponNameLocation = playerWeaponNameLocation + newTextLocation;
                
                FindObjectOfType<InventoryMover>().SquadUpdate();  //voikohan jo poistaa?
            }
        
        }
        
    }
    //private GameObject NextOf = players[0];


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("tab") && oneMindIsActive == false)
        {
            
            if (players.Count == 1)
            {
                Debug.Log("No next player");
                //return;
            }
            else
            {
                GroupTargeting(false);  //kytketään grouptargeting pois päältä mutta jätetään se hahmolla päälle jotta se aktivoituu autom
                //CurrentPlayer.gameObject.GetComponent<PlayerMovement>().enabled = false;
                //CurrentPlayer.gameObject.GetComponentInChildren<PlayerShooting>().enabled = false;
                CurrentPlayer.gameObject.GetComponent<PlayerMovement>().pelaaja = false;
                CurrentPlayer.gameObject.GetComponentInChildren<PlayerShooting>().pelaaja = false;
                CurrentPlayer.gameObject.GetComponent<Interact>().PlayerNonActive();
                CurrentPlayer.gameObject.GetComponent<AiController>().pelaaja = false;
                CurrentPlayer.gameObject.tag = CurrentPlayer.name.ToString();

                //NextPlayer = players.SkipWhile(x => x != CurrentPlayer).Skip(1).DefaultIfEmpty(players[0]).FirstOrDefault();  //tehdään tällä hetkellä fixedupdatessa, poista sieltä myöhemmin ja aktivoi tämä

                //NextPlayer.gameObject.GetComponent<PlayerMovement>().enabled = true;
                //NextPlayer.gameObject.GetComponentInChildren<PlayerShooting>().enabled = true;
                NextPlayer.gameObject.GetComponent<PlayerMovement>().pelaaja = true;
                NextPlayer.gameObject.GetComponentInChildren<PlayerShooting>().pelaaja = true;
                NextPlayer.gameObject.GetComponent<Interact>().PlayerActive();
                NextPlayer.gameObject.GetComponent<AiController>().pelaaja = true;
                NextPlayer.gameObject.tag = "Player";

                CurrentPlayer = NextPlayer;
                CurrentPlayer.gameObject.GetComponent<PlayerMovement>().IsGadgetActive();  //tsekataan onko gadgetti aktiivinen ja käytetöön sitä
                PlayerCamera = CurrentPlayer.gameObject.transform;
            }
        }  
    }

    public void SquadMemberAddButton()
    {
        if (oneMindIsActive == false)
        {
            GameObject ButtonParent = new GameObject("ButtonParent");
            ButtonParent.transform.SetParent(canvasGameObject.transform, false);
            MoveReserveButtonLocation = AddSquadButtonLocation + new Vector3(0, 0, 0);

            //Debug.Log("List players in reserve");
            if (reserve.Any())
            {
                foreach (GameObject a in reserve)
                {

                    playerClass = a.GetComponent<Interact>().playerClass;//FindObjectOfType<PlayerChange>().PlayerCamera;
                    Vector3 newSquadButton = new Vector3(0, -15, 0);
                    Button freePlayerName = Instantiate(ReserveButton) as Button; // laita muualle, vaikka pikkunappulan kuuntelijan alle
                    freePlayerName.transform.SetParent(ButtonParent.transform, false);
                    freePlayerName.GetComponentInChildren<Text>().text = playerClass;
                    //freePlayerName.GetComponent<RectTransform>().anchoredPosition3D = SquadButton;
                    //freePlayerName.transform.position = MoveReserveDropdownLocation;
                    freePlayerName.GetComponent<RectTransform>().anchoredPosition3D = MoveReserveButtonLocation;

                    freePlayerName.onClick.AddListener(delegate { SquadMemberAdd(a); Destroy(ButtonParent.gameObject); });
                    //freePlayerName.onClick.AddListener(delegate { SquadMemberAdd(a); freePlayerName.onClick.RemoveAllListeners(); Destroy(freePlayerName.gameObject); });
                    MoveReserveButtonLocation = MoveReserveButtonLocation + newSquadButton;
                }
            }
        }
        else
        {
            Debug.Log("One Mind is active an can't add squad members right now");
        }
        
    }
    public void SquadMemberAdd(GameObject player) // tää on isolle nappulalle
    {
        //player.tag = "Player";
        Debug.Log("Add Squad Member");
        players.Add(player);
        reserve.Remove(player);
        //ActivePlayersUpdate();
        string playerName = playerNames[Random.Range(0, playerNames.Count)];
        playerNames.Remove(playerName);
        Vector3 newTextLocation = new Vector3(50, 0, 0);
        //Vector3 addSquadButtonLocation = AddSquadButtonLocation + new Vector3(50, 0, 0);
        Vector3 newAddSquadButtonLocation = new Vector3(50, 0, 0);
        Vector3 newCharacterParentLocation = new Vector3(50, 0, 0);
        GameObject characterParent = new GameObject(playerName);
        characterParent.transform.SetParent(canvasGameObject.transform, false);
        characterParent.AddComponent<RectTransform>();
        characterParent.GetComponent<RectTransform>().anchoredPosition3D = CharacterParentLocation;
        playerInfoParents.Add(characterParent);
        player.gameObject.GetComponent<PlayerHealth>().ActivatePlayerClass(parent: characterParent, location: playerClassLocation);
        player.gameObject.GetComponent<PlayerHealth>().ActivatePlayerName(textString: playerName, location: playerTextLocation, parent: characterParent);
        player.gameObject.GetComponent<PlayerHealth>().ActivateHealthBar(canvasGameObject: characterParent, location: playerHealthBarLocation);
        player.gameObject.GetComponentInChildren<PlayerShooting>().ActivateWeaponName(canvasGameObject: characterParent, location: playerWeaponNameLocation);
        player.gameObject.GetComponent<Interact>().PlayerActiveInCanvas(charParent: characterParent);   //kokeillaan siirtää paneelia
        //AddSquadMember.transform.position = addSquadButtonLocation;
        AddSquadButtonLocation = AddSquadButtonLocation + newAddSquadButtonLocation;
        AddSquadMember.GetComponent<RectTransform>().anchoredPosition3D = AddSquadButtonLocation;
        //AddSquadButtonLocation = AddSquadButtonLocation + newAddSquadButtonLocation;
        CharacterParentLocation = CharacterParentLocation + newCharacterParentLocation;

        Button removeFromSquad = Instantiate(RemSquadMember) as Button;
        removeFromSquad.transform.SetParent(characterParent.transform, false);
        removeFromSquad.GetComponent<RectTransform>().anchoredPosition3D = RemSquadButtonLocation;
        removeFromSquad.onClick.AddListener(delegate { SquadMemberRemoveButton(player, characterParent); });   //Destroy(removeFromSquad.gameObject);
        //RemSquadButtonLocation = RemSquadButtonLocation + newTextLocation;
        FindObjectOfType<InventoryMover>().SquadUpdate();

        player.gameObject.GetComponent<PlayerInventory>().isInSquad = true; //kokeilaan saada inventaario näkymään
        player.gameObject.GetComponent<PlayerInventory>().IsInSquad();  //-||-
        player.gameObject.GetComponent<AiController>().isInSquad = true;

        //playerTextLocation = playerTextLocation + newTextLocation;
        //playerHealthBarLocation = playerHealthBarLocation + newTextLocation;
        //playerWeaponNameLocation = playerWeaponNameLocation + newTextLocation;
    }
    public void SquadMemberRemoveButton(GameObject player, GameObject characterParent)
    {
        if (player == CurrentPlayer)
        {
            Debug.Log("Can't remove active player");
        }
        else
        {
            //player.tag = "Reserve";
            Vector3 newAddLocation = new Vector3(-50, 0, 0);
            AddSquadButtonLocation = AddSquadButtonLocation + newAddLocation;
            Debug.Log("Remove Squad Member");
            reserve.Add(player);
            players.Remove(player);
            playerInfoParents.Remove(characterParent);
            Destroy(characterParent);
            AddSquadMember.GetComponent<RectTransform>().anchoredPosition3D = AddSquadButtonLocation;
            ReArrangeUI();
            player.gameObject.GetComponent<PlayerInventory>().isInSquad = false; //kokeilaan saada inventaario näkymään
            player.gameObject.GetComponent<PlayerInventory>().IsInSquad();  //-||-
            FindObjectOfType<InventoryMover>().SquadUpdate();  //Tänki voi varmaan poistaa
            player.gameObject.GetComponent<AiController>().isInSquad = false;
        }
    }
    public void ReArrangeUI()
    {
        CharacterParentLocation = new Vector3(-358, 190, 0);
        foreach(GameObject b in playerInfoParents)
        {
            Vector3 newLocation = new Vector3(50, 0, 0);
            b.GetComponent<RectTransform>().anchoredPosition3D = CharacterParentLocation;
            CharacterParentLocation = CharacterParentLocation + newLocation;
        }
    }
    private void FixedUpdate()
    {
        NextPlayer = players.SkipWhile(x => x != CurrentPlayer).Skip(1).DefaultIfEmpty(players[0]).FirstOrDefault();
    }

    public void GroupTargeting(bool active)
    {
        if (active && players.Count > 0)
        {
            foreach (GameObject s in players)
            {
                s.gameObject.GetComponentInChildren<PlayerShooting>().groupTargeting = true;
                s.gameObject.GetComponent<PlayerMovement>().groupTargeting = true;
                s.gameObject.GetComponent<AiController>().groupTargeting = true;
            }
            CurrentPlayer.GetComponentInChildren<PlayerShooting>().groupTargeting = false;
            CurrentPlayer.GetComponent<PlayerMovement>().groupTargeting = false;
            CurrentPlayer.GetComponent<AiController>().groupTargeting = false;
        }
        if (!active && players.Count > 0)
        {
            foreach (GameObject s in players)
            {
                s.gameObject.GetComponentInChildren<PlayerShooting>().groupTargeting = false;
                s.gameObject.GetComponent<PlayerMovement>().groupTargeting = false;
                s.gameObject.GetComponent<AiController>().groupTargeting = false;
            }
            CurrentPlayer.GetComponentInChildren<PlayerShooting>().pelaaja = true;
            CurrentPlayer.GetComponent<PlayerMovement>().pelaaja = true;
            CurrentPlayer.GetComponent<AiController>().pelaaja = true;
        }
        else
        {
            Debug.Log("No one in this group");
        }        
    }

    public void OneMind(bool active)
    {
        if (active)
        {
            oneMindIsActive = true;
            foreach (GameObject s in players)
            {
                
                s.gameObject.GetComponentInChildren<PlayerShooting>().pelaaja = false;
                s.gameObject.GetComponent<PlayerMovement>().pelaaja = false;
                s.gameObject.GetComponent<PlayerMovement>().oneMind = true;
                s.gameObject.GetComponent<AiController>().pelaaja = false;
                s.gameObject.GetComponent<AiController>().oneMind = true; //tee jotain :D
            }
            FindObjectOfType<StrageticController>().enabled = true; //pass control to strageticController
            FindObjectOfType<CameraFollow>().enabled = false;
            FindObjectOfType<CameraControl>().enabled = true;
            Debug.Log("No fail!!!");
        }
        if (!active)
        {
            oneMindIsActive = false;
            foreach (GameObject s in players)
            {               
                s.gameObject.GetComponent<AiController>().oneMind = false; //tee jotain :D
                s.gameObject.GetComponent<PlayerMovement>().oneMind = false;
                s.gameObject.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            CurrentPlayer.GetComponentInChildren<PlayerShooting>().pelaaja = true;
            CurrentPlayer.GetComponent<PlayerMovement>().pelaaja = true;
            CurrentPlayer.GetComponent<AiController>().pelaaja = true;
            FindObjectOfType<StrageticController>().enabled = false; //disable strageticController
            FindObjectOfType<CameraControl>().enabled = false;
            FindObjectOfType<CameraFollow>().enabled = true;    //aktivoidaan seuranta
            Debug.Log("No fail!!!");
        }
        else
        {
            Debug.Log("Fail!!! No one in this group");
        }
    }
    public void NewScene()
    {
        foreach (var v in players)
        {
            FindObjectOfType<GameManager>().squad.Add(v);
            DontDestroyOnLoad(v);
        }
    }
}

