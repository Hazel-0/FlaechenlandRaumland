using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowCoordinateSystem : MonoBehaviour
{
    private Quests quests;
    private GameObject coordinateSystem;
    private Animator animator;
    [SerializeField]
    private GameObject[] coordinateSystemParts;

    private bool soundPlayed1 = false;
    private bool soundPlayed2 = false;
    private bool kugelBeruehrt = false;
    private bool sphereReturns = false;
    private GameObject room;
    private AudioSource warblingAudio;
    private AudioSource roomWarpAudio;
    //private bool compressArrowStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        quests = GameObject.Find("Scripts").GetComponent<Quests>();
        coordinateSystem = GameObject.Find("CoordinateSystem");
        coordinateSystem.SetActive(false);
        animator = GetComponent<Animator>();
        animator.enabled = true;
        room = GameObject.Find("room");
        room.SetActive(true);

        warblingAudio = GameObject.Find("WarblingAudio").GetComponent<AudioSource>();
        StartCoroutine(WaitUntilReturn());

        roomWarpAudio = GameObject.Find("Pop Sound").GetComponent<AudioSource>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hand" && quests.lichtWiederAn && !kugelBeruehrt)
        {
            kugelBeruehrt = true;
            animator.SetTrigger("MoveToOrigin");
            if (!soundPlayed1)
            {
                warblingAudio.Play();
                soundPlayed1 = true;
            }
            StartCoroutine(WaitUntilReturn());
        }
    }

    private IEnumerator WaitUntilReturn()
    {
        while (!kugelBeruehrt) {
            yield return null;
        }
        yield return new WaitForSeconds(5.0f);
        quests.PlayAudio(5);



        coordinateSystem.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        room.SetActive(false);
        roomWarpAudio.Play();
        yield return new WaitForSeconds(4.0f);
        quests.PlayAudio(6);

        SetCSTrigger("stretchArrow");
        yield return new WaitForSeconds(14.0f);
        animator.SetTrigger("MoveFromOriginToTable");
        SetCSTrigger("compressArrow");
        if (!soundPlayed2)
        {
            warblingAudio.Play();
            soundPlayed2 = true;
        }
        yield return new WaitForSeconds(6.0f);
        coordinateSystem.SetActive(false);
        quests.KoodinatenSystemFertig();

        room.SetActive(true);
        roomWarpAudio.Play();

        animator.enabled = false;
        yield return null;
        
    }

    // set trigger for all parts of the coordinate system (arrow + arrow head)
    private void SetCSTrigger(string triggerName)
    {
        for (int i = 0; i < coordinateSystemParts.Length; i++)
        {
            coordinateSystemParts[i].GetComponent<Animator>().SetTrigger(triggerName);
            Debug.Log("Set Trigger " + triggerName);
        }
        // audioSource = GameObject.Find("WarblingAudio").GetComponent<AudioSource>();
    }
}
