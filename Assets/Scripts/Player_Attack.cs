using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public Vector2 size = new Vector2(20f, 20f); 
    public Vector2 offset = new Vector2(20f, 0f);
    public Move_Enemy_Projectile enemy_projectile_script;
    public BoxCollider2D attack_hitbox;
    public Animator animator;
    public char input;
    public int attacking;
    public Transform attack_circle;
    public SpriteRenderer circle_renderer;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "player_attack";
        Transform parent = transform.parent;
        attack_circle = parent.Find("Circle");
        attacking = 0;
        attack_circle.gameObject.SetActive(false);
        circle_renderer = attack_circle.GetComponent<SpriteRenderer>();
        // get animator component from parent
        animator = GetComponentInParent<Animator>();

        // attach collider to child object
        attack_hitbox = gameObject.AddComponent<BoxCollider2D>();

        // Set the size and offset of the collider
        attack_hitbox.size = new Vector2(20f, 20f);
        attack_hitbox.offset = new Vector2(20f, 0f);
        

        attack_hitbox.enabled = false;

        input = 'o';


    }

    // Update is called once per frame
    public void Generate_Attack_Collider(char player_input)
    {
        switch (player_input)
        {
            case 'i':
                circle_renderer.color = new Color(0f, 0f, 1f, 0.5f);
                attack_circle.gameObject.SetActive(true);
                animator.SetBool("attack2", true);
                break;

            case 'o':
                circle_renderer.color = new Color(0f, 1f, 0f, 0.5f);
                attack_circle.gameObject.SetActive(true);
                animator.SetBool("attack2", true);
                break;

            default:
                circle_renderer.color = new Color(1f, 0f, 0f, 0.5f);
                attack_circle.gameObject.SetActive(true);
                animator.SetBool("attack2", true);
                break;
        }

        input = player_input;
        attack_hitbox.enabled = true;

        Invoke("Remove_Attack_Collider", 0.1f);

    }

    public void Remove_Attack_Collider()
    {
        attacking = 0;
        switch (input)
        {
            case 'i':
                attack_circle.gameObject.SetActive(false);
                animator.SetBool("attack2", false);
                break;

            case 'o':
                attack_circle.gameObject.SetActive(false);
                animator.SetBool("attack2", false);
                break;

            case 'p':
                attack_circle.gameObject.SetActive(false);
                animator.SetBool("attack2", false);
                break;

            default:
                break;
        }


        attack_hitbox.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        enemy_projectile_script = other.gameObject.GetComponent<Move_Enemy_Projectile>();
        
        Transform player_transform = transform.parent;
        Transform parent_transform = other.transform.parent;
        Enemy_Behavior enemy_script = parent_transform.GetComponent<Enemy_Behavior>();
        if (input == enemy_projectile_script.required_attack )
        {   
            enemy_script.damage_count += 1;
            enemy_projectile_script.redirected = true;
            enemy_projectile_script.player_position = player_transform.position;
            enemy_projectile_script.enemy_position  = parent_transform.position;
            enemy_projectile_script.journey_time = Vector3.Distance(player_transform.position, parent_transform.position) / enemy_projectile_script.redirect_speed;
        }

        else 
        {   
            enemy_script.damage_count = 0;
            GameObject parent_object = parent_transform.gameObject;
            Enemy_Attack enemy_attack_script = parent_object.GetComponent<Enemy_Attack>();
            enemy_attack_script.Sequence_Fail();
            Debug.Log("Player Took Damage");
        }
    }
}
