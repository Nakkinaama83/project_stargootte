using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent nav;
    UnityEngine.AI.NavMeshObstacle obstacle;
    public bool isInSquad;
    public bool pelaaja;
    public bool groupTargeting;
    public bool aiRotation;
    public bool aiMovement;
    public bool collision;
    public bool collisionTarget;
    public bool aiming;
    public bool oneMind;

    private Vector3 hit;
    private GameObject squadLeader;
    public GameObject target;
    private Vector3 v;
    private Vector3 va;

    private IEnumerator coroutine;
    private IEnumerator moveCoroutine;
    private IEnumerator groupTarget;

    // Start is called before the first frame update
    void Start()
    {
        aiming = false;
        squadLeader = FindObjectOfType<PlayerChange>().CurrentPlayer;
        target = new GameObject(gameObject.name + " target");
        target.tag = "Target";
        //target.AddComponent<CapsuleCollider>();
        target.AddComponent<SphereCollider>();
        target.AddComponent<TargetCollider>();
        target.AddComponent<DontDestroy>();
        collision = target.GetComponent<TargetCollider>().collision;
        target.GetComponent<SphereCollider>().radius = .6f;
        //target.GetComponent<CapsuleCollider>().radius = .5f;
        //target.GetComponent<CapsuleCollider>().height = 2;
        //target.GetComponent<CapsuleCollider>().transform.position = new Vector3(0, 1, 0);
        target.GetComponent<SphereCollider>().isTrigger = true;
        target.AddComponent<Rigidbody>();
        target.GetComponent<Rigidbody>().freezeRotation = true;
        target.GetComponent<Rigidbody>().useGravity = false;
        target.tag = "Target";
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        obstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
        coroutine = AiTurning();
        moveCoroutine = AiMovement();
        groupTarget = GroupTarget();
        float angle = Random.Range(0.0f, Mathf.PI * 2); //laskee ympyrästä randomi luvun asteina
        v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)); //laskee koordinaatit x,y,z joista x ja z on randomeita
        //v *= 11;
        //v /= 10;  //hmm.. ei toimi jako

        //StartCoroutine(coroutine);
    }

   

    

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (pelaaja == false && aiming == false && aiRotation == false)
        {
            StartCoroutine(coroutine);
            aiRotation = true;
        }
        if (pelaaja == false && groupTargeting == true && aiming == true && oneMind == false)
        {
            StopAllCoroutines();
            nav.enabled = false;
            obstacle.enabled = true;
            aiRotation = false;
            aiMovement = false;
        }
        if (pelaaja == true && groupTargeting == false && oneMind == false)
        {          
            StopAllCoroutines();
            nav.enabled = false;
            obstacle.enabled = true;
            aiRotation = false;
            aiMovement = false;
        }
        if (isInSquad == true && oneMind == true)
        {
            StopCoroutine(moveCoroutine);
            nav.enabled = false;
            obstacle.enabled = true;
            aiMovement = false;
        }
        if (isInSquad == true && pelaaja == false && aiming == false && oneMind == false)
        {
           
            if (aiMovement == false)
            {
                squadLeader = FindObjectOfType<PlayerChange>().CurrentPlayer;
                obstacle.enabled = false;
                StartCoroutine(moveCoroutine);
                nav.enabled = true;
                aiMovement = true;
            }
            if (Vector3.Distance(target.transform.position, squadLeader.transform.position) > 5)
            {
                StopCoroutine(moveCoroutine);
                StartCoroutine(moveCoroutine);
                
            }
            
        }
        if (Vector3.Distance(gameObject.transform.position, target.transform.position) < 0.2)
        {
            nav.enabled = false;
            obstacle.enabled = true;
            //pysäytä animointi
        }
    }

    public IEnumerator AiMovement()
    {
        while (isInSquad == true && pelaaja == false)
        {
            obstacle.enabled = false;
            nav.enabled = true;
            squadLeader = FindObjectOfType<PlayerChange>().CurrentPlayer;
            float angle = Random.Range(0.0f, Mathf.PI * 2); //laskee ympyrästä randomi luvun asteina
            v = new Vector3(Mathf.Sin(angle) / 10, 0, Mathf.Cos(angle) / 10); //laskee koordinaatit x,y,z joista x ja z on randomeita
            va = v;
            

            for (int i = 0; i < 40; i++)
            {
                                                                
                va += v;
                target.transform.position = squadLeader.transform.position - va;  //target - va, allmost there. Välillä collision ei triggeröidy pois
                collision = target.GetComponent<TargetCollider>().collision;
                collisionTarget = target.GetComponent<TargetCollider>().collisionTarget;
                if (collision == true)
                {
                    //collision = false;
                    break;
                }
                else if (collisionTarget == true)
                {
                    yield return new WaitForSeconds(3);
                    angle = Random.Range(0.0f, Mathf.PI * 2); //laskee ympyrästä randomi luvun asteina
                    v = new Vector3(Mathf.Sin(angle) / 10, 0, Mathf.Cos(angle) / 10); //laskee koordinaatit x,y,z joista x ja z on randomeita
                    va = v;
                }
                yield return new WaitForEndOfFrame();
                                                   
            }
            target.GetComponent<TargetCollider>().collision = false;
            nav.SetDestination(target.transform.position);  //target = target.pos - v
            //animointi tähän
            float waitTime = Random.Range(10, 30);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator AiTurning() //pitäiskö tehdä ihan oma scripti ja aktivoida se?
    {
        while (pelaaja == false)
        {
            
            int waitTime = Random.Range(3, 7);
            int rotation = Random.Range(5, 15);
            int direction = Random.Range(-1, 1);
            direction *= 10;
            for (int i = 0; i < rotation; i++)
            {
                yield return new WaitForEndOfFrame();
                gameObject.transform.Rotate(0, direction, 0);
            }
            //gameObject.transform.Rotate(0, rotation, 0);    //for-loop?
            Debug.Log("AiTurning is running");
            yield return new WaitForSeconds(waitTime);
            
            
        }
    }

    public void NavPointClicked()
    {
        obstacle.enabled = false;
        nav.enabled = true;
        nav.SetDestination(target.transform.position);
        //animointi tähän
        Debug.Log("Siirretään ukkoa");
    }

    public void GroupNavPointClicked(Vector3 Hit)
    {
        hit = Hit;
        StartCoroutine(groupTarget);
    }
    private IEnumerator GroupTarget()
    {
        target.transform.position = hit;
        while (Vector3.Distance(gameObject.transform.position, target.transform.position) > 1)
        {
            obstacle.enabled = false;
            nav.enabled = true;
            //squadLeader = target;
            float angle = Random.Range(0.0f, Mathf.PI * 2); //laskee ympyrästä randomi luvun asteina
            v = new Vector3(Mathf.Sin(angle) / 10, 0, Mathf.Cos(angle) / 10); //laskee koordinaatit x,y,z joista x ja z on randomeita
            va = v;
            

            for (int i = 0; i < 40; i++)
            {

                va += v;
                target.transform.position = hit - va;  //target - va, allmost there. Välillä collision ei triggeröidy pois, hmm..pitäiskö laittaa itse targetti liikuttamaan itteensä
                collision = target.GetComponent<TargetCollider>().collision;
                collisionTarget = target.GetComponent<TargetCollider>().collisionTarget;
                if (collision == true)
                {
                    //collision = false;
                    break;
                }
                else if (collisionTarget == true)
                {
                    yield return new WaitForSeconds(3);
                    angle = Random.Range(0.0f, Mathf.PI * 2); //laskee ympyrästä randomi luvun asteina
                    v = new Vector3(Mathf.Sin(angle) / 10, 0, Mathf.Cos(angle) / 10); //laskee koordinaatit x,y,z joista x ja z on randomeita
                    va = v;
                }
                yield return new WaitForEndOfFrame();

            }
            target.GetComponent<TargetCollider>().collision = false;
            nav.SetDestination(target.transform.position);  //target = target.pos - v
            //animointi tähän
            StopCoroutine(groupTarget);
            //break;    //jostain syystä ajaa vielä kerran for-loopin mutta ei aseta kohdetta
        }
        StopCoroutine(groupTarget);
    }
}
