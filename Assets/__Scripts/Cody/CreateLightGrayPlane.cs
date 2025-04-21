using UnityEngine;

public class CreateLightGrayPlane : MonoBehaviour
{
    public Vector2 planeSize = new Vector2(10f, 10f);
    public Color planeColor = new Color(0.8f, 0.8f, 0.8f); // Light gray color

    void Start()
    {
        CreatePlane();
    }

    void CreatePlane()
    {
        // Create a new GameObject for the plane
        GameObject plane = new GameObject("LightGrayPlane");

        // Add a SpriteRenderer component to the plane
        SpriteRenderer spriteRenderer = plane.AddComponent<SpriteRenderer>();

        // Create a new texture for the plane
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, planeColor);
        texture.Apply();

        // Create a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));

        // Assign the sprite to the SpriteRenderer
        spriteRenderer.sprite = sprite;

        // Set the size of the plane
        plane.transform.localScale = planeSize;
    }
}
