using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class HypersphereMovement : MonoBehaviour
{
    private Vector3 scaleChange;
    private float scaleChangeParameter = 0.04f;
    private float positionChangeParameter = 0.03f;
    private Vector3 startScale, startPosition;
    private float timer = 0.0f;
    void Start()
    {
        Debug.Log("HypersphereMovement active");
        startPosition = transform.position;
        Debug.Log("current position: " + startPosition);
        startScale = new Vector3(0f, 0f, 0f);
        transform.localScale = startScale;

        scaleChange = new Vector3(scaleChangeParameter, scaleChangeParameter, scaleChangeParameter);
    }

    void Update()
    {
        timer += Time.deltaTime;
        // Debug.Log("timer: " + timer);
        if (timer > 5.0f)
        {
            // increase size by adding vector
            transform.localScale += scaleChange;
            // slightly change position
            transform.localPosition += new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f)*positionChangeParameter, 
                UnityEngine.Random.Range(-1.0f, 1.0f) * positionChangeParameter, UnityEngine.Random.Range(-1.0f, 1.0f) * positionChangeParameter);
            // check for max size
            if (transform.localScale.y > 4.0f)
            {
                scaleChange = -scaleChange;
            }
            // set inactive if scale = 0
            else if (timer > 6.0f && transform.localScale.y < 0.1f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
