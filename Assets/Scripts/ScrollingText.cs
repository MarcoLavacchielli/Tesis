using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingText : MonoBehaviour
{
    public TextMeshPro textComponent;
    public float scrollSpeed = 50f;

    private string originalText;
    private float textWidth;

    void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshPro>();
        }

        originalText = textComponent.text;
        textWidth = textComponent.preferredWidth;
    }

    void Update()
    {
        // Move the text to the right
        Vector3 position = textComponent.rectTransform.localPosition;
        position.x += scrollSpeed * Time.deltaTime;

        // If the text has moved completely out of view, reset its position
        if (position.x > textWidth)
        {
            position.x = -textComponent.rectTransform.rect.width;
        }

        textComponent.rectTransform.localPosition = position;
    }
}
