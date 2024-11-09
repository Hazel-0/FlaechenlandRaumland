using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDucking : MonoBehaviour
{
    private QuestsChapter3 questsChapter3;
    private void Start()
    {
        questsChapter3 = GameObject.Find("Scripts").GetComponent<QuestsChapter3>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Trigger_ChangeScene")  && questsChapter3.readyToDuck)
        {
            questsChapter3.PlayerIsDucking();
            Debug.Log("Trigger entered for Changing scene");
        }
    }
}
