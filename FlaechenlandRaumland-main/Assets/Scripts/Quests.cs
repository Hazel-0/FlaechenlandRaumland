using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Quests : MonoBehaviour {

    // to play instructions
    [SerializeField]
    private GameObject[] audioClips;
    // to play door + background audio
    private AudioSource backgroundMusic;
    private AudioSource doorClosesAudio;

    [SerializeField]
    private GameObject[] fadeOutObjects;
    [SerializeField]
    private GameObject[] fadeInObjects;


    [SerializeField]
    private GameObject spotlight;
    private Light spotlight_light;

    [SerializeField]
    private GameObject gluehbirne;
    private Rigidbody gluehbirne_rb;
    private Vector3 gluehbirne_pos;
    private Quaternion gluehbirne_rot;
    public bool gluehbirnebereit = false;
    [SerializeField]
    private GameObject gluehbirne_bulb;
    private Material lightbulb_on;
    [SerializeField]
    private Material lightbulb_off;

    public GameObject scripts;
    private RestartGame restart;
    private FadeOutFadeIn fadeOutFadeIn;

    // wird verwendet um Grab-Skripte erst zu aktivieren wenn Objekte sortiert werden sollen
    [SerializeField]
    private GameObject[] sortingObjects;

    private bool giessen = true;
    public bool gie�kanneGegriffen = false;
    public bool pflanze1Fertig = false;

    private bool lichtaus = true;

    public bool lichtWiederAn = false;

    private bool warteAufKoordinatensystem = true;

    private bool sortObjects = true;

    private int correctPlacedObjects = 0;
    List<string> placedObjects = new();

    void Start()
    {
        restart = scripts.GetComponent<RestartGame>();
        fadeOutFadeIn = scripts.GetComponent<FadeOutFadeIn>();
        StartCoroutine(QuestLine());
        spotlight_light = spotlight.GetComponent<Light>();
        gluehbirne_rb = gluehbirne.GetComponent<Rigidbody>();
        lightbulb_on = gluehbirne_bulb.GetComponent<Renderer>().material;
        AudioSetup();
    }

    private void AudioSetup()
    {
        doorClosesAudio = GameObject.Find("DoorAudio").GetComponent<AudioSource>();
        backgroundMusic = GameObject.Find("Background Music").GetComponent<AudioSource>();
        backgroundMusic.Stop();
    }

    /** Audio Clips
     *    0: Schau dich um
     *    1: Gie�kanne greifen
      *   2: Giessen fertig
     *    3: Gl�hbirne rausgefallen
     *    4: Kugel ber�hren
     *    5: L�nge Breite H�he
     *    6: Dreidimensionaler Raum
     *    7: Figuren anschauen
     *    8: Figuren sortieren
     *    9: Sortieren fertig TODO noch mal neu machen, durch T�r gehen f�r Szenenwechsel?
     */
    IEnumerator QuestLine() {
        /** Initiation */
        // wait for 2 seconds, then close door
        yield return new WaitForSeconds(2.0f);
        doorClosesAudio.Play();
        // wait for 2 seconds, then play background music initiation
        yield return new WaitForSeconds(2.0f);
        backgroundMusic.Play();
        yield return new WaitForSeconds(3.0f);

        /** Schau dich um */
        audioClips[0].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(8.0f);

        /** Gie�kanne greifen */
        audioClips[1].GetComponent<AudioSource>().Play();

        while (giessen) {
            yield return null;
        }

        /** Gie�en fertig **/
        yield return new WaitForSeconds(3.0f);
        StopAllAudio();
        audioClips[2].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(10.0f);

        gluehbirne_pos = gluehbirne.transform.position;
        gluehbirne_rot = gluehbirne.transform.rotation;
        spotlight_light.enabled = false;
        gluehbirne_bulb.GetComponent<Renderer>().material = lightbulb_off;
        gluehbirne_rb.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(1.0f);
        gluehbirnebereit = true;

        /** Gl�hbirne einsetzen **/
        audioClips[3].GetComponent<AudioSource>().Play();
        gluehbirne.GetComponent<XRGrabInteractable>().enabled = true;
        while (lichtaus) {
            yield return null;
        }

        /** Gl�hbirne eingesetzt **/
        lichtWiederAn = true;

        // TODO noch einbauen: Audio Clips 04-06 und Logik in Skript ShowCoordinateSystem auf sphere
        // TODO noch Debuggen: ScriptFadeOutFadeIn auf GameObject Scripts f�r alle Room Materials (siehe Array) 
        yield return new WaitForSeconds(1f);
        audioClips[4].GetComponent<AudioSource>().Play();

        //yield return new WaitForSeconds(10f);
        //fadeOutFadeIn.fadeOutRoomMaterials = true;

        while (warteAufKoordinatensystem) {
            yield return null;
        }
        //fadeOutFadeIn.fadeInRoomMaterials = true;


        /** Objekte sortieren **/
        // Aktiviere Skripte
        foreach (GameObject sortingObject in sortingObjects)
        {
            sortingObject.GetComponent<XRGrabInteractable>().enabled = true;
            sortingObject.GetComponent<MeshRenderer>().enabled = true;
            Debug.Log("XRGrab enabled for " + this.name);
        }
        yield return new WaitForSeconds(1f);
        audioClips[8].GetComponent<AudioSource>().Play();
        while (sortObjects) {
            yield return null;
        }

        /** Objekte sortiert **/
        yield return new WaitForSeconds(1f);
        audioClips[9].GetComponent<AudioSource>().Play();



        // restart.AllowRestart();
        

        foreach (GameObject o in fadeOutObjects) {
            o.SetActive(false);
        }
        foreach (GameObject o in fadeInObjects) {
            o.SetActive(true);
        }

        yield return null;
    }

    public void StopAllAudio() {
        foreach (GameObject audio in audioClips) {
            audio.GetComponent<AudioSource>().Stop();
        }
    }

    public void GiessenFertig() {giessen = false;}

    public void Pflanze1Fertig() {pflanze1Fertig = true; Debug.Log("Pflanze 1 fertig"); }

    public void Gie�kanneGegriffen() {gie�kanneGegriffen = true; Debug.Log("Pflanze 2 fertig"); }

    public void GluehbirneFertig() {
        Debug.LogWarning("quest: Gl�hbirne fertig?");
        if (gluehbirnebereit) {
            Debug.LogWarning("quest: -> ja");
            gluehbirne.GetComponent<XRGrabInteractable>().enabled = false;
            gluehbirne.transform.position = gluehbirne_pos;
            gluehbirne.transform.rotation = gluehbirne_rot;
            gluehbirne_rb.constraints = RigidbodyConstraints.FreezeAll;
            gluehbirne_bulb.GetComponent<Renderer>().material = lightbulb_on;
            spotlight_light.enabled = true;
            lichtaus = false;
        }
    }

    public void PlayAudio(int index) {
        audioClips[index].GetComponent<AudioSource>().Play();
    }

    public void KoodinatenSystemFertig() {
        warteAufKoordinatensystem = false;
    }

    public void AddCorrectObject(string id) {
        if (!placedObjects.Contains(id)) {
            placedObjects.Add(id);
            correctPlacedObjects++;
            print(correctPlacedObjects + " objects were placed correctly!");
            if (correctPlacedObjects >= 12) {
                sortObjects = false;
            }
        }

    }

}
