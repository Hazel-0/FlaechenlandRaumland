using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAxisCheck : MonoBehaviour
{
    private QuestsChapter2 questsChapter2;

    void Start()
    {
        questsChapter2 = GameObject.Find("Scripts").GetComponent<QuestsChapter2>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger 'Axis' enter");
        if (other.tag.Equals("AxisUp"))
        {
            questsChapter2.UpperAxisTouched();
            Debug.Log("Trigger entered for AxisUp");
        }

        else if (other.tag.Equals("AxisDown"))
        {
            questsChapter2.LowerAxisTouched();
            Debug.Log("Trigger entered for AxisDown");
        }
    }
}
