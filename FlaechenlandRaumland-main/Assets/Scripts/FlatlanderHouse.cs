using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatlanderHouse : MonoBehaviour
{
    [SerializeField]
    private SceneControl sceneControl;
    private bool sceneChangeActivated = false;

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
        Debug.Log("scene change activated is " + sceneChangeActivated);

        if (other.name == "GrabVolumeBig" && sceneChangeActivated)
        {
            Debug.Log("Grab volume entered house");
            sceneControl.enabled = true;
        }
    }

    public void ActivateSceneChange()
    {
        if (!sceneChangeActivated)
        {
            sceneChangeActivated = true;
        }
    }
}
