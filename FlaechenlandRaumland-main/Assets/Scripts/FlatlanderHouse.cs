using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatlanderHouse : MonoBehaviour
{
    [SerializeField]
    private SceneControl sceneControl;

    private void Start()
    {
        if (sceneControl == null)
        {
            sceneControl = GameObject.Find("Scripts").GetComponent<SceneControl>();
        }
        if (sceneControl.enabled == true)
        {
            sceneControl.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "GrabVolumeBig")
        {
            Debug.Log("Grab volume entered house");
            sceneControl.enabled = true;
        }
    }
}
