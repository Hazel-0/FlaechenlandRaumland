using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustFlatland : MonoBehaviour
{
    public GameObject VRcamera;

    private float initialFlatlandY;
    private float initialFlatlandX;
    private float initialFlatlandZ;

    private float cameraY;
    void Start()
    {
        initialFlatlandY = gameObject.transform.localPosition.y;
        initialFlatlandX = gameObject.transform.localPosition.x;
        initialFlatlandZ = gameObject.transform.localPosition.z;

        AdjustFlatlandHeight();
    }

    void Update()
    {
        AdjustFlatlandHeight();
    }

    private void AdjustFlatlandHeight()
    {
        cameraY = VRcamera.transform.position.y;
        Debug.Log("camera y position is " + cameraY);
        this.transform.SetPositionAndRotation(new Vector3(initialFlatlandX, initialFlatlandY-cameraY, initialFlatlandZ), Quaternion.identity);
    }
}
