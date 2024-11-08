using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour {
    private bool initiated = false;
    public Animator animator;

    void Start() {
        GameObject sphere = GameObject.FindGameObjectWithTag("NoCollision");
        Physics.IgnoreCollision(sphere.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Executed once when script is activated by Sphere > XR Grab Interactable 
    // then initiated is set to false
    private void Update() {
        if (!initiated) {
            StartCoroutine(LoadAsyncScene());
            initiated = true;
        }
    }

    IEnumerator LoadAsyncScene() {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        animator.SetTrigger("Fade");
        yield return new WaitForSeconds(1.0f);

        int y = SceneManager.GetActiveScene().buildIndex;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(y + 1);
        Debug.LogWarning("LoadScene...beep...bup...");
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            Debug.LogWarning("...loading...");
            yield return null;
        }
    }
}
