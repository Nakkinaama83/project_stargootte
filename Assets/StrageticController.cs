using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class StrageticController : MonoBehaviour//, IPointerDownHandler    //EventTrigger
{
    //public CameraFollow cameraFollow;
    //public CameraControl cameraControl;
    public GameObject selected;
    public List<GameObject> groupSelected;

    // Start is called before the first frame update
    void Start()
    {

        groupSelected = new List<GameObject>();
    }

    //public void onPointerDown(PointerEventData eventData)   //pitäisköhän käyttää raycastiä ennemmin??
    //{
    //    Debug.Log(this.gameObject.name + " Was Clicked.");
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //raycastinä
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player" || hit.collider.tag == "Tech" || hit.collider.tag == "Soldier" || hit.collider.tag == "Scientist")
                {
                    if (groupSelected.Contains(hit.collider.gameObject))
                    {
                        groupSelected.Remove(hit.collider.gameObject);
                        hit.collider.gameObject.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    }
                    else
                    {
                        groupSelected.Add(hit.collider.gameObject);
                        hit.collider.gameObject.transform.GetComponentInChildren<SpriteRenderer>().enabled = true;
                    }
                    
                    Debug.Log(hit.collider.name.ToString() + " Was clicked");
                    //hit.collider.gameObject now refers to the 
                    //cube under the mouse cursor if present
                }
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //raycastinä
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                if (hit.collider.tag == "Player" || hit.collider.tag == "Tech" || hit.collider.tag == "Soldier" || hit.collider.tag == "Scientist")
                {
                    if (selected != null)
                    {
                        selected.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    }
                    selected = hit.collider.gameObject;
                    selected.transform.GetComponentInChildren<SpriteRenderer>().enabled = true;
                    Debug.Log(hit.collider.name.ToString() + " Was clicked");
                    //hit.collider.gameObject now refers to the 
                    //cube under the mouse cursor if present
                }
                else if (selected != null)
                {
                    selected.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    selected = null;
                    foreach (var ring in groupSelected)
                    {
                        ring.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    }
                    groupSelected.Clear();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //raycastinä
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Floor Quad" || hit.collider.gameObject.name == "Floor")
                {
                    if (selected != null)
                    {
                        selected.GetComponent<AiController>().target.transform.position = hit.point;    //jee, toimii mut jos collideri on eessä niin osuu siihen
                        selected.GetComponent<AiController>().NavPointClicked();
                    }
                    if (groupSelected.Count > 0)
                    {
                        if (selected == null)
                        {
                            selected = groupSelected[0];
                        }
                        selected.GetComponent<AiController>().target.transform.position = hit.point;
                        selected.GetComponent<AiController>().NavPointClicked();
                        foreach (var a in groupSelected)
                        {
                            a.GetComponent<AiController>().GroupNavPointClicked(hit.point);
                        }
                    }
                    
                    Debug.Log(hit.collider.name.ToString() + " Was clicked");
                    //hit.collider.gameObject now refers to the 
                    //cube under the mouse cursor if present
                }
            }
        }
    }
}
