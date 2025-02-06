using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class SphereMovementAxisTouched : MonoBehaviour
{
    QuestsChapter2 questsChapter2;
    private bool axisUpTouched = false;
    void Start()
    {
        questsChapter2 = GameObject.Find("Scripts").GetComponent<QuestsChapter2>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Axis touched");
        if (other.tag == "AxisTracker" || other.name == "AxisTracker" || other.name == "sphere" ) {
            Debug.Log("Axis touched by sphere");
            if (this.name == "AxisUp")
            {
                questsChapter2.upperAxisTouched = true;
                Debug.Log("Upper Axis touched");
                axisUpTouched = true;
            }
            else if (this.name == "AxisDown") // && axisUpTouched
            {
                questsChapter2.lowerAxisTouched = true;
                Debug.Log("Lower Axis touched");
            }
        }
    }

}
