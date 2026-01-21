using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // ============================================
    // VARIABILI
    // ============================================
    public float speed = 1f;
    private Rigidbody2D _ballRigidBody;

    // ============================================
    // LIFECYCLE
    // ============================================
    void Start()
    {
        _ballRigidBody = GetComponent<Rigidbody2D>();
        _ballRigidBody.velocity = new Vector2(-0.5f, speed);
    }

    void Update()
    {
        // Velocità controllata da OnTriggerEnter2D
    }

    // ============================================
    // COLLISIONI
    // ============================================
    void OnTriggerEnter2D(Collider2D collision)
    {
        // ✅ PRIMO CONTROLLO: Collisione con i limiti
        if (collision.tag == "Limit")
        {
            Debug.Log("Collided with a limit");

            // Rimbalzo sul limite superiore (Top Limit)
            if (collision.transform.position.y > transform.position.y && _ballRigidBody.velocity.y > 0)
            {
                _ballRigidBody.velocity = new Vector2(
                    _ballRigidBody.velocity.x,
                    -_ballRigidBody.velocity.y
                );
            }

            // Rimbalzo sul limite inferiore (Bottom Limit)
            if (collision.transform.position.y < transform.position.y && _ballRigidBody.velocity.y < 0)
            {
                _ballRigidBody.velocity = new Vector2(
                    _ballRigidBody.velocity.x,
                    -_ballRigidBody.velocity.y
                );
            }
        }
        // ✅ SECONDO CONTROLLO: Collisione con il paddle (FUORI dal primo if)
        else if (collision.tag == "Paddle")
        {
            Debug.Log("Collided with paddle!");

            // Rimbalzo sulla racchetta sinistra (Left Paddle)
            if (collision.transform.position.x < transform.position.x && _ballRigidBody.velocity.x < 0)
            {
                _ballRigidBody.velocity = new Vector2(
                    -_ballRigidBody.velocity.x,
                    _ballRigidBody.velocity.y
                );
            }

            // Rimbalzo sulla racchetta destra (Right Paddle)
            if (collision.transform.position.x > transform.position.x && _ballRigidBody.velocity.x > 0)
            {
                _ballRigidBody.velocity = new Vector2(
                    -_ballRigidBody.velocity.x,
                    _ballRigidBody.velocity.y
                );
            }
        }
    }
}