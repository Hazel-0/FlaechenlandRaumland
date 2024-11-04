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
        Debug.Log("Trigger 'Player ducking' enter");
        if (other.tag.Equals("Trigger_ChangeScene"))
        {
            questsChapter3.WaitForPlayerDucking();
            Debug.Log("Trigger entered for Changing scene");
        }
    }
}
