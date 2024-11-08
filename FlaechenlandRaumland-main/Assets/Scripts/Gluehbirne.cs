using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gluehbirne : MonoBehaviour {

    private Quests quests;

    void Start() {
        quests = GameObject.Find("Scripts").GetComponent<Quests>();
        if (quests != null ) {
            Debug.LogWarning("quests script not fount by Gluehbirne");
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.LogWarning("Gewinde Trigger! " + other.tag);
        if (other.tag == "Lightbulb") {
            Debug.LogWarning("Glühbirne erkannt!");
            quests.GluehbirneFertig();
        }
    }
}
