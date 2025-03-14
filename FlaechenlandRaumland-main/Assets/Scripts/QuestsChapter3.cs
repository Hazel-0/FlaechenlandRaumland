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
    private LookingAtAxisCheck lookingAtAxisCheck_script;

    public GameObject square;
    private Animator squareAnimator;
    public GameObject triangle;
    private Animator triangleAnimator;
    public GameObject circle;
    private Animator circleAnimator;
    public GameObject sphere;
    private Animator sphereAnimator;

    // for checking if player moves joystick
    private GameObject joystickCollider;
    private GameObject mainCameraInScene;

    // to play door + background audio
    private AudioSource backgroundMusic;

    // for triggering next action
    private bool mainCameraTriggerTouched = false;
    public bool waitingForMainCameraTrigger = false;
    private bool lookingAtSquare = false;
    private bool lookingAtAxis = false;
    private bool standingNearSquare = false;
    private bool playerDucking = false;
    //public bool readyToDuck = false;

    private GameObject triggerObject;

    // für Kapitelende
    [SerializeField]
    private SceneControl sceneControlScript;
    [SerializeField]
    private GameObject trigger_changeScene;

    void Start()
    {
        AudioSetup();

        // Make sphere invisible
        sphere.SetActive(false);

        squareAnimator = square.GetComponent<Animator>();
        triangleAnimator = triangle.GetComponent<Animator>();
        circleAnimator = circle.GetComponent<Animator>();
        sphereAnimator = sphere.GetComponent<Animator>();

        joystickCollider = GameObject.Find("JoystickCollider");
        mainCameraInScene = GameObject.Find("Main Camera");

        triggerObject = GameObject.Find("Trigger_Square");
        triggerObject.SetActive(true);

        // find scripts and disable to prevent instant triggering
        lookingAtSquareCheck_script = GameObject.Find("Main Camera").GetComponent<lookingAtSquareCheck>();
        lookingAtSquareCheck_script.enabled = false;

        lookingAtAxisCheck_script = GameObject.Find("Main Camera").GetComponent<LookingAtAxisCheck>();
        lookingAtAxisCheck_script.enabled = false;


        trigger_changeScene.SetActive(false);
        if (sceneControlScript == null)
        {
            Debug.LogError("no scene control script found");
        }

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
        yield return new WaitForSeconds(17.5f);
        squareAnimator.SetTrigger("WaitInside");

        // set collider to player position
        joystickCollider.GetComponent<Transform>().position = mainCameraInScene.GetComponent<Transform>().position;
        
        // only continue if player moves outside controller
        waitingForMainCameraTrigger = true;


        while (!mainCameraTriggerTouched)
        {
            yield return null;
        }

        waitingForMainCameraTrigger = false;
        yield return new WaitForSeconds(2.0f);

        // continues talking
        squareAnimator.SetTrigger("TalkInside");
        audioClips_Square[2].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(6.0f);
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
            // Debug.Log("Not near square");
       }

        Debug.Log("Standing near square");
        
        // square talking
        squareAnimator.SetTrigger("TalkOutside");
        audioClips_Square[3].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(18.0f);
        squareAnimator.SetTrigger("IdleOutside");

        // triangle talking
        yield return new WaitForSeconds(2.0f);
        triangleAnimator.SetTrigger("TalkOutside");
        audioClips_Triangle[0].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3.0f);
        triangleAnimator.SetTrigger("IdleOutside");

        // circle talking
        yield return new WaitForSeconds(2.0f);
        circleAnimator.SetTrigger("TalkOutside");
        audioClips_Circle[0].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(13.0f);
        circleAnimator.SetTrigger("IdleOutside");

        // Square talking
        yield return new WaitForSeconds(1.0f);
        squareAnimator.SetTrigger("TalkOutside");
        audioClips_Square[4].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3.0f);
        squareAnimator.SetTrigger("IdleOutside");

        // Triangle turns around
        yield return new WaitForSeconds(3.0f);
        triangleAnimator.SetTrigger("TurnAround");

        lookingAtAxisCheck_script.enabled = true;
        sphere.SetActive(true);

        // is set to true by AxisHit method (used by lookingAtAxisCheck on MainCamera)
        while (!lookingAtAxis)
        {
            yield return new WaitForSeconds(2.0f);
            // check again
            if (!lookingAtAxis)
            {
                yield return new WaitForSeconds(2.0f);
            }
            // check again
            if (!lookingAtAxis)
            {
                yield return new WaitForSeconds(2.0f);
                audioClips_Square[4].GetComponent<AudioSource>().Play();
            }
        }

        // Sphere transitions through flatland
        audioClips_Square[4].GetComponent<AudioSource>().Stop();
        sphereAnimator.SetTrigger("Transition");
        audioClips_Sphere[0].GetComponent<AudioSource>().Play();

        // Square talking
        yield return new WaitForSeconds(1.0f);
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
        yield return new WaitForSeconds(1.0f);
        squareAnimator.SetTrigger("TalkOutside");
        audioClips_Square[6].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(7.0f);
        squareAnimator.SetTrigger("IdleOutside");

        // Sphere Talking
        yield return new WaitForSeconds(1.0f);
        sphereAnimator.SetTrigger("Talk");
        audioClips_Sphere[2].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(13.0f);
        sphereAnimator.SetTrigger("Wait");

        // Square Talking
        yield return new WaitForSeconds(1.0f);
        squareAnimator.SetTrigger("TalkOutside");
        audioClips_Square[7].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(11.0f);
        squareAnimator.SetTrigger("IdleOutside");

        // Sphere Talking
        yield return new WaitForSeconds(1.0f);
        sphereAnimator.SetTrigger("Talk");
        audioClips_Sphere[3].GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(13.0f);
        sphereAnimator.SetTrigger("Wait");
        yield return new WaitForSeconds(2.0f);
        sphereAnimator.SetTrigger("Leave");
        audioClips_Sphere[0].GetComponent<AudioSource>().Play();

        // WaitForPlayerDucking();
        //readyToDuck = true;
        //Debug.Log("Ready to duck");

        trigger_changeScene.SetActive(true);

        // Waiting for player to duck
        while (!playerDucking)
        {
            yield return new WaitForSeconds(6.0f); ;
            audioClips_Sphere[4].GetComponent<AudioSource>().Play();
        }

        Debug.Log("scene finished");
        yield return new WaitForSeconds(2.0f);
        sceneControlScript.enabled = true;
    }

    public void PlayerIsDucking()
    {
        playerDucking = true;
    }

    public void SquareHit()
    {
        lookingAtSquare = true;
    }

    public void AxisHit()
    {
        lookingAtAxis = true;
    }

    public void StandingNearSquare()
    {
        standingNearSquare = true;
    }

    public bool FirstTriggerDone()
    {
        return lookingAtSquare;
    }

    public bool SecondTriggerDone()
    {
        return lookingAtAxis;
    }

    public void MainCameraTriggerTouched()
    {
        mainCameraTriggerTouched = true;
    }
}
