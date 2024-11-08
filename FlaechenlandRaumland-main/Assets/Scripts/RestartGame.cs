using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private InputAction menuInputActionReference;

    public Animator animator;

    private bool initiated = false;
    private bool restartGame = false;
    private bool restartAllowed = false;
    private void OnEnable() {
        menuInputActionReference.Enable();
    }

    private void OnDisable() {
        menuInputActionReference.Disable();
    }

    private void Update() {
        float val = menuInputActionReference.ReadValue<float>();
        if (val != 0) {
            restartGame = true;
        }
        if (restartGame && restartAllowed) {
            if (!initiated) {
                StartCoroutine(LoadAsyncScene());
                initiated = true;
            }
        }  
    }

    public void AllowRestart() {
        restartAllowed = true;
    }

    IEnumerator LoadAsyncScene() {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        animator.SetTrigger("Fade");
        yield return new WaitForSeconds(1.0f);

        int y = SceneManager.GetActiveScene().buildIndex;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        Debug.LogWarning("LoadScene...beep...bup...");
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            Debug.LogWarning("...loading...");
            yield return null;
        }
    }
}
