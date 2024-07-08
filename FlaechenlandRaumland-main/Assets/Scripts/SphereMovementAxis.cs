using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SphereMovementAxis : MonoBehaviour
{
    Animator[] axisAnimators;

    void Start()
    {
        axisAnimators = GetComponentsInChildren<Animator>();
        Debug.Log("number of axis animators: " + axisAnimators.Length);
    }

    public void ActivateAxes() {
        Debug.Log("Activate sphere movement axis");
        foreach (Animator axisAnimator in axisAnimators)
        {
            axisAnimator.SetTrigger("StretchAxis");
        }
    }
}
