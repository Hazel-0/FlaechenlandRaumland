using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiessenAudio : MonoBehaviour
{
    [SerializeField]
    private GameObject[] audioClips;

    private Quests quests;

    void Start() {
        quests = GameObject.Find("Scripts").GetComponent<Quests>();
    }

    public void GiesskanneGegriffenAudio()
    {
        Debug.Log("GiesskanneGegriffenAudio");
        quests.StopAllAudio();
        audioClips[0].GetComponent<AudioSource>().Play();
    }
    public void PflanzeAudio(int index)
    {
        Debug.Log("Pflanze" + index + "Audio");
        audioClips[index].GetComponent<AudioSource>().Play();

        // funktioniert das: Audio "gut gemacht" kommt direkt nach dem Wachsen
        quests.GiessenFertig();
    }
}
