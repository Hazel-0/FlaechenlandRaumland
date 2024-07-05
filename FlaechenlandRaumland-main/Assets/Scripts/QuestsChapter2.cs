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
    private GameObject[] grabCommands; //new

    [SerializeField]
    private GameObject[] objects2D;
    [SerializeField]
    private GameObject[] objects3D; // called "objects in Jan's script"

    // below all new
    private int objectIndex = 0; //new
    private int heldObject = -1; //new

    [SerializeField]
    private GameObject screen; //new

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

    // above all new

    // to play door + background audio
    private AudioSource backgroundMusic;
    private AudioSource doorClosesAudio;

    private FadeDesk fadeDesk;
    public bool cylinderNotGrabbed = true;

    // könnte man nutzen um Licht erst zu dimmen wenn die Tischplatte ausgeblendet wird
    // [SerializeField]
    private GameObject spotlight;
    private Light spotlight_light;

    // könnte man gebrauchen um zu tracken wovon schon Querschnitt gemacht wurde
    //private bool sortObjects = true;
    //private int correctPlacedObects = 0;
    //List<string> placedObjects = new List<string>();

    // könnte man gebrauchen um zu tracken wovon schon Querschnitt gemacht wurde
    //private bool sortObjects = true;
    //private int correctPlacedObjects = 0;
    //List<string> placedObjects = new List<string>();

    void Start()
    {
        offset = objects3D[objectIndex].transform.position - camera.transform.position; //neu
        foreach (GameObject obj in objects3D)
        {
            obj.GetComponent<XRGrabInteractable>().enabled = false;
            //obj.GetComponent<Collider>().enabled = false;
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
        screen.SetActive(false); //new
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

        // new below here

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

// new above here

/** Zylinder greifen */
        audioClips[1].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(7.0f);


        /* foreach (GameObject obj in objects3D)
        {
            obj.GetComponent<Collider>().enabled = true;
            obj.GetComponent<XRGrabInteractable>().enabled = true;
        }
        while (cylinderNotGrabbed)
        {
            yield return null;
        } */

        // start fading desk, flatlanders become alive
        /* foreach (GameObject obj in objects2D)
        {
            obj.GetComponent<Animator>().SetTrigger("StartWiggling");
        }
        fadeDesk.fadeMaterials = true; */
        //yield return new WaitForSeconds(12.0f);

        /** Querschnitt Zylinder **/

        /* audioClips[2].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(5.0f);

        // flatlanders leave house to watch objects
        foreach (GameObject obj in objects2D)
        {
            obj.GetComponent<Animator>().SetTrigger("LeaveHouse");
        } */

        // Achse erscheint, in der Kugel durch den Tisch bewegt werden solls

        /*while (sortObjects) {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        audioClips[5].GetComponent<AudioSource>().Play();

        yield return null; */
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
    //new below here

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

    //new above here

}
