using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenOnDeath : MonoBehaviour
{
    public Image panelImage;
    public float fadeSpeed = 1.0f;
    private bool isFading = false;

    private void Start()
    {
        StartCoroutine(FadeToClear());
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && !isFading)
        {
            StartCoroutine(FadeToClear());
        }
    }*/

    public void fade()
    {
        StartCoroutine(FadeToClear());
    }

    IEnumerator FadeToClear()
    {
        isFading = true;

        Color panelColor = panelImage.color;
        panelColor.a = 1.0f;
        panelImage.color = panelColor;

        while (panelColor.a > 0.0f)
        {
            panelColor.a -= fadeSpeed * Time.deltaTime;
            panelImage.color = panelColor;

            yield return null;
        }

        panelColor.a = 0.0f;
        panelImage.color = panelColor;

        isFading = false;
    }
}
