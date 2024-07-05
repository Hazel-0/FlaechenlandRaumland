using UnityEngine;
using System.Collections;

public class RespawnSphere : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private Quests_Intro questControl;

    // Start is called before the first frame update
    void Start()
    {
        // get quest controller
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        questControl = gameManager.GetComponent<Quests_Intro>();
        StartCoroutine("SaveTransformPosition");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentTransformPosition();
        // nur prüfen wenn kugelRespawnen von Controller ausgelöst wird (Aufgabe: Kugel in Schüssel)
        if (questControl.kugelRespawnen && (currentPosition == previousPosition) && (currentPosition != startPosition))
        {
            RestoreTransformPosition();
        }
    }
    private void RestoreTransformPosition()
    {
        transform.position = startPosition;
    }

    private void UpdateCurrentTransformPosition()
    {
        previousPosition = currentPosition;
        currentPosition = transform.position;
    }

    private IEnumerator SaveTransformPosition()
    {
        /** Willkommen */
        // wait for 5 seconds, then start initiation
        yield return new WaitForSeconds(5.0f);
        startPosition = transform.position;
        yield return null;
    }
}
