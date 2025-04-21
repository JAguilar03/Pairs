using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    void Start()
    {
        // Create a new GameObject
        GameObject square = new GameObject("Square");

        // Add a SpriteRenderer component to the GameObject
        SpriteRenderer renderer = square.AddComponent<SpriteRenderer>();

        // Create a square sprite using a 1x1 white texture
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        // Set the sprite to the SpriteRenderer
        renderer.sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));

        // Scale the GameObject to set the desired size
        square.transform.localScale = new Vector3(30, 30, 1);
        
        // Optional: Set the position of the square
        square.transform.position = new Vector3(0, 0, -1);
    }
}

// 0d6fc-997c6ab22d-b87500cd1b-cb96b35b2f-872c5597a2-c903494c1b-bb21680ece-122753e402-0bd555307d-a0e58226ed-e3e72dc34f-a093712f83-3d76df0179-19679d899b-4bfbf00942-b88f9648c8-20faa