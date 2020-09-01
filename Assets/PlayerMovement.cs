using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 3f;
    public string gadget;
    Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100f;
    public bool gadgetIsActive = false;
    public bool pelaaja = false;
    public bool groupTargeting = false;
    private float timer;
    private float delay;
    public bool oneMind;
 

    void Awake()
	{        
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}

    private void Start()
    {
        
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gadget != string.Empty)
        {
            if (pelaaja == true || oneMind == true)
            {
                ActivateGadget(gadget);
            }
            
        }

    }

    void FixedUpdate()
	{
        

		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");


       
        if (pelaaja==true && Input.GetButton("Fire2"))
        {
            bool aiming = h==0f && v==0f;
            anim.SetBool("IsAiming", aiming);
            Turning();
            Move(h, v);
            //WalkingAnim(h, v);          
        }
        if (groupTargeting == true && Input.GetButton("Fire2"))
        {
            timer += Time.deltaTime;
            delay = Random.Range(1, 6);
            delay = delay / 4;
            if (timer >= delay)
            {
                bool aiming = h == 0f && v == 0f;
                anim.SetBool("IsAiming", aiming);
                Turning();
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            timer = Time.deltaTime;
        }
        if (h==0f && v==0f && !Input.GetButton("Fire2"))
        {
            anim.SetBool("IsAiming", false);
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsWalking", false);
        }
        
        else if (pelaaja==true && !Input.GetButton("Fire2"))
        {    
             MoveRotation(h, v);
             //RunningAnim(h, v);

        }
        //Move(h, v);
        //Turning ();
        //Animating (h, v, speed);
	}

    public void IsGadgetActive()
    {
        if (gadgetIsActive == true)
        {
            FindObjectOfType<PlayerChange>().GroupTargeting(gadgetIsActive);
        }
    }

    public void ActivateGadget(string gadget)
    {
        if (gadgetIsActive == false)
        {
            if (gadget == "Group Targeting")
            {
                gadgetIsActive = true;
                FindObjectOfType<PlayerChange>().GroupTargeting(gadgetIsActive);
                Debug.Log("You just activated " + gadget.ToString());
            }
            if (gadget == "One Mind")
            {
                gadgetIsActive = true;
                FindObjectOfType<PlayerChange>().OneMind(gadgetIsActive);
                Debug.Log("You just activated " + gadget.ToString());
            }
            
        }
        else if (gadgetIsActive == true)
        {
            if (gadget == "Group Targeting")
            {
                gadgetIsActive = false;
                FindObjectOfType<PlayerChange>().GroupTargeting(gadgetIsActive);
                Debug.Log("You just deactivated " + gadget.ToString());
            }
            if (gadget == "One Mind")
            {
                gadgetIsActive = false;
                FindObjectOfType<PlayerChange>().OneMind(gadgetIsActive);
                Debug.Log("You just deactivated " + gadget.ToString());
            }
           
        }
        
    }

    void MoveRotation(float h, float v)
    {
        speed = 4f;
        float axisCombined = h / v;
        Vector3 movement = new Vector3(h + 0.001f, 0.0f, v);

        playerRigidbody.AddForce(movement * speed, ForceMode.Acceleration);

        Quaternion newRotation = Quaternion.LookRotation(movement);

        anim.SetBool("IsWalking", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsRunning", true);
        


        if (axisCombined >= 0.1f || axisCombined <= 0.1f)
        {

            transform.rotation = Quaternion.Slerp
                (transform.rotation, newRotation, 20 * Time.deltaTime);

            movement.Set(h, 0f, v);

            movement = movement.normalized * speed * Time.deltaTime;

            playerRigidbody.MovePosition(transform.position + movement);

            

        }
    }

      

        
    
    void Move (float h, float v)
	{
        speed = 1f;
        movement.Set (h, 0f, v);

		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);
        anim.SetBool("IsRunning", false);
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);

    }

	void Turning ()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) 
		{
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			playerRigidbody.MoveRotation (newRotation);
		}
	}
    


    /*void WalkingAnim(float h, float v)
    {

        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

    void RunningAnim(float h, float v)
    {

        bool running = h != 0f || v != 0f;
        anim.SetBool("IsRunning", running);
    }
    */
    /*void Animating(float h, float v, float speed)
	{
        
        bool walking = speed <= 3f && h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);

        
        bool running = speed >= 3f && h != 0f || v != 0f;
        anim.SetBool("IsRunning", running);
        
	}
    */
}
	