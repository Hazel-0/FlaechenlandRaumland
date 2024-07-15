using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class QuestsChapter3 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] audioClips;

    public GameObject[] square;
    public GameObject[] triangle;
    public GameObject[] circle;

    // to play door + background audio
    private AudioSource backgroundMusic;

    void Start()
    {
        AudioSetup();
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
        // play initiation audio
        // audioClips[0].GetComponent<AudioSource>().Play();
    }
}
