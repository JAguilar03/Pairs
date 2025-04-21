using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    public bool attack_performed = false;
    public char[] input_pool = {'i', 'o', 'p'}; 
    public int num_inputs_required = 3;
    public char[] required_inputs;
    public int curr_input = 0;
    public GameObject projectile;
    public int poolSize = 3;
    public List<GameObject> projectile_pool = new List<GameObject>();

    public SpriteRenderer[] spriteRenderers; // Array for the SpriteRenderer components
    public Sprite[] imageSequence; // Array of Sprites to display

    public void Start()
    {
        required_inputs = new char[num_inputs_required];
        projectile_pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject curr_projectile = Instantiate(projectile, transform.position, transform.rotation);
            curr_projectile.transform.SetParent(transform);
            projectile_pool.Add(curr_projectile);
        }
        Generate_Sequence();

        // Initially set all sprites to be empty
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sprite = null; // Clear the sprite
        }

    }

    public void Start_Attack(int attack_number)
    {   
        // Call the Attack method after 1 seconds
        Invoke("Perform_Attack", attack_number * 1);
    }

    private void Perform_Attack()
    {
        // Initially set all sprites to be empty
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sprite = null; // Clear the sprite
        }
        
        GameObject curr_projectile = projectile_pool[curr_input];
        Move_Enemy_Projectile projectile_movement_script = curr_projectile.GetComponent<Move_Enemy_Projectile>();
        projectile_movement_script.Activate_Projectile(transform);
        projectile_movement_script.required_attack = required_inputs[curr_input];
    


        if(curr_input == num_inputs_required - 1)
        {
            attack_performed = true;
            curr_input = 0;
        }

        else
        {
            curr_input += 1;
        }

    }

    private void Reactivate_Enemy()
    {
        attack_performed = true;
        Generate_Sequence();
        curr_input = 0; 
    }

    public void Generate_Sequence()
    {
        for (int i = 0; i < num_inputs_required; i++)
        {
            int random_index = Random.Range(0, num_inputs_required);
            required_inputs[i] = input_pool[random_index];
        }
    }

    public void Display_Sequence()
    {
        string test = new string(required_inputs);
        Debug.Log(test);

        // Assign the images to the sprite renderers based on your sequence logic
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (required_inputs[i] == 'i')
            {
                spriteRenderers[i].sprite = imageSequence[0]; // Set the sprite
            }

            else if (required_inputs[i] == 'o')
            {
                spriteRenderers[i].sprite = imageSequence[1]; // Set the sprite

            }

            else
            {
                spriteRenderers[i].sprite = imageSequence[2]; // Set the sprite
            }
        }
    }

    public void Sequence_Fail()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject curr_projectile = projectile_pool[i];
            Move_Enemy_Projectile projectile_movement_script = curr_projectile.GetComponent<Move_Enemy_Projectile>();
            projectile_movement_script.Deactivate_Projectile();
        }
        CancelInvoke("Perform_Attack");
        CancelInvoke("Reactivate_Enemy");
        CancelInvoke("Start_Attack");
        CancelInvoke("Call_Attacks");
        Reactivate_Enemy();

    }

    public bool Check_Active()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject curr_projectile = projectile_pool[i];
            Move_Enemy_Projectile projectile_movement_script = curr_projectile.GetComponent<Move_Enemy_Projectile>();
            if (projectile_movement_script.active == true)
            {
                return true;
            }
        }

        return false;
    }

}
