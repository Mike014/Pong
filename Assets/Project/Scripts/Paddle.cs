using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private Rigidbody2D rb;
    [SerializeField] private bool isAI = false;
    [SerializeField] private float aiReactionTime = 0.1f;
    [SerializeField] private float aiPredictionOffset = 0.5f;
    
    private Transform _ballTransform;
    private Rigidbody2D _ballRb;
    private float _aiTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isAI)
        {
            MoveAI();
        }
        else
        {
            float verticalMovement = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(0, verticalMovement * speed);
        }
    }

    private void MoveAI()
    {
        if (_ballTransform == null || _ballRb == null)
            return;

        _aiTimer += Time.deltaTime;

        if (_aiTimer >= aiReactionTime)
        {
            _aiTimer = 0f;
            CalculateAIMovement();
        }
    }

    private void CalculateAIMovement()
    {
        float ballY = _ballTransform.position.y;
        float ballVelocityY = _ballRb.velocity.y;
        float predictedBallY = ballY + (ballVelocityY * aiPredictionOffset);
        float paddleY = transform.position.y;
        float tolerance = 0.3f;

        float verticalMovement = 0f;

        if (predictedBallY > paddleY + tolerance)
            verticalMovement = 1f;
        else if (predictedBallY < paddleY - tolerance)
            verticalMovement = -1f;

        rb.velocity = new Vector2(0, verticalMovement * speed);
    }

    public void SetBallReference(Transform ballTransform, Rigidbody2D ballRb)
    {
        _ballTransform = ballTransform;
        _ballRb = ballRb;
    }
}