using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float speed = 5f;
    Vector3 movement;
    Rigidbody rb;
    private float smoothing = 5f;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float zoom = Input.GetAxis("Mouse ScrollWheel");    //antaa joko -1 tai 1
        Move(h, v);
        Zoom(zoom);
    }

    private void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;

        //transform.position = Vector3.Lerp(transform.position, movement, smoothing * Time.deltaTime);
        //gameObject.transform.position = movement;
        rb.MovePosition(transform.position + movement);
    }

    private void Zoom(float zoom)
    {
        //gameObject.transform.position = transform.forward + new Vector3(zoom, 0);

        Vector3 targetCamPos = gameObject.transform.position + transform.forward * zoom * 20;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
