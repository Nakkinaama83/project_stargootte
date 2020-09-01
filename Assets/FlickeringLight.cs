using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{

    bool activeLight = false;


    // Start is called before the first frame update
    void Start()
    {
      
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeLight == false)
        {
            //StartCoroutine(Example(timer: Random.Range(0.5f, 3f)));
            StartCoroutine(Example());
            activeLight = true;
        }
        
    }

    //IEnumerator Example(float timer)
    IEnumerator Example()
    {

        GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(Random.Range(0.1f, 3f));
        GetComponent<Light>().enabled = false;
        yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        activeLight = false;
    }

}

