using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraTriggerCheck : MonoBehaviour
{
    private QuestsChapter3 questsChapter3;

    private void Start()
    {
        questsChapter3 = GameObject.Find("Scripts").GetComponent<QuestsChapter3>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Main Camera" && questsChapter3.waitingForMainCameraTrigger)
        {
            Debug.Log("Main Camera touched Trigger Object");
            questsChapter3.MainCameraTriggerTouched();
        }
    }
}
