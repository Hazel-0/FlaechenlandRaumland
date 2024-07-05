using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderPickup : MonoBehaviour

{
    private QuestsChapter2 questsChapter2;
    private BoxCollider boxCollider;
    private void Start()
    {
        questsChapter2 = GameObject.Find("Scripts").GetComponent<QuestsChapter2>();
        boxCollider = GetComponent<BoxCollider>();
        DisableCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GrabVolumeBig")
        {
            Debug.Log("Cylinder grabbed");
            questsChapter2.cylinderNotGrabbed = false;
        }
    }
    public void EnableCollider() {boxCollider.enabled = true; }
    public void DisableCollider() {boxCollider.enabled = false; }
}
