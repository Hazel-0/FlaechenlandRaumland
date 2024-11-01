using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookingAtSquareCheck : MonoBehaviour
{
    private QuestsChapter3 questsChapter3;

    private void Start()
    {
        questsChapter3 = GameObject.Find("Scripts").GetComponent<QuestsChapter3>();
    }

    void Update()
    {
        // if first trigger for turning around has NOT been triggered
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f) && !questsChapter3.FirstTriggerDone())
        {
            questsChapter3.SquareHit();
        }

        // if first trigger HAS been triggered
        
        /*else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo2, 20f) && questsChapter3.FirstTriggerDone())
        {
            questsChapter3.StandingNearSquare();
        }*/
    }
}


