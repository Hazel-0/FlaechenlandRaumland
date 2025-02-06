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
    //private bool soundPlayed2 = false;
    private bool kugelBeruehrt = false;
    private bool sphereReturns = false;
    private GameObject room;
    [SerializeField]
    private AudioSource warblingAudio;
    [SerializeField]
    private AudioSource roomWarpAudio;

    void Start()
    {
        quests = GameObject.Find("Scripts").GetComponent<Quests>();
        coordinateSystem = GameObject.Find("CoordinateSystem");
        coordinateSystem.SetActive(false);
        animator = GetComponent<Animator>();
        animator.enabled = true;
        room = GameObject.Find("room");
        room.SetActive(true);

        //StartCoroutine(WaitUntilReturn());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand" && quests.lichtWiederAn && !kugelBeruehrt)
        {
            kugelBeruehrt = true;
            animator.SetTrigger("MoveToOrigin");
            if (!soundPlayed1)
            {
                warblingAudio.GetComponent<AudioSource>().Play();
                soundPlayed1 = true;
            }
            StartCoroutine(WaitUntilReturn());
        }
    }

    private IEnumerator WaitUntilReturn()
    {
        room.SetActive(false);
        roomWarpAudio.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(5.0f);
        quests.PlayAudio(5);

        yield return new WaitForSeconds(4.0f);
        coordinateSystem.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        quests.PlayAudio(6);

        SetCSTrigger("stretchArrow");
        yield return new WaitForSeconds(14.0f);
        animator.SetTrigger("MoveFromOriginToTable");
        warblingAudio.GetComponent<AudioSource>().Play();
        SetCSTrigger("compressArrow");

        yield return new WaitForSeconds(7.0f);
        coordinateSystem.SetActive(false);
        quests.KoodinatenSystemFertig();

        room.SetActive(true);
        roomWarpAudio.GetComponent<AudioSource>().Play();

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
    }
}
