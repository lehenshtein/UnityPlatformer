using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    [SerializeField] float bulletSpeed = 13f;
    float dissapearTimer = 3f;
    PlayerMovement player;
    float xSpeed;
    bool gotCollision = false;
    void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        float arrowSide = transform.localScale.y;
        if (player.transform.localScale.x < 0) {
            arrowSide = -arrowSide;
        }
        transform.localScale = new Vector2(transform.localScale.x, arrowSide);
    }
    void Start() {
        
    }

    void Update() {
        if (!gotCollision) {
            myRigidbody.velocity = new Vector2(xSpeed, 0f);
        } else {
            myRigidbody.velocity = new Vector2(0f, 0f);
            myRigidbody.gravityScale = 0;
        }
        
        UpdateTimer();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);

        // counter += Time.deltaTime;
        // if (counter > dissapearTimer) {
        //     Destroy(gameObject);
        // }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }
        gotCollision = true;
    }

    void UpdateTimer() {
        if (gotCollision) {
            dissapearTimer -= Time.deltaTime;
            if (dissapearTimer < 0) {
                Destroy(gameObject);
        }
        }
    }
}
