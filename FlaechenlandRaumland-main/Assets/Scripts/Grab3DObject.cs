using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab3DObject : MonoBehaviour
{
    private QuestsChapter2 questsChapter2;
    void Start()
    {
        questsChapter2 = GameObject.Find("Scripts").GetComponent<QuestsChapter2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
