using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollider : MonoBehaviour
{
    public bool collision;
    public bool collisionTarget;
    // Start is called before the first frame update
    void Start()
    {
        collision = false;
        collisionTarget = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Chest" || other.gameObject.tag == "Box")
        {
            collision = true;
        }
        else if (other.gameObject.tag == "Target")
        {
            collisionTarget = true;
        }
        
        else
        {
            collision = false;
            collisionTarget = false;
        }
    }






    private void OnTriggerExit(Collider other)
    {
        collision = false;
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Chest" || other.gameObject.tag == "Box" || other.gameObject.tag == "Target")
        {
            collision = false;
            collisionTarget = false;
        }
    }
}
