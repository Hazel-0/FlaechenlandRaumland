using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giesskanne : MonoBehaviour {
    [Header("Gieﬂkanne")]
    [SerializeField]
    private float minFillLevel = 0;
    [SerializeField]
    private GameObject ausguss;
    [SerializeField]
    private GameObject water_lvl;
    [SerializeField]
    private ParticleSystem partSys;

    [Header("Physik")]
    [SerializeField]
    private float flowSpeed = 3;
    [SerializeField]
    private float drainRate = 1;

    private float initialWaterLevel = 1;
    private float minWaterLevel = 0;

    bool giessenErlaubt = false;
    private bool erstesMalAufgehoben = false;
    private GiessenAudio giessenAudio;

    // Start is called before the first frame update
    void Start() {
        partSys.Pause();
        initialWaterLevel = water_lvl.transform.localPosition.y;
        minWaterLevel = initialWaterLevel * minFillLevel;
        giessenAudio = GameObject.Find("Scripts").GetComponent<GiessenAudio>();
    }

    // Update is called once per frame
    void Update() {
        float neigung = getNeigung();

        var emission = partSys.emission;
        var main = partSys.main;
        float wasserstand = (water_lvl.transform.localPosition.y / initialWaterLevel);
        if (wasserstand < 0) {
            wasserstand = 0;
        }
        float flowrate = neigung * wasserstand;
        // Debug.Log("flowrate: " + flowrate);
        if (flowrate > 0) {
            emission.rateOverTime = 300 * flowrate;
            main.startSpeed = flowSpeed * flowrate;
            drainWater(flowrate);
        } else {
            emission.rateOverTime = 0;
            main.startSpeed = 0;
        }
        //Debug.Log(emission.rateOverTime);
        //Debug.Log(emission.enabled);
    }

    private float getNeigung() {
        float distance = Vector3.Distance(ausguss.transform.position, water_lvl.transform.position);
        float raw = water_lvl.transform.position.y - ausguss.transform.position.y;
        float neigung = 0;
        if (raw > 0) {
            neigung = raw / distance;
        }
        return neigung;
    }

    private void drainWater(float flowrate) {
        float drain = flowrate * Time.deltaTime * drainRate;
        if (water_lvl.transform.localPosition.y > 0) {
            water_lvl.transform.Translate(new Vector3(0, -1, 0) * drain);
        }
    }

    public void pickup() {
        // um Audio "Pflanze 1 gieﬂen abzuspielen"
        if (!erstesMalAufgehoben) { 
            giessenAudio.GiesskanneGegriffenAudio();
        }
        erstesMalAufgehoben = true;
        giessenErlaubt = true;
        StartCoroutine(Giessen());
    }

    public void drop() {
        giessenErlaubt = false;
        partSys.Pause();
    }

    IEnumerator Giessen() {
        yield return new WaitForSeconds(1f);
        if (giessenErlaubt) {
            partSys.Play();
        }
        yield return null;
    }

}
