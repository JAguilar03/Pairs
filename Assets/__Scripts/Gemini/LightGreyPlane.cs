using UnityEngine;

public class LightGrayPlane : MonoBehaviour
{
    public Color planeColor = new Color(0.8f, 0.8f, 0.8f); // Light gray color
    public float planeScaleX = 2000f; // Scale factor for the plane in the X direction
    public float planeScaleY = 100f; // Scale factor for the plane in the Y direction

    void Start()
    {
        // Create a SpriteRenderer component to render the plane
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        // Set the sprite renderer's color to light gray
        spriteRenderer.color = planeColor;

        // Create a simple square sprite for the plane
        Sprite sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);
        spriteRenderer.sprite = sprite;

        // Scale the plane
        transform.localScale = new Vector3(planeScaleX, planeScaleY, 0);

        // Position the plane
        transform.position = new Vector3(-10, -5, 0);
    }
}