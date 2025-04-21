using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Enemy_Projectile : MonoBehaviour
{
    public float projectile_speed;
    public char required_attack;
    public bool active;
    public Vector3 deactivated_position;
    public float distance_travelled;
    public float distance_to_travel;

    public bool redirected;
    public float max_height;
    public Vector3 player_position;
    public Vector3 enemy_position;
    public float journey_time;
    public float elapsed_time;
    public float redirect_speed;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        deactivated_position = new Vector3(-30, 0, 0);
        transform.position = deactivated_position;
        distance_travelled = 0;
        distance_to_travel = 40f;

        max_height = 5f;
        redirected = false;
        elapsed_time = 0f;
        redirect_speed = 20f;
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (redirected)
        {
            Redirected_Movement();
            return;
        }

        if (distance_travelled >= distance_to_travel)
        {
            Deactivate_Projectile();
        }

        if (active == true)
        {
            Vector3 movement = new Vector3(projectile_speed, 0, 0);
            transform.Translate(movement);
            distance_travelled += projectile_speed;
        }

        else
        {
            transform.position = deactivated_position;
        }

    }

    public void Activate_Projectile(Transform enemy_transform)
    {
        active = true;
        transform.position = enemy_transform.position;
        transform.rotation = enemy_transform.rotation;

    }

    public void Deactivate_Projectile()
    {
        distance_travelled = 0;
        active = false;
        transform.position = deactivated_position;
        redirected = false;
        elapsed_time = 0f;
    }

    public void Redirected_Movement()
    {   
        // Increment elapsed time
        elapsed_time += Time.deltaTime;

        // Calculate normalized time (0 to 1)
        float t = Mathf.Clamp01(elapsed_time / journey_time);

        // Interpolate the X and Z positions (linear movement between points)
        Vector3 curr_pos = Vector3.Lerp(player_position, enemy_position, t);

        // Adjust the Y position to create a parabolic arc
        float parabolic_height = max_height * 4 * t * (1 - t);  // Creates a parabolic arc

        // Set the object's position (interpolating XZ, applying parabola on Y)
        transform.position = new Vector3(curr_pos.x, 
                                         parabolic_height + Mathf.Lerp(player_position.y, enemy_position.y, t), 
                                         curr_pos.z);

        // Stop the movement when the target is reached
        if (transform.position == enemy_position)
        {
            Deactivate_Projectile();  // Disable the script when the object reaches the target
        }
    }


}
