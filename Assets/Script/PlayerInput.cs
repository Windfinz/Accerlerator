using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float moveX, moveY;
    public Rigidbody2D rb;

    [SerializeField]
    private float speed = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerInputMovement();
        Move();   
    }


    private void PlayerInputMovement()
    {
        moveX = Input.GetAxisRaw("Horizontal") * speed;
        moveY = Input.GetAxisRaw("Vertical") * speed;
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveX, moveY);
    }
}
