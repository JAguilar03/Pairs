using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;  // Reference to the player's transform
    public Vector3 offset = new Vector3(4, 1.5f, -10); 
    public float smoothSpeed = 0.125f; 
    public float curr_x;
    public float prev_x;

    void Start()
    {
        curr_x = player.transform.position.x;
        prev_x = player.transform.position.x;
    }

    void LateUpdate()
    {
        if (player != null)
        {   

            prev_x = curr_x;
            curr_x = player.transform.position.x;

            if (curr_x > prev_x)
            {
                offset = new Vector3(0, 1.5f, -10);
            }

            else if (curr_x < prev_x)
            {
                offset = new Vector3(0, 1.5f, -10);
            }

        
            // Desired position is the player's position plus an offset
            Vector3 desiredPosition = player.position + offset;

            // Smoothly move the camera from its current position to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            // Set the camera's position
            transform.position = smoothedPosition;
            

        }
    }
}
