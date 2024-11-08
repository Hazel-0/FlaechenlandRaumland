using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFloor : MonoBehaviour
{
    private GameObject mainCamera;
    private float mainCamera_x;
    private float mainCamera_z;
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        mainCamera_x = mainCamera.transform.position.x; 
        mainCamera_z = mainCamera.transform.position.z;

        // set position of floor to main camera
        transform.position = new Vector3(mainCamera_x,
            transform.position.y, mainCamera_z);
    }
}
