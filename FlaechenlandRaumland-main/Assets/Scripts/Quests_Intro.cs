using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quests_Intro : MonoBehaviour {


    [SerializeField]
    private GameObject[] audioClips;
    // to play door + background audio
    private AudioSource backgroundMusic;

    [SerializeField]
    private GameObject triggerKugel;

    public bool kugelMussAbgelegtWerden = true;
    public bool kugelRespawnen = false;
    private bool weiterGedrueckt = false;


    private int correctPlacedObects = 0;
    List<string> placedObjects = new();

    void Start() {
        StartCoroutine(QuestLine());
        AudioSetup();
    }

    private void AudioSetup()
    {
        backgroundMusic = GameObject.Find("# Background Music").GetComponent<AudioSource>();
        backgroundMusic.Stop();
    }


    /** Audio Clips
     *    0: Willkommen
     *    1: Einführung
     *    2: Anweisung
     *    3: Abschluss
     */
    IEnumerator QuestLine() {
        // wait for 2 seconds, then play background music
        yield return new WaitForSeconds(2.0f);
        backgroundMusic.Play();
        Debug.Log("Music playing");

        /** Willkommen */
        // wait for 3 seconds, then start initiation
        yield return new WaitForSeconds(3.0f);
        audioClips[0].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(13.0f);
        /** Einführung */
        audioClips[1].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(25.0f);


        /** Anweisung Greifen */
        while (kugelMussAbgelegtWerden) {
            audioClips[2].GetComponent<AudioSource>().Play();
            // triggere Kugel-Respawn für den Fall dass Kugel sich nicht bewegt
            yield return kugelRespawnen = true;  
            yield return new WaitForSeconds(0.01f);
            yield return kugelRespawnen = false;
            yield return new WaitForSeconds(10.0f);
        }
        audioClips[3].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(10.0f);
    }

    public void AddCorrectObject(string id) {
        if (!placedObjects.Contains(id)) {
            placedObjects.Add(id);
            correctPlacedObects++;
            print(correctPlacedObects + " objects were placed correctly!");
            if (correctPlacedObects >= 1) {
                kugelMussAbgelegtWerden = false;
            }
        }
    }

    public void PressedGrabToAdvance()
    {
        weiterGedrueckt = true;
        Debug.Log("weiter gedrückt" + weiterGedrueckt);
    }

}
