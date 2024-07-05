using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleScript : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    public GameObject scripts;

    private Quests quests;
    private GiessenAudio giessenAudio;

    private bool plant1 = false;
    private bool plant2 = false;

    /*private int topf1Count = 0;
    private int topf2Count = 0;

    private int grenze = 3;*/

    void Start() {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        quests = scripts.GetComponent<Quests>();
        giessenAudio = scripts.GetComponent<GiessenAudio>();
    }

    void OnParticleCollision(GameObject other) {
        //Debug.Log("plant particle collision");
        if (!plant1 || !plant2) {
            //Debug.LogWarning("Collision with " + other.name);
            if (other.name == "topf 1") {
                plant1 = true;
                Animator anim = other.GetComponentInChildren<Animator>();

                AudioSource audio = other.GetComponent<AudioSource>();
                anim.enabled = true;
                audio.enabled = true;
                if (!plant2) {
                    giessenAudio.PflanzeAudio(1);
                }
            }
            else if(other.name == "topf 2") {
                plant2 = true;
                Animator anim = other.GetComponentInChildren<Animator>();
                AudioSource audio = other.GetComponent<AudioSource>();
                anim.enabled = true;
                audio.enabled = true;
                if (!plant1) {
                    giessenAudio.PflanzeAudio(0);
                }
            }
            if (plant1 && plant2) {
                Debug.LogWarning("Giessen fertig!");
                quests.GiessenFertig();
            }
        }       

    }
}
