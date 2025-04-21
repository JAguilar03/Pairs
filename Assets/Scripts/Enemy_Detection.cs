using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Detection : MonoBehaviour
{
    public float detection_range;

    void Start()
    {
        detection_range = 7.5f;
    }

    public bool Player_Detected(Transform player)
    {
        float distance_to_player = Vector3.Distance(transform.position, player.position);
        return distance_to_player <= detection_range;
    }
}
