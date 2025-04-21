using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behavior : MonoBehaviour
{
    public Enemy_Detection detection_script;
    public Enemy_Movement movement_script;
    public Enemy_Attack attack_script;
    public Transform player;
    public int damage_count;
    public Vector3 deactivated_position;
    public bool moving;
    public bool attacking;
    public bool active;

    public Score_Counter score_script;

    // Start is called before the first frame update
    void Start()
    {

        GameObject score_counter = GameObject.Find("ScoreCounter");
        score_script = score_counter.GetComponent<Score_Counter>();
        detection_script = GetComponent<Enemy_Detection>();
        movement_script = GetComponent<Enemy_Movement>();
        attack_script = GetComponent<Enemy_Attack>();
        moving = true;
        active = true;
        deactivated_position = new Vector3(-30, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }

        bool player_detected = detection_script.Player_Detected(player);

        if (player_detected == true && attacking == false)
        {
            //stop, look at player, attack
            moving = false;
            movement_script.Look_At_Player(player.position.x);
            Debug.Log("player detected");
            attacking = true;

            attack_script.Display_Sequence();
            Invoke("Call_Attacks", 1.0f);
        }

        if (moving)
        {
            movement_script.Move_Enemy();
        }

        if (attack_script.attack_performed == true && attack_script.Check_Active() == false)
        {
            attacking = false;
            moving = true;
            attack_script.attack_performed = false;
        }

        if (damage_count >= 3 && !attack_script.Check_Active())
        {
            Deactivate_Enemy();
        }
        

    }

    private void Deactivate_Enemy()
    {
        active = false;
        transform.position = deactivated_position;
        score_script.AddScore(100);
    }

    public void Spawn_Enemy(Transform player_transform)
    {
        active = true;
        damage_count = 0;
        attack_script.Generate_Sequence();
        transform.position = new Vector3 (player_transform.position.x + 7f, player_transform.position.y, player_transform.position.z);
    }

    private void Call_Attacks()
    {
        for (int i = 1; i <= attack_script.num_inputs_required; i++)
            {
                Debug.Log("Launching Attack #" + i);
                attack_script.Start_Attack(i);
            }
    }
}
