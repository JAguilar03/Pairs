using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sprite_Generator : MonoBehaviour
{
    public GameObject player_prefab;
    public GameObject enemy_prefab;
    public GameObject camera_prefab; 
    private GameObject player;
    public Player_Movement player_script;
    public Enemy_Behavior enemy_script;
    public EventTrigger left_button;
    public EventTrigger right_button;
    public Button attack1;
    public Button attack2;
    public Button attack3;
    bool respawn_called;

    public GameObject platform;  
    public Transform player_transform;     

    public SpriteRenderer background1;
    public SpriteRenderer background2;
    public List<SpriteRenderer> background_list = new List<SpriteRenderer>();


    private float lastXPosition;    // To track the last repositioned platform's X position


    // Start is called before the first frame update
    void Start()
    {

        background_list.Add(background1);
        background_list.Add(background2);
        // Instantiate the player first
        player = Instantiate(player_prefab, new Vector3(-12.18f, -3.1f, -1f), Quaternion.identity);
        player_script = player.GetComponent<Player_Movement>();
        player_transform = player.transform;
        

        // Ensure the player is created before generating the enemy
        if (player != null)
        {
            GameObject enemy = Instantiate(enemy_prefab, new Vector3(13f, -3.5f, -1f), Quaternion.identity);
            enemy_script = enemy.GetComponent<Enemy_Behavior>();
            enemy_script.player = player.transform;  // Assign player to enemy


            // Create and configure the camera
            GameObject camera = Instantiate(camera_prefab, new Vector3(0, 0, -10), Quaternion.identity);
            Camera_Move camera_controller = camera.AddComponent<Camera_Move>();
            camera_controller.player = player.transform;
            
        }
        else
        {
            Debug.LogError("Player creation failed.");
        }

        Setup_Buttons();

    }

    void Update()
    {
        platform.transform.position = new Vector3(player_transform.position.x, -4.94f, -1.0f);

        if (enemy_script.active == false && respawn_called == false)
        {
            respawn_called = true;
            Invoke("Call_Respawn", 3f);
        }
        Background_Scroll();
    }

    void Setup_Buttons()
    {
        // Setup attack button onclicks
        attack1.onClick.AddListener(player_script.Call_Attack1);
        attack2.onClick.AddListener(player_script.Call_Attack2);
        attack3.onClick.AddListener(player_script.Call_Attack3);


        // Setup left button event triggers
        AddEventTrigger(left_button, EventTriggerType.PointerDown, _ => player_script.Left_Button_Down());
        AddEventTrigger(left_button, EventTriggerType.PointerUp, _ => player_script.Left_Button_Up());

        // Setup right button event triggers
        AddEventTrigger(right_button, EventTriggerType.PointerDown, _ => player_script.Right_Button_Down());
        AddEventTrigger(right_button, EventTriggerType.PointerUp, _ => player_script.Right_Button_Up());
    }

    private void AddEventTrigger(EventTrigger eventTrigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        // Create a new EventTrigger.Entry
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventType
        };

        // Add a listener to the entry
        entry.callback.AddListener(action);

        // Add the entry to the event trigger
        eventTrigger.triggers.Add(entry);
    }

    private void Call_Respawn()
    {
        enemy_script.Spawn_Enemy(player_transform);

        respawn_called = false;
    }


    private void Background_Scroll()
    {
        // background1_bounds = background1.bounds;
        // background2_bounds = background2.bounds;
       // Debug.Log("In background 1");

        if(player_transform.position.x >= background1.bounds.min.x && player_transform.position.x <= background1.bounds.max.x)
        {
            Debug.Log("In background 1");
            Position_Backgrounds(0, 1);
        }

        else if(player_transform.position.x >= background2.bounds.min.x && player_transform.position.x <= background2.bounds.max.x)
        {
            Debug.Log("In background 2");
            Position_Backgrounds(1, 0);
        }

    }

    private void Position_Backgrounds(int curr_background, int background_to_position)
    {
        // Get the center position of the current background
        float currCenterX = background_list[curr_background].bounds.center.x;

        // Determine the new position for the background to position
        if (player_transform.position.x < currCenterX)
        {
            // Position to the left of the current background
            float newPosX = currCenterX - background_list[curr_background].bounds.size.x;
            background_list[background_to_position].transform.position = new Vector3(newPosX, background_list[background_to_position].transform.position.y, background_list[background_to_position].transform.position.z);
        }
        else if (player_transform.position.x > currCenterX)
        {
            // Position to the right of the current background
            float newPosX = currCenterX + background_list[curr_background].bounds.size.x;
            background_list[background_to_position].transform.position = new Vector3(newPosX, background_list[background_to_position].transform.position.y, background_list[background_to_position].transform.position.z);
        }
    }


}
