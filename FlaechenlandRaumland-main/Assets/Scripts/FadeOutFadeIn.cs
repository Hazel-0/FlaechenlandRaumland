using System;
using System.Collections;
using UnityEngine;

public class FadeOutFadeIn : MonoBehaviour
{
    [SerializeField]
    private Material[] roomMaterials;
    public bool fadeOutRoomMaterials = false;
    public bool fadeInRoomMaterials = false;
    public float fadeDuration = 3.0f;

    private float startAlpha = 1.0f;
    private float currentAlpha;
    private float fadeTimer = 0.0f;

    void Start()
    {
        currentAlpha = startAlpha;
        for (int i = 0; i < roomMaterials.Length; i++)
        {
            // Set the desk materials' alpha to start value
            Color materialColor = roomMaterials[i].color;
            materialColor.a = startAlpha;
            roomMaterials[i].color = materialColor;
        }
    }

    void Update()
    {
        if (fadeOutRoomMaterials)
        {
            Debug.Log("Start Fade Out Materials");
            FadeOutRoomMaterials();
        }

        if (fadeInRoomMaterials)
        {
            Debug.Log("Start Fade In Materials");
            FadeInRoomMaterials();
        }
    }


    private void FadeOutRoomMaterials()
    {
        fadeTimer += Time.deltaTime;

        // fade out table
        currentAlpha = 1.0f - Mathf.Clamp01(fadeTimer / fadeDuration);
        foreach (Material mat in roomMaterials)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, currentAlpha);
        }

        // check alpha value
        if (currentAlpha <= 0)
        {
            // The material has completely faded out
            fadeOutRoomMaterials = false;
            fadeTimer = 0.0f;
        }
    }
    private void FadeInRoomMaterials()
    {
        fadeTimer += Time.deltaTime;

        // fade out table
        currentAlpha = 0.0f + Mathf.Clamp01(fadeTimer / fadeDuration);
        foreach (Material mat in roomMaterials)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, currentAlpha);
        }

        // check alpha value
        if (currentAlpha >= 1)
        {
            // The material has completely faded out
            fadeInRoomMaterials = false;
        }
    }
}
