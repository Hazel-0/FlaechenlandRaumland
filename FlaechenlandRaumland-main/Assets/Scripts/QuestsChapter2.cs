using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class QuestsChapter2 : MonoBehaviour {

    // to play instructions
    [SerializeField]
    private GameObject[] audioClips;

    [SerializeField]
    private GameObject[] grabCommands;

    [SerializeField]
    private GameObject[] objects2D;
    [SerializeField]
    private GameObject[] objects3D;

    private int objectIndex = 0;
    private int heldObject = -1;

    [SerializeField]
    private GameObject screen; 

    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject cameraPlane;
    private Plane plane;

    private bool trackObject;

    [SerializeField]
    private GameObject corner_max;
    [SerializeField]
    private GameObject corner_min;

    private Vector3 offset;

    // to play door + background audio
    private AudioSource backgroundMusic;
    private AudioSource doorClosesAudio;

    private FadeDesk fadeDesk;
    public bool cylinderNotGrabbed = true;

    // k�nnte man nutzen um Licht erst zu dimmen wenn die Tischplatte ausgeblendet wird
    // [SerializeField]
    private GameObject spotlight;
    private Light spotlight_light;

    // k�nnte man gebrauchen um zu tracken wovon schon Querschnitt gemacht wurde
    //private bool sortObjects = true;
    //private int correctPlacedObects = 0;
    //List<string> placedObjects = new List<string>();

    // k�nnte man gebrauchen um zu tracken wovon schon Querschnitt gemacht wurde
    //private bool sortObjects = true;
    //private int correctPlacedObjects = 0;
    //List<string> placedObjects = new List<string>();

    void Start()
    {
        offset = objects3D[objectIndex].transform.position - camera.transform.position;
        foreach (GameObject obj in objects3D)
        {
            obj.GetComponent<XRGrabInteractable>().enabled = false;
        }
        AudioSetup();
        StartCoroutine(QuestLine());

        spotlight = GameObject.Find("Spot Light"); // wird noch nicht benutzt (dimmen?)
        spotlight_light = spotlight.GetComponent<Light>(); // wird noch nicht benutzt (dimmen?)
        fadeDesk = GameObject.Find("Work_Desk").GetComponent<FadeDesk>();
    }

    private void AudioSetup()
    {
        doorClosesAudio = GameObject.Find("(00) DoorAudio").GetComponent<AudioSource>();
        backgroundMusic = GameObject.Find("# Background Music").GetComponent<AudioSource>();
        backgroundMusic.Stop();
    }

    IEnumerator QuestLine() {
        screen.SetActive(false);
        /** Initiation */
        // wait for 2 seconds, then close door
        yield return new WaitForSeconds(2.0f);
        doorClosesAudio.Play();
        // wait for 2 seconds, then play background music initiation
        yield return new WaitForSeconds(2.0f);
        backgroundMusic.Play();
        yield return new WaitForSeconds(2.0f);
        // play initiation audio
        audioClips[0].GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(15.0f);

        objects3D[0].GetComponent<XRGrabInteractable>().enabled = true;

        for (int i = 0; i < objects3D.Length; i++)
        {
            float waitTime = playGrabCommand(i);
            // wait until command ends
            yield return new WaitForSeconds(waitTime);
            // make object grabbable
            activateObject(i);
            while (heldObject != i)
            {
                // wait until object is picked up
                yield return null;
                Debug.Log("index for object 0 " + i);
            }
            if (i == 0)
            {
                Debug.Log("Fade Desk");
                // first object -> fade desk
                fadeDesk.fadeMaterials = true;
                screen.SetActive(true);
                // yield return new WaitForSeconds(5.0f);

                /** Querschnitt Zylinder **/
                AudioSource audio = audioClips[1].GetComponent<AudioSource>();
                audio.Play();

                // flatlanders start wiggling
                foreach (GameObject obj in objects2D)
                {
                    obj.GetComponent<Animator>().SetTrigger("StartWiggling");
                    Debug.Log("StartWiggling");
                }

                yield return new WaitForSeconds(audio.clip.length);

                // flatlanders leave house to watch objects
                foreach (GameObject obj in objects2D)
                {
                    obj.GetComponent<Animator>().SetTrigger("LeaveHouse");
                }
            }
            while (heldObject != -1)
            {
                // wait until object is dropped
                yield return null;
            }
        }


/** Zylinder greifen */
        audioClips[1].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(5.0f);
    }


    private void activateObject(int index)
    {
        if (index > 0)
        {
            // make previous object not grabbable
            objects3D[index - 1].GetComponent<XRGrabInteractable>().enabled = false;
        }
        objects3D[index].GetComponent<XRGrabInteractable>().enabled = true;

        trackObject = true; // cross section cam tracks the object
        objectIndex = index;
    }


/*public void AddCorrectObject(string id) {
if (!placedObjects.Contains(id)) {
    placedObjects.Add(id);
    correctPlacedObects++;
    print(correctPlacedObects + " objects were placed correctly!");
    if (correctPlacedObects >= 12) {
        this.sortObjects = false;
    }
}
}*/
    private void Update()
    {
        Vector3 newPos = camera.transform.position;
        float newX = objects3D[objectIndex].transform.position.x - offset.x;
        float newZ = objects3D[objectIndex].transform.position.z - offset.z;

        // cross section cam should hover over the object but should not leave the table
        //float x = Mathf.Clamp(newX, corner_max.transform.position.x, corner_min.transform.position.x);
        //float z = Mathf.Clamp(newZ, corner_max.transform.position.z, corner_min.transform.position.z);
        //Debug.Log("x: " + x);
        //Debug.Log("z: " + z);
        //Vector3 pos = new Vector3(x, camera.transform.position.y, z);
        //Debug.Log(pos);
        newPos.x = newX;
        newPos.z = newZ;
        camera.transform.position = newPos;
    }

    private float playGrabCommand(int i)
    {
        AudioSource audio = grabCommands[i].GetComponent<AudioSource>();
        audio.Play();
        return audio.clip.length;
    }

    public void HoldObject(int index)
    {
        heldObject = index;
    }
    public void DropObject(int index)
    {
        heldObject = -1;
    }
}