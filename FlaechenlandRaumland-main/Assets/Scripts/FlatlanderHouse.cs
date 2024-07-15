using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatlanderHouse : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "GrabVolumeBig")
        {
            // TODO: continue once axis tracking works
            Debug.Log("Grab volume entered house");
        }
    }
}
