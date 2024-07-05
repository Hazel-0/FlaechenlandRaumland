using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTest : MonoBehaviour
{
    private Renderer rend;
    private Color initialColor;
    private Color targetColor;
    private Color lerpedColor;

    // Start is called before the first frame update
    void Start()
    {
        rend = transform.GetComponent<Renderer>();
        initialColor = rend.material.color;
        targetColor = new Color(0, 0, 0);
        //StartCoroutine("MakeBlack");
    }

    private void Update()
    {
        lerpedColor = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
        GetComponent<Renderer>().material.color = lerpedColor;
    }

    private IEnumerator MakeBlack() {
        yield return new WaitForSeconds(5.0f);
        rend.material.color = targetColor;
        yield return new WaitForSeconds(5.0f);
        rend.material.color = initialColor;
        yield return null;
    }
}
