using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePlayerInCanvas : MonoBehaviour
{
    public Transform activePlayerInCanvas;  //kokeillaan jotain UI:ssa
    public float smoothing = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activePlayerInCanvas != null)
        {
            transform.position = Vector3.Lerp(transform.position, activePlayerInCanvas.position, smoothing * Time.deltaTime);
        }
    }
}
