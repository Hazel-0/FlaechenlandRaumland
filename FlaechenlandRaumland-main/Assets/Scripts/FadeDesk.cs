using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FadeDesk : MonoBehaviour
{
    [SerializeField]
    private Material[] deskMaterials;
    [SerializeField]
    private Material[] flatlandMaterials;
    public bool fadeMaterials = false;
    public bool flatlandersAlive = false;
    public float fadeDuration = 3.0f;

    private float startAlpha = 1.0f;
    private float currentAlpha;
    private float startAlpha_Flatland = 0.0f;
    private float currentAlpha_Flatland;
    private float fadeTimer = 0.0f;
    void Start()
    {
        currentAlpha = startAlpha;
        for (int i = 0; i < deskMaterials.Length; i++)
        {
            // Set the desk materials' alpha to start value
            Color materialColor = deskMaterials[i].color;
            materialColor.a = startAlpha;
            deskMaterials[i].color = materialColor;
        }
        for (int i = 0; i < flatlandMaterials.Length; i++)
        {
            // Set the flatland material's alpha to start value
            Color materialColor_Flatland = flatlandMaterials[i].color;
            materialColor_Flatland.a = startAlpha_Flatland;
            flatlandMaterials[i].color = materialColor_Flatland;
        }
    }

    void Update()
    {
        if (fadeMaterials)
        {
            FadeMaterials();
        }
    }

    private void FadeMaterials()
    {
        fadeTimer += Time.deltaTime;

        // fade out table
        currentAlpha = 1.0f - Mathf.Clamp01(fadeTimer / fadeDuration);
        foreach (Material mat in deskMaterials)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, currentAlpha);
        }

        // fade in flatland 
        currentAlpha_Flatland = (1 - currentAlpha)/2;
        foreach (Material mat in flatlandMaterials)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, currentAlpha_Flatland);
        }

        // check alpha value
        if (currentAlpha <= 0)
        {
            // The material has completely faded out
            GameObject.Find("Desk Frame").SetActive(false);
            GameObject.Find("Desk Top").SetActive(false);
            fadeMaterials = false;
            flatlandersAlive = true;
        }
    }
}
