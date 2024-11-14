using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDucking : MonoBehaviour
{
    [SerializeField]
    private QuestsChapter3 questsChapter3;
    private void Start()
    {
        if (questsChapter3 == null)
        {
            Debug.Log("Finding questsChapter3");
            questsChapter3 = GameObject.Find("Scripts").GetComponent<QuestsChapter3>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Trigger_ChangeScene"))
            {
            Debug.Log("trigger for scene change entered");
            questsChapter3.PlayerIsDucking();
        }

        /*if (other.tag.Equals("Trigger_ChangeScene")  && questsChapter3.readyToDuck)
        {
            questsChapter3.PlayerIsDucking();
            Debug.Log("Trigger entered for Changing scene");
        }*/
    }
}
