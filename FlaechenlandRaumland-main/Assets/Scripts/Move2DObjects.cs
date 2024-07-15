using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2DObjects : MonoBehaviour
{
    private FadeDesk fadeDesk;
    [SerializeField] public GameObject[] flatlanders;
    [SerializeField] public GameObject[] flatlanderEyes;
    void Start()
    {
        fadeDesk = GameObject.Find("Work_Desk").GetComponent<FadeDesk>();
        flatlanderEyes = GameObject.FindGameObjectsWithTag("Eye");
        //Debug.Log("Number of Eyes found" + flatlanderEyes.Length);
        foreach (GameObject eye in flatlanderEyes)
        {
            if (eye != null)
            {
                eye.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (fadeDesk.flatlandersAlive)
        {
            foreach (GameObject eye in flatlanderEyes)
            {
                if (eye != null)
                {
                    eye.SetActive(true);
                    //Debug.Log("Set eye active " + eye);
                }
            }
            StartMovingAround();
        }
    }

    private void StartMovingAround()
    {
        // führe leichte Bewegungen im Haus aus wie Unterhaltung, evtl. Textur auf Augen und damit Blickrichtung ändern?
        // throw new NotImplementedException();
    }
}
