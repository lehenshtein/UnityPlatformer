using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int coins = 0;
    void Awake() {
        //Make gameSession a singleton
        //it instatiates on play after scene reset or going to next scene
        //so we will destroy each new instance
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1) {
            Destroy(gameObject);
        } else {
            //not destroy recent game object after scene reloads
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        livesText.text = lives.ToString();
        scoreText.text = coins.ToString();
    }

    public void ProcessPlayerDeath() {
        if (lives > 1) {
            TakeLife();
        } else {
            ResetGameSession();
        }
    }

    void TakeLife() {
        lives--;
        livesText.text = lives.ToString();
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }

    void ResetGameSession() {
        if (FindObjectOfType<ScenePersist>() != null) {
            FindObjectOfType<ScenePersist>().DestroyAfterNewLevel();
        }
        // FindObjectOfType<ScenePersist>().DestroyAfterNewLevel();
        
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void IncreaseCoins() {
        coins++;
        scoreText.text = coins.ToString();
    }
}
