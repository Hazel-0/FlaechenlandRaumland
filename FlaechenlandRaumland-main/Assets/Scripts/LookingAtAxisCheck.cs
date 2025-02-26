using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAtAxisCheck : MonoBehaviour
{
    private QuestsChapter3 questsChapter3;

    private void Start()
    {
        questsChapter3 = GameObject.Find("Scripts").GetComponent<QuestsChapter3>();
    }

    void Update()
    {
        // if trigger for looking at axis has NOT been triggered
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 60f) && !questsChapter3.SecondTriggerDone())
        {
            questsChapter3.AxisHit();
            Debug.Log("Axis hit with raycast");
        }
    }
}
