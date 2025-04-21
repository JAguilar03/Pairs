using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float direction;
    public float movement_speed;
    public float distance_travelled;
    public float distance_to_travel;
    public float rotation;


    // Start is called before the first frame update
    void Start()
    {
        movement_speed = 0.05f;
        direction = 1f;
        distance_travelled = 0f;
        distance_to_travel = 5f;
        rotation = 0f;

    }

    // Update is called once per frame
    public void Move_Enemy()
    {
        if (distance_travelled >= distance_to_travel)
        {
            distance_travelled = 0;
            rotation += 180;
            transform.eulerAngles = new Vector3(0, rotation,0);
        }

        Vector3 movement = new Vector3(movement_speed, 0, 0);
        transform.Translate(movement);
        distance_travelled += movement_speed;
    }

    public void Look_At_Player(float player_position_x)
    {
        float enemy_position_x = transform.position.x;
        if (player_position_x < enemy_position_x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
