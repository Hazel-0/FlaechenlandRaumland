using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2DObjects : MonoBehaviour
{
    private QuestsChapter2 questsChapter2_script;
    [SerializeField] public GameObject[] flatlanders;
    [SerializeField] public GameObject[] flatlanderEyes;
    void Start()
    {
        questsChapter2_script = GameObject.Find("Scripts").GetComponent<QuestsChapter2>();
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
        if (questsChapter2_script.flatlandExpanding)
        {
            foreach (GameObject eye in flatlanderEyes)
            {
                if (eye != null)
                {
                    eye.SetActive(true);
                }
            }
        }
    }
}
