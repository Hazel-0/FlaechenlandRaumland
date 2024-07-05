using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gluehbirne : MonoBehaviour {

    public GameObject scripts;

    private Quests quests;
    void Start() {
        quests = scripts.GetComponent<Quests>();
    }

    private void OnTriggerEnter(Collider other) {
        Debug.LogWarning("Gewinde Trigger!");
        Debug.LogWarning(other.tag);
        if (other.tag == "Lightbulb") {
            Debug.LogWarning("Glühbirne erkannt!");
            quests.GluehbirneFertig();
        }
    }
}
