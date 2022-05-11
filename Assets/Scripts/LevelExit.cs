using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    float delayTime = 1f;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(GotToExit());
        }
    }

    IEnumerator GotToExit() {
        yield return new WaitForSecondsRealtime(delayTime);

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = sceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }

        if (FindObjectOfType<ScenePersist>() != null) {
            FindObjectOfType<ScenePersist>().DestroyAfterNewLevel();
        }
        // FindObjectOfType<ScenePersist>().DestroyAfterNewLevel();
        SceneManager.LoadScene(nextSceneIndex);
    }
        
}
