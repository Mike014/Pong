using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // ============================================
    // VARIABILI
    // ============================================
    public float difficultMultiplier = 1.3f;
    public float minXSpeed = 0.8f;
    public float maxXSpeed = 1.2f;
    public float minYSpeed = 0.8f;
    public float maxYSpeed = 1.2f;
    private Rigidbody2D _ballRigidBody;
    private AudioSource _audioSource;

    // ============================================
    // LIFECYCLE
    // ============================================
    void Start()
    {
        _ballRigidBody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _ballRigidBody.velocity = new Vector2
        (Random.Range(minXSpeed, maxXSpeed) * (Random.value > 0.5f ? -1 : 1), Random.Range(minYSpeed, maxYSpeed) * (Random.value > 0.5f ? -1 : 1));
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
            PlaySound();

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
            PlaySound();

            // Rimbalzo sulla racchetta sinistra (Left Paddle)
            if (collision.transform.position.x < transform.position.x && _ballRigidBody.velocity.x < 0)
            {
                _ballRigidBody.velocity = new Vector2(
                    -_ballRigidBody.velocity.x * difficultMultiplier,
                    _ballRigidBody.velocity.y * difficultMultiplier
                );
            }

            // Rimbalzo sulla racchetta destra (Right Paddle)
            if (collision.transform.position.x > transform.position.x && _ballRigidBody.velocity.x > 0)
            {
                _ballRigidBody.velocity = new Vector2(
                    -_ballRigidBody.velocity.x * difficultMultiplier,
                    _ballRigidBody.velocity.y * difficultMultiplier
                );
            }
        }
    }

    // ============================================
    // METODI PRIVATI
    // ============================================
    private void PlaySound()
    {
        if (_audioSource != null && _audioSource.clip != null)
        {
            _audioSource.Play();
        }
    }
}