using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaceInsideBowl : MonoBehaviour
{
    [SerializeField] Material neutralMat;
    [SerializeField] Material individualMat;
    public AudioClip correctActionAudio;
    AudioSource audioSource;
    XRGrabInteractable grabbableScript;

    private Quests questControl;
    private Rigidbody this_Rigidbody;
    private bool soundPlayed;

    void Start() {
        individualMat = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material = neutralMat;

        audioSource = GetComponent<AudioSource>(); // in case there are several audio sources, e.g. sphere

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
        }

        grabbableScript = GetComponent<XRGrabInteractable>();
        //grabbableScript.enabled = true;

        // get quest controller
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        questControl = gameManager.GetComponent<Quests>(); // changed from Quests (Chapter1) to Quests_Intro

        // get and set rigidbody
        this_Rigidbody = this.GetComponent<Rigidbody>();
        this_Rigidbody.constraints = RigidbodyConstraints.None;

        soundPlayed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bowl" && this.tag == "3D")
        {
            GetComponent<Renderer>().material = individualMat;
            PlayCorrectActionSound();
            print(this.tag + "_" + individualMat);
            questControl.AddCorrectObject(this.tag + "_" + individualMat);
            grabbableScript.enabled = false; // deactivate ability to be grabbed
        }
    }

    private void PlayCorrectActionSound()
    {
        audioSource.clip = correctActionAudio;
        audioSource.pitch = Time.timeScale;
        audioSource.volume = 0.1f;
        if (!soundPlayed) {
            audioSource.Play();
            soundPlayed = true;
        }
        StartCoroutine("LockPosition");
    }

    // make sure objects stop moving inside bowl
    private IEnumerator LockPosition() { 
        yield return new WaitForSeconds(2.0f);
        this_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        yield return null;
    }
}
