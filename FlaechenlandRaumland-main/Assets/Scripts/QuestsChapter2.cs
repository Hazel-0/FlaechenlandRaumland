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
    public float waitTillDropTime;

    // könnte man nutzen um Licht erst zu dimmen wenn die Tischplatte ausgeblendet wird
    // [SerializeField]
    //private GameObject spotlight;
    //private Light spotlight_light;
    private SphereMovementAxis sphereMovementAxis;

    // für Animation der Augen in Move2DObjects
    public bool flatlandExpanding = false;
    
    // für Kapitelende
    [SerializeField]
    private SceneControl sceneControlScript;

    private GameObject sphereMovementAxisObj;
    public bool upperAxisTouched = false;
    public bool lowerAxisTouched = false;
    private bool sceneFinished = false;
    private bool houseTouched = false;
    private Flatland flatland;

    void Start()
    {
        offset = objects3D[objectIndex].transform.position - camera.transform.position;
        foreach (GameObject obj in objects3D)
        {
            obj.GetComponent<XRGrabInteractable>().enabled = false;
        }
        AudioSetup();
        StartCoroutine(QuestLine());

        //spotlight = GameObject.Find("Spot Light"); // wird noch nicht benutzt (dimmen?)
        //spotlight_light = spotlight.GetComponent<Light>(); // wird noch nicht benutzt (dimmen?)
        fadeDesk = GameObject.Find("Work_Desk").GetComponent<FadeDesk>();
        sphereMovementAxisObj = GameObject.Find("SphereMovementAxis");
        sphereMovementAxis = sphereMovementAxisObj.GetComponent<SphereMovementAxis>();
        flatland = GameObject.Find("Flatland").GetComponent<Flatland>();
        sphereMovementAxisObj.SetActive(true);
    }

    private void AudioSetup()
    {
        doorClosesAudio = GameObject.Find("(00) DoorAudio").GetComponent<AudioSource>();
        backgroundMusic = GameObject.Find("# Background Music").GetComponent<AudioSource>();
        backgroundMusic.Stop();
    }

    IEnumerator QuestLine() {
        screen.SetActive(false);
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
            // wait until audio ends
            yield return new WaitForSeconds(waitTime);

            // make object grabbable
            ActivateObject(i);

            // wait for cylinder to be grabbed
            while (heldObject != i)
            {
                // wait until object is picked up
                yield return null;
                // Debug.Log("index for object 0 " + i);
            }

            // first object grabbed: activate flatland
            if (i == 0)
            {
                yield return new WaitForSeconds(2.0f);
                Debug.Log("Fade Desk");
                fadeDesk.fadeMaterials = true;
                screen.SetActive(true);

                // wait for Flatland to expand 
                yield return new WaitForSeconds(3.0f);
                flatland.Expand();
                flatlandExpanding = true;

                // flatlanders start wiggling
                foreach (GameObject obj in objects2D)
                {
                    obj.GetComponent<Animator>().SetTrigger("StartWiggling");
                    Debug.Log("StartWiggling");
                }

                // explain cross-section with cylinder
                AudioSource audio1 = audioClips[1].GetComponent<AudioSource>();
                audio1.Play();

                // flatlanders leave house to watch objects
                foreach (GameObject obj in objects2D)
                {
                    obj.GetComponent<Animator>().SetTrigger("LeaveHouse");
                }

                sphereMovementAxis.ActivateAxes();

                yield return new WaitForSeconds(20.0f);
                AudioSource audio2 = audioClips[2].GetComponent<AudioSource>(); // play cylinder drop information
                audio2.Play();

                yield return new WaitForSeconds(audio2.clip.length);
            }

            if (i == 6) 
            {
                AudioSource audio4 = audioClips[4].GetComponent<AudioSource>();
                audio4.Play();

                yield return new WaitForSeconds(waitTillDropTime + audio4.clip.length); // TODO: wird der Clip länger abgespielt?
                AudioSource audio3 = audioClips[3].GetComponent<AudioSource>();
                audio3.Play();

                while (!upperAxisTouched) { yield return null; }
                while (!lowerAxisTouched) { yield return null; }
                Debug.Log("Both axis touched");
            }

            else if (i >= 1 && i < 6) {
                yield return new WaitForSeconds(waitTillDropTime);
                AudioSource audio3 = audioClips[3].GetComponent<AudioSource>(); 
                audio3.Play();
            }

            while (heldObject != -1)
            {
                // wait until object is dropped
                yield return null;
            }
        }

        while (!sceneFinished)
        {
            yield return null;
            Debug.Log("Scene not yet finished");
        }

        Debug.Log("Audio: Ins Flächenland");
        AudioSource audio5 = audioClips[5].GetComponent<AudioSource>();
        audio5.Play();
        Debug.Log("All 3D objects done");

        // if house was touched
        while (!houseTouched)
        {
            yield return null;
        }

        sphereMovementAxisObj.SetActive(false);
        Debug.Log("change to next scene");
        yield return new WaitForSeconds(3.0f);
        sceneControlScript.enabled = true;
    }

    private void ActivateObject(int index)
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

    public void UpperAxisTouched()
    {
        upperAxisTouched = true;
    }
    public void LowerAxisTouched()
    {
        if (upperAxisTouched) {
            lowerAxisTouched = true;
            sceneFinished = true;
        }
    }
    public void HouseTouched()
    {
        if (upperAxisTouched && lowerAxisTouched)
        {
            houseTouched = true;
        }
    }
}
