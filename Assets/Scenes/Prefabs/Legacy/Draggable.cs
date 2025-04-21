using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 dragOffset;
    private Camera mainCamera;
    private bool isDragging = false;
    public Color objectColor;

    void Start()
    {
        mainCamera = Camera.main;
        GetComponent<SpriteRenderer>().color = objectColor;
    }

    void OnMouseDown()
    {
        isDragging = true;
        dragOffset = transform.position - GetMouseWorldPos();
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckMarkerHit();
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + dragOffset;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = mainCamera.nearClipPlane; // Distance from camera
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }

    private void CheckMarkerHit()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Marker"))
            {
                Marker marker = hit.GetComponent<Marker>();
                if (marker != null && marker.markerColor == objectColor)
                {
                    Debug.Log("Correct match!");
                    Destroy(gameObject); // Remove object on successful match
                }
            }
        }
    }
}
