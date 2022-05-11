using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinSFX;
    bool wasCollected = false;
   private void OnTriggerEnter2D(Collider2D other) {
       if (other.CompareTag("Player") && !wasCollected) {
           wasCollected = true;
           AudioSource.PlayClipAtPoint(coinSFX, Camera.main.transform.position, .5f);
           Destroy(gameObject);
           gameObject.SetActive(false);
           FindObjectOfType<GameSession>().IncreaseCoins();
       }
   }
}
