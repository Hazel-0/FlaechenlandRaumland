using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideTriggerCheck : MonoBehaviour
{

    private QuestsChapter3 questsChapter3;

    private void Start()
    {
        questsChapter3 = GameObject.Find("Scripts").GetComponent<QuestsChapter3>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger 'Near Square' enter");
        if (other.tag.Equals("Trigger_ContinueTalking"))
        {
            questsChapter3.StandingNearSquare();
            Debug.Log("Trigger entered for Standing near square");
        }
    }
    /*private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger stay");
        if (other.tag.Equals("Trigger_ContinueTalking"))
        {
            questsChapter3.StandingNearSquare();
            Debug.Log("Trigger stay for Standing near square");
        }
    }*/
}
