using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flatland : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Expand()
    {
        animator.SetTrigger("ExpandFlatland");
    }
}
