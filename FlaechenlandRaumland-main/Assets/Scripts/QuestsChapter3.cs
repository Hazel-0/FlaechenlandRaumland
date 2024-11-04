using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class QuestsChapter3 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] audioClips_Square;
    [SerializeField]
    private GameObject[] audioClips_Triangle;
    [SerializeField]
    private GameObject[] audioClips_Circle;
    [SerializeField]
    private GameObject[] audioClips_Sphere;

    private lookingAtSquareCheck lookingAtSquareCheck_script;

    public GameObject square;
    private Animator squareAnimator;
    public GameObject triangle;
    private Animator triangleAnimator;
    public GameObject circle;
    private Animator circleAnimator;
    public GameObject sphere;
    private Animator sphereAnimator;

    // to play door + background audio
    private AudioSource backgroundMusic;

    // for triggering next action
    private bool lookingAtSquare = false;
    private bool standingNearSquare = false;
    private bool playerDucking;

    private GameObject triggerObject;

    // find XR controllers and input devices
    private UnityEngine.XR.Interaction.Toolkit.ActionBasedController leftHandController;
    private UnityEngine.XR.InputDevice leftInputDevice;

    void Start()
    {
        AudioSetup();

        // Make sphere invisible
        sphere.SetActive(false);

        squareAnimator = square.GetComponent<Animator>();
        triangleAnimator = triangle.GetComponent<Animator>();
        circleAnimator = circle.GetComponent<Animator>();
        sphereAnimator = sphere.GetComponent<Animator>();

        triggerObject = GameObject.Find("Trigger_Square");
        triggerObject.SetActive(true);

        // find script and disable to prevent instant triggering of square
        lookingAtSquareCheck_script = GameObject.Find("Main Camera").GetComponent<lookingAtSquareCheck>();
        lookingAtSquareCheck_script.enabled = false;

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
        audioClips_Square[0].GetComponent<AudioSource>().Play();
        // make it possible to trigger square for turning around
        yield return new WaitForSeconds(4.0f);
        lookingAtSquareCheck_script.enabled = true;


        // is set to true by SquareHit method (used by lookingAtSquareCheck on MainCamera)
        while (!lookingAtSquare)
        {
            yield return null;
        }

        // Square stops singing
        audioClips_Square[0].GetComponent<AudioSource>().Stop();

        // Square turns around
        yield return new WaitForSeconds(2.0f);
        squareAnimator.SetTrigger("TalkInside");
        triggerObject.SetActive(false);

        // starts talking
        yield return new WaitForSeconds(3.0f);
        audioClips_Square[1].GetComponent<AudioSource>().Play();

        // waits for joystick test
        yield return new WaitForSeconds(16.0f);
        squareAnimator.SetTrigger("WaitInside");
        yield return new WaitForSeconds(6.0f);

        // continues talking
        squareAnimator.SetTrigger("TalkInside");
        audioClips_Square[2].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(8.0f);
        squareAnimator.SetTrigger("WaitInside");

        // leaves house
        yield return new WaitForSeconds(1.0f);
        squareAnimator.SetTrigger("GoOutside");

        // triangle and circle join them
        yield return new WaitForSeconds(5.0f);
        triangleAnimator.SetTrigger("GoOutside");
        circleAnimator.SetTrigger("GoOutside");

       while (!standingNearSquare) { 
            yield return null;
            Debug.Log("Not near square");
       }

        Debug.Log("Standing near square");
        
        // square talking
        squareAnimator.SetTrigger("TalkOutside");
        audioClips_Square[3].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(18.0f);
        squareAnimator.SetTrigger("IdleOutside");

        // triangle talking
        triangleAnimator.SetTrigger("TalkOutside");
        audioClips_Triangle[0].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(5.0f);
        triangleAnimator.SetTrigger("IdleOutside");

        // circle talking
        circleAnimator.SetTrigger("TalkOutside");
        audioClips_Circle[0].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(15.0f);
        circleAnimator.SetTrigger("IdleOutside");

        // Square talking
        squareAnimator.SetTrigger("TalkOutside");
        audioClips_Square[4].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3.0f);
        squareAnimator.SetTrigger("IdleOutside");

        // Sphere transitions through flatland
        sphere.SetActive(true);
        sphereAnimator.SetTrigger("Transition");
        audioClips_Sphere[0].GetComponent<AudioSource>().Play();

        // Square talking
        yield return new WaitForSeconds(2.0f);
        squareAnimator.SetTrigger("TalkOutside");
        audioClips_Square[5].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(4.0f);
        squareAnimator.SetTrigger("IdleOutside");

        // Sphere Talking
        yield return new WaitForSeconds(4.0f);
        sphereAnimator.SetTrigger("Talk");
        audioClips_Sphere[1].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(7.0f);
        sphereAnimator.SetTrigger("Wait");

        // Square Talking
        yield return new WaitForSeconds(2.0f);
        squareAnimator.SetTrigger("TalkOutside");
        audioClips_Square[6].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(7.0f);
        squareAnimator.SetTrigger("IdleOutside");

        // Sphere Talking
        yield return new WaitForSeconds(2.0f);
        sphereAnimator.SetTrigger("Talk");
        audioClips_Sphere[2].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(13.0f);
        sphereAnimator.SetTrigger("Wait");

        // Square Talking
        yield return new WaitForSeconds(2.0f);
        squareAnimator.SetTrigger("TalkOutside");
        audioClips_Square[7].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(11.0f);
        squareAnimator.SetTrigger("IdleOutside");

        // Sphere Talking
        yield return new WaitForSeconds(2.0f);
        sphereAnimator.SetTrigger("Talk");
        audioClips_Sphere[3].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(13.0f);
        sphereAnimator.SetTrigger("Leave");

        WaitForPlayerDucking();

        // Waiting for player to duck
        while (!playerDucking)
        {
            yield return new WaitForSeconds(6.0f); ;
            audioClips_Sphere[4].GetComponent<AudioSource>().Play();
        }

        yield return new WaitForSeconds(2.0f);
        Debug.Log("scene finished");
        // TODO: Implement Scene Transition - see Chapter1 Restart Game
    }

    public void WaitForPlayerDucking()
    {
        playerDucking = true;
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
