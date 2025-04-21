using UnityEngine;

public class Marker : MonoBehaviour
{
    public Color markerColor;

    private void Start()
    {
        // Set the marker's color based on the assigned color
        GetComponent<SpriteRenderer>().color = markerColor;
    }
}
