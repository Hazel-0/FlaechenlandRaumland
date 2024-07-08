using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovementAxisTouched : MonoBehaviour
{
    QuestsChapter2 questsChapter2;
    void Start()
    {
        questsChapter2 = GameObject.Find("Scripts").GetComponent<QuestsChapter2>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AxisTracker") { 
            if (this.name == "AxisUp")
            {
                questsChapter2.upperAxisTouched = true;
                Debug.Log("Upper Axis touched");
            }
            else if (this.name == "AxisDown")
            {
                questsChapter2.lowerAxisTouched = true;
                Debug.Log("Lower Axis touched");
            }
        }
    }

}
