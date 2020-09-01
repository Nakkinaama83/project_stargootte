using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float startingHealth = 100;
    public float currentHealth;
    private Slider healthSlider;
    //public Image damageImage;
    //public AudioClip deathClip;
    //public float flashSpeed = 5f;
    //public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    //public GameObject canvasGameObject; //tehdään player change koodissa
    //public Vector3 playerTextLocation; //tehdään player change koodissa
    //public Sprite someBgSprite;
    //public Sprite someFillSprite;
    //public Sprite someKnobSprite;
    public Slider healthBarPrefab;
    public string playerClass;
    public string PlayerName;
    


    Animator anim;
    //AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;

    

    public void ActivatePlayerClass(GameObject parent, Vector3 location)
    {
        playerClass = gameObject.name.ToString();
        GameObject playerclass = new GameObject("PlayerClass");
        playerclass.transform.SetParent(parent.transform);

        playerclass.AddComponent<Text>().text = playerClass;
        playerclass.GetComponent<Text>().fontSize = 15; // * (int)InventoryManager.Instance.canvas.scaleFactor
        playerclass.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        //playerclass.GetComponent<Text>().font = Resources.Load<Font>("Assets/TextMesh Pro/Examples & Extras/Fonts/Anton.ttf") as Font;
        playerclass.GetComponent<RectTransform>().anchoredPosition3D = location;

        //playerclass.AddComponent<TextMesh>().text = playerClass;
        //playerclass.AddComponent<RectTransform>();
        //playerclass.GetComponent<TextMesh>().fontSize = 15;
        //playerclass.GetComponent<TextMesh>().font = Resources.Load<Font>("TextMesh Pro/Fonts/Antton");
        //playerclass.GetComponent<TextMesh>().characterSize = 12;
        //playerclass.GetComponent<RectTransform>().anchoredPosition3D = location;
    }

    public void ActivatePlayerName(string textString, Vector3 location, GameObject parent)
    {
        GameObject playerName = new GameObject("Child"); //creating player name
        //gameObject.transform.SetParent(this.transform);
        playerName.transform.SetParent(parent.transform);
        //gameObject.transform.position = location;

        playerName.AddComponent<Text>().text = textString;
        playerName.GetComponent<Text>().fontSize = 15;  // * (int)InventoryManager.Instance.canvas.scaleFactor
        playerName.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        playerName.GetComponent<RectTransform>().anchoredPosition3D = location;
        PlayerName = textString;
    }
    
    
    public void ActivateHealthBar(GameObject canvasGameObject, Vector3 location)
    {
        /*
        //creating player health bar
        GameObject healthBar = new GameObject("Child");
        healthBar.transform.SetParent(canvasGameObject.transform);
        healthBar.transform.position = location;
        healthBar.AddComponent<Slider>().value = currentHealth;
        healthBar.AddComponent<Slider>().targetGraphic = Resources.GetBuiltinResource(typeof(Slider), "handle.ttf") as Graphic;
        //healthBar.AddComponent<Slider>().direction = Slider.Direction.LeftToRight;
        //healthBar.AddComponent<Slider>().minValue = 0;
        //healthBar.AddComponent<Slider>().maxValue = 100;
        */

        //Instantiate(healthSlider, new Vector3(0, 0, 0), Quaternion.identity);
        //Instantiate(healthSlider, location, Quaternion.identity);

        //Slider childObject = Instantiate(healthSlider) as Slider;
        //childObject.transform.parent = gameObject.transform;

        /*
        //tämä pelkällä koodilla
        DefaultControls.Resources uiResources = new DefaultControls.Resources();
        //Set the Slider Background Image someBgSprite;
        uiResources.background = someBgSprite;
        //Set the Slider Fill Image someFillSprite;
        uiResources.standard = someFillSprite;
        //Set the Slider Knob Image someKnobSprite;
        uiResources.knob = someKnobSprite;
        GameObject uiSlider = DefaultControls.CreateSlider(uiResources);
        uiSlider.transform.SetParent(canvasGameObject.transform, false);
        */

        
        //tämä prefabbeinä
        Slider uiToggle = Instantiate(healthBarPrefab) as Slider;
        uiToggle.transform.SetParent(canvasGameObject.transform, false);
        healthSlider = uiToggle;
        //Move to another position?
        //uiToggle.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(...,...,...);
        uiToggle.GetComponent<RectTransform>().anchoredPosition3D = location;
        //Re-scale?
        //uiToggle.GetComponent<RectTransform>().localScale = new Vector3(...,...,...);
        uiToggle.value = currentHealth;
        
    }
    
    /* //tää on turha?
    public static Text AddTextToCanvas(string textString, GameObject canvasGameObject, Vector3 location)
    {
        
        Text text = canvasGameObject.AddComponent<Text>();
        text.text = textString;

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.transform.position = location;
        text.font = ArialFont;
        text.material = ArialFont.material;

        return text;
        

        
        GameObject gameObject = new GameObject("Child");
        //gameObject.transform.SetParent(this.transform);
        gameObject.transform.SetParent(canvasGameObject.transform);
        gameObject.transform.position = location;

        gameObject.AddComponent<Text>().text = textString;
        gameObject.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        return text;
        
    }
    */

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        //playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
        //healthSlider = GameObject.Find("HealthBar").GetComponent<Slider>(); //luodaan player change koodissa
        //healthSlider.value = currentHealth; //-||-

        //canvasGameObject = GameObject.Find("Canvas"); //tehdään player change koodissa
        //playerTextLocation = new Vector3(-358, 190, 0); //tehdään player change koodissa
        //AddTextToCanvas(textString: "Hey", canvasGameObject: canvasGameObject, location: playerTextLocation); //tää on turha?
        //ActivatePlayerName(textString: "Alaric", canvasGameObject: canvasGameObject, location: playerTextLocation); // aktivoidaan player change koodissa
        //ActivateHealthBar(canvasGameObject: canvasGameObject, location: playerTextLocation);
    }


    void Update ()
    {
        if(damaged)
        {
            //damageImage.color = flashColour;
        }
        else
        {
            //damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    public void TakeDamage (float amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;
        

        //playerAudio.Play ();

        if (currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;
        //Destroy(playerMovement);
        //Destroy(playerShooting);
        playerMovement.enabled = false;
        playerShooting.enabled = false;
        playerShooting.DisableEffects ();

        anim.SetBool("IsAiming", false);
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsWalking", false);
        //anim.SetTrigger ("Die");
        anim.SetBool("Death", true);

        //playerAudio.clip = deathClip;
        //playerAudio.Play ();

        Rigidbody player_rigidbody = GetComponent<Rigidbody>();
        player_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    public void HealthKit()
    {
        healthSlider.value = currentHealth;
    }

    /*public void RestartLevel ()
    {
        Application.LoadLevel (Application.loadedLevel);
    }*/
}
