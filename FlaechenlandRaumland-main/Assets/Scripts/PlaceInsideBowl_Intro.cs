using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaceInsideBowl_Intro : MonoBehaviour
{
    [SerializeField] Material neutralMat;
    [SerializeField] Material individualMat;
    public AudioClip correctActionAudio;
    AudioSource audioSource;
    XRGrabInteractable grabbableScript;
    [SerializeField] GameObject sphere;

    private Quests_Intro questControl; // changed from Quests (Chapter1) to Quests_Intro
    private Rigidbody this_Rigidbody;
    private bool soundPlayed;

    void Start() {
        individualMat = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material = neutralMat;

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
        }

        grabbableScript = GetComponent<XRGrabInteractable>();
        grabbableScript.enabled = true;

        // get quest controller
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        questControl = gameManager.GetComponent<Quests_Intro>(); // changed from Quests (Chapter1) to Quests_Intro

        soundPlayed = false;

        // get and set rigidbody
        this_Rigidbody = this.GetComponent<Rigidbody>();
        this_Rigidbody.constraints = RigidbodyConstraints.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bowl" && this.tag == "3D")
        {
            GetComponent<Renderer>().material = individualMat;
            PlayCorrectActionSound();
            sphere.GetComponent<XRGrabInteractable>().enabled = true;
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
        if (!soundPlayed)
        {
            audioSource.Play();
            soundPlayed = true;
        }
        StartCoroutine("LockPosition");
    }

    // make sure objects stop moving inside bowl
    private IEnumerator LockPosition()
    {
        yield return new WaitForSeconds(1.0f);
        // this_Rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        yield return null;
    }
}
