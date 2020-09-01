using UnityEngine;
using System.Collections;

//namespace CompleteProject

    public class CameraFollow : MonoBehaviour
    {
        public Transform target;            // The position that that camera will be following.
        public float smoothing = 5f;        // The speed with which the camera will be following.


        Vector3 offset;                     // The initial offset from the target.


        void Start ()
        {
            //target = FindObjectOfType<PlayerChange>().PlayerCamera;
            target = GameObject.FindGameObjectWithTag("Soldier").transform; //tää on viimeisin
            
            // Calculate the initial offset.
            //offset = transform.position - target.position;
            offset = new Vector3(0, 6, -5);
        }


        void FixedUpdate ()
        {
            target = FindObjectOfType<PlayerChange>().PlayerCamera;
            // Create a postion the camera is aiming for based on the offset from the target.
            Vector3 targetCamPos = target.position + offset;

            // Smoothly interpolate between the camera's current position and it's target position.
            transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
