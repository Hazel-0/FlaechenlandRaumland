using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsChapter4 : MonoBehaviour
{
    // to play instructions
    [SerializeField]
    private GameObject[] audioClips;

    public GameObject hypersphere;
    private Animator hypersphereAnimator;

    // to trigger hypersphere appearing
    private GameObject triggerHypersphere;
    private bool lookingAtDoor = false;

    // to play door + background audio
    private AudioSource backgroundMusic;
    private AudioSource doorClosesAudio;

    void Start()
    {
        triggerHypersphere = GameObject.Find("Trigger_Hypersphere");
        triggerHypersphere.SetActive(false);

        hypersphereAnimator = hypersphere.GetComponent<Animator>();
        AudioSetup();
        StartCoroutine(QuestLine());
    }

    private void AudioSetup()
    {
        doorClosesAudio = GameObject.Find("(00) DoorAudio").GetComponent<AudioSource>();
        backgroundMusic = GameObject.Find("# Background Music").GetComponent<AudioSource>();
        backgroundMusic.Stop();
    }

    IEnumerator QuestLine()
    {
        doorClosesAudio.Play();
        // wait for 2 seconds, then play background music initiation
        yield return new WaitForSeconds(2.0f);
        backgroundMusic.Play();

        // sphere talking
        yield return new WaitForSeconds(2.0f);
        audioClips[0].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(24.0f);

        // start animating when player is looking at door
        triggerHypersphere.SetActive(true);
        while (!lookingAtDoor)
        {
            yield return null;
            Debug.Log("Not looking at door");
        }

        // Hypersphere transition
        hypersphereAnimator.SetTrigger("Transition");
        hypersphere.GetComponent<AudioSource>().Play();

        // sphere talking
        yield return new WaitForSeconds(15.0f);
        audioClips[1].GetComponent<AudioSource>().Play();
    }

    public void LookingAtDoor()
    {
        lookingAtDoor = true;
    }
}
