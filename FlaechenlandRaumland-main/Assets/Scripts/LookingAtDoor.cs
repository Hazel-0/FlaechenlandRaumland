using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAtDoor : MonoBehaviour
{
    private QuestsChapter4 questsChapter4;

    private void Start()
    {
        questsChapter4 = GameObject.Find("Scripts").GetComponent<QuestsChapter4>();
    }

    void Update()
    {
        // if trigger for turning around has NOT been triggered
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f) 
            && hitInfo.collider.gameObject.tag == "Trigger_Hypersphere")
        {
            questsChapter4.LookingAtDoor();
        }
    }
}
