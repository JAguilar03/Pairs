using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{
    public float speed = 0.07f;
    public float jump_force = 0.2f; 
    public float fall_speed = -0.1f;
    public bool jumping = false;
    public float max_height = 5f;
    public LayerMask ground_layer; 
    public Transform ground_check;
    private bool is_grounded; 
    public Rigidbody2D rb;
    public Player_Attack attack_script;
    public GameObject attack_holder;
    public bool move_left;
    public bool move_right;
    public Vector3 movement;

    public GameObject heartPrefab; // Prefab for the heart
    public Image[] hearts;
    public int max_health;
    public int player_health;
    public GameObject canvas;

    // Define heart size and spacing
    public Vector2 heartSize = new Vector2(2, 2);
    public float heartSpacing = 100f;

    // Animator reference
    private Animator animator;

    void Start()
    {
        // Find the Canvas GameObject in the scene
        canvas = GameObject.Find("Canvas");
        max_health = 3;
        player_health = max_health;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;
        attack_script = attack_holder.GetComponent<Player_Attack>();

        // Get the Animator component
        animator = GetComponent<Animator>();

        for (int i = 0; i < max_health; i++)
        {
            // Instantiate heart prefab under the canvas
            GameObject heartObject = Instantiate(heartPrefab, canvas.transform);
            hearts[i] = heartObject.GetComponent<Image>();

            // Set the scale of the heart
            heartObject.GetComponent<RectTransform>().localScale = new Vector2(2, 2);

            // Position the hearts
            float xPosition = -600 + (100 * i); // Calculate the x position based on index
            heartObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition, 250); // Positioning hearts horizontally
        }
    }

    void Update()
    {
        if (player_health == 0)
        {
            SceneManager.LoadScene("Start_Screen");
        }
        bool is_grounded = Physics2D.OverlapCircle(ground_check.position, 0.3f, ground_layer);

        // Fall if player is not grounded
        if (!is_grounded)
        {
            animator.SetBool("jumping", true); 
            Fall();
        }
        else
        {
            animator.SetBool("jumping", false); 
        }

        // Jump on click if player is grounded
        if (Input.GetButtonDown("Jump") && is_grounded)
        {
            // Remove fall_speed, set max_height, and jump
            fall_speed = 0.0f;
            jumping = true;
            Debug.Log("Grounded");
            max_height = transform.position.y + 5f;
            Jump();
            animator.SetBool("jumping", true); 
        }

        if (Input.GetButtonUp("Jump") && !is_grounded)
        {
            jumping = false;
            fall_speed = -0.1f;
        }

        // Jump if player hasn't reached max_height
        if (jumping)
        {
            if (transform.position.y <= max_height)
            {
                Jump();
            }

            // Stop jumping if player reaches max_height and set fall_speed
            else
            {
                jumping = false;
                fall_speed = -0.1f;
            }
        }

        if (attack_script.attacking == 0)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                attack_script.attacking = 1;
                attack_script.Generate_Attack_Collider('i');
            }

            else if (Input.GetKeyDown(KeyCode.O))
            {
                attack_script.attacking = 1;
                attack_script.Generate_Attack_Collider('o');
            }

            else if (Input.GetKeyDown(KeyCode.P))
            {
                attack_script.attacking = 1;
                attack_script.Generate_Attack_Collider('p');
            }
        }

        // Handle horizontal movement using transform
        float move_horizontal = Input.GetAxis("Horizontal");
        movement = new Vector3(move_horizontal * speed, 0, 0);
        transform.Translate(movement);

        // Continuous movement based on button flags
        if (move_left)
        {
            // Moving left
            transform.localScale = new Vector3(-0.072f, 0.072f, 0.072f); // Flip scale for left
            Move(-1);
        }
        else if (move_right)
        {
            // Moving right
            transform.localScale = new Vector3(0.072f, 0.072f, 0.072f); // Original scale for right
            Move(1);
        }

        // Flip the sprite based on movement direction
        if (move_horizontal > 0)
        {
            // Moving right
            transform.localScale = new Vector3(0.072f, 0.072f, 0.072f); // Original scale for right
        }
        else if (move_horizontal < 0)
        {
            // Moving left
            transform.localScale = new Vector3(-0.072f, 0.072f, 0.072f); // Flip scale for left
        }

        // Set walking animation
        if (movement.x != 0.0f)
        {
            animator.SetBool("moving", true);  // Start walk animation
        }
        else
        {
            animator.SetBool("moving", false);  // Stop walk animation
        }

    }

    public void Call_Attack1()
    {
        if (attack_script.attacking == 0)
        {
            attack_script.attacking = 1;
            attack_script.Generate_Attack_Collider('i');
        }
    }


    public void Call_Attack2()
    {
        if (attack_script.attacking == 0)
        {
            attack_script.attacking = 1;
            attack_script.Generate_Attack_Collider('o');
        }
    }

    public void Call_Attack3()
    {
        if (attack_script.attacking == 0)
        {
            attack_script.attacking = 1;
            attack_script.Generate_Attack_Collider('p');
        }
    }

    public void Take_Damage()
    {
        if (player_health > 0)
        {
            player_health--;  // Reduce health
            Update_Hearts();  // Update UI
        }
    }

    public void Left_Button_Down()
    {
        move_left = true;
    }

    public void Left_Button_Up()
    {
        move_left = false;
    }

    public void Right_Button_Down()
    {
        move_right = true;
    }

    public void Right_Button_Up()
    {
        move_right = false;
    }

    private void Move(int direction)
    {
        movement = new Vector3(direction * speed, 0, 0);
        transform.Translate(movement);
    }

    private void Update_Hearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Set heart visibility based on current health
            hearts[i].enabled = i < player_health;
        }
    }

    void Jump()
    {
        // Apply an upward force to the player's position
        transform.Translate(new Vector3(0, jump_force, 0));
    }

    void Fall()
    {
        // Apply a downward force to the player's position
        transform.Translate(new Vector3(0, fall_speed, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision detection
        Debug.Log("Collided with " + collision.gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("enemy_attack"))
        {
            Move_Enemy_Projectile enemy_projectile_script = collider.gameObject.GetComponent<Move_Enemy_Projectile>();
            if (enemy_projectile_script.redirected == false)
            {
                Transform parent_transform = collider.transform.parent;
                GameObject parent_object = parent_transform.gameObject;
                Enemy_Attack enemy_attack_script = parent_object.GetComponent<Enemy_Attack>();
                enemy_attack_script.Sequence_Fail();
                Debug.Log("Player Took Damage");
                Take_Damage();
            }
        }

        // Handle trigger detection
        Debug.Log("Triggered with " + collider.gameObject.name);
    }
}
