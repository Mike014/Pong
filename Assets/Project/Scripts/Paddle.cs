using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // GetAxis() è istantaneo grazie alle impostazioni
        float verticalMovement = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(0, verticalMovement * speed);
    }
}
