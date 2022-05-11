using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake() {
        //Make ScenePersist a singleton
        //it instatiates on play after scene reset or going to next scene
        //so we will destroy each new instance
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersist > 1) {
            Destroy(gameObject);
        } else {
            //not destroy recent game object after scene reloads
            DontDestroyOnLoad(gameObject);
        }
    }

    public void DestroyAfterNewLevel() {
        Debug.Log(gameObject);
        if (gameObject != null) {
            DestroyImmediate(gameObject);
        }
    }
}
