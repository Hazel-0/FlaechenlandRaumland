using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaceInsidePlate : MonoBehaviour
{
    [SerializeField] Material neutralMat;
    [SerializeField] Material individualMat;
    public AudioClip correctActionAudio;
    AudioSource audioSource;
    XRGrabInteractable grabbableScript;

    private Quests questControl;

    void Start()
    {
        individualMat = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material = neutralMat;

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
        }

        grabbableScript = GetComponent<XRGrabInteractable>();
        // grabbableScript.enabled = true;

        // get quest controller
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        questControl = gameManager.GetComponent<Quests>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Plate" && this.tag == "2D")
        {
            GetComponent<Renderer>().material = individualMat;
            PlayCorrectActionSound();
            questControl.AddCorrectObject(this.tag + "_" + individualMat);
            if (grabbableScript != null)
            {
                grabbableScript.enabled = false; // deactivate ability to be grabbed
            }
        }
    }

    private void PlayCorrectActionSound()
    {
        if (audioSource != null)
        {
            audioSource.clip = correctActionAudio;
            audioSource.pitch = Time.timeScale;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
    }
}
