using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryButton : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string targetMessage;

    public Color highlightColor = Color.cyan;

    private void OnMouseOver()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if  (sprite != null)
        {
            sprite.color = highlightColor;
        }
    }

    public void OnMouseExit() 
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = Color.white;
        }
    }

    public void OnMouseDown()
    {
        transform.localScale = new Vector3(0.05f, 0.05f, 1.0f);
    }

    public void OnMouseUp()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 1.0f);

        if (targetObject != null)
        {
            targetObject.SendMessage(targetMessage);
        }
    }
}
