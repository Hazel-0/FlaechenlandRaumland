using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerChange : MonoBehaviour {

    [SerializeField] Material chosenMat;
    [SerializeField] Animator animator;

    private bool initiated = false;
    private bool reset = false;
    // Start is called before the first frame update
    void Start() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "bowl_l") {
            other.GetComponent<Renderer>().material = chosenMat;
            reset = true;
            initiated = true;
        } else if (other.tag == "Bowl") {
            other.GetComponent<Renderer>().material = chosenMat;
            initiated = true;

        }
    }

    // Update is called once per frame
    void Update() {
        if (initiated) {
            StartCoroutine(LoadAsyncScene());
        }
    }

    IEnumerator LoadAsyncScene() {
        if (reset) {
            animator.SetTrigger("Fade_slow");
            yield return new WaitForSeconds(10.0f);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        } else {
            animator.SetTrigger("Fade");
            yield return new WaitForSeconds(1.0f);

            int y = SceneManager.GetActiveScene().buildIndex;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(y + 1);
        }
        yield return null;
    }
}
