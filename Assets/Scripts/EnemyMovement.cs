using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D enemyRigidbody;
    CapsuleCollider2D enemyCheckEdgeCollider;
    [SerializeField] float moveSpeed = 2f;
    void Awake() {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyCheckEdgeCollider = GetComponent<CapsuleCollider2D>();
    }
    void Start() {
        
    }

    void Update() {
        enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
        
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Platforms")) {
            moveSpeed = -moveSpeed;
            transform.localScale = new Vector2(Mathf.Sign(moveSpeed), transform.localScale.y);
        }
    }
}
