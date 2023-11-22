using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GroundDetection groundDetection;
    [SerializeField] GameObject doubleJumpFX;

    bool hasDoubleJump;

    void Start()
    {
        
    }

    private void Update()
    {
        if (GameManager.playerAlive && UpPressed())
        {
            if (groundDetection.IsOnGround())
            {
                rb.velocity = Vector2.right * rb.velocity.x + Vector2.up * jumpPower;
            }
            else if (hasDoubleJump)
            {
                rb.velocity = Vector2.right * rb.velocity.x + Vector2.up * jumpPower * .9f;
                hasDoubleJump = false; 
            }
        }
    }

    void FixedUpdate()
    {
        if (GameManager.playerAlive)
        {
            HandleMovement();
        }

        if (groundDetection.IsOnGround())
        {
            hasDoubleJump = true;
        }
    }

    [SerializeField] GameObject deathFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
            GameManager.playerAlive = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed, jumpPower;
    void HandleMovement()
    {
        if (RightHeld())
        {
            if (!LeftHeld())
            {
                rb.velocity = Vector2.right * moveSpeed + Vector2.up * rb.velocity.y;
            }
            else
            {
                rb.velocity = Vector2.up * rb.velocity.y;
            }
        }
        else if (LeftHeld())
        {
            rb.velocity = Vector2.left * moveSpeed + Vector2.up * rb.velocity.y;
        }
        else
        {
            rb.velocity = Vector2.up * rb.velocity.y;
        }
    }

    #region Handle Input
    bool LeftHeld()
    {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    }
    bool RightHeld()
    {
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }
    bool UpPressed()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
    }
    #endregion
}
