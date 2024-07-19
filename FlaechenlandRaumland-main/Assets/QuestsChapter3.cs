using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class QuestsChapter3 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] audioClips_Square;
    private GameObject[] audioClips_Triangle;
    private GameObject[] audioClips_Circle;
    private GameObject[] audioClips_Sphere;

    public GameObject square;
    private Animator squareAnimator;
    public GameObject triangle;
    private Animator triangleAnimator;
    public GameObject circle;
    private Animator circleAnimator;

    // to play door + background audio
    private AudioSource backgroundMusic;

    // for triggering next action
    private bool lookingAtSquare = false;
    private bool standingNearSquare = false;

    private GameObject triggerObject;

    void Start()
    {
        AudioSetup();
        squareAnimator = square.GetComponent<Animator>();
        triangleAnimator = triangle.GetComponent<Animator>();
        circleAnimator = circle.GetComponent<Animator>();
        triggerObject = GameObject.Find("Trigger_Squre");
        triggerObject.SetActive(true);

        StartCoroutine(QuestLine());
    }

    private void AudioSetup()
    {
        backgroundMusic = GameObject.Find("# Background Music").GetComponent<AudioSource>();
        backgroundMusic.Stop();
    }

    IEnumerator QuestLine()
    {
        // wait for 2 seconds, then play background music initiation
        yield return new WaitForSeconds(2.0f);
        backgroundMusic.Play();
        yield return new WaitForSeconds(2.0f);
        while (!lookingAtSquare)
        {
            yield return null;
        }


        squareAnimator.SetTrigger("TalkInside");
        triggerObject.SetActive(false);

        // play first audio
        // audioClipsSquare[0].GetComponent<AudioSource>().Play();

        // squareAnimator.SetTrigger("GoOutside");
        // yield return new WaitForSeconds(3.0f);
        // triggerObject.SetActive(true);

        // triggerObject.SetActive(false);

        /*while (!standingNearSquare) { 
            yield return null; 
        }*/

        // TODO trigger for other flatlanders coming out, keep talking

    }

    public void SquareHit()
    {
        lookingAtSquare = true;
    }

    public void StandingNearSquare()
    {
        standingNearSquare = true;
    }

    public bool FirstTriggerDone()
    {
        return lookingAtSquare;
    }
}
