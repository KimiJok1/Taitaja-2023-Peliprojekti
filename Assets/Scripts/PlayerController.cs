using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Players properties
    public float speed;
    public float jumpForce;
    
    // Player controls
    [SerializeField] private float verticalInput;
    [SerializeField] private float horizontalInput;

    // Player assets
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;

    // Jumping properties
    [SerializeField] private bool isOnGround = false;
    [SerializeField] private bool doubleJump = true;
    [SerializeField] private bool flipSprite = false;

    void Start()
    {
        // Get player Rigidbody and SpriteRenderer
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get players input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Change player's horizontal velocity based on input
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // Check if spacebar pressed and player can jump
        if (Input.GetKeyDown(KeyCode.Space) && (isOnGround || doubleJump))
        {
            // Change player's vertical velocity based on input
            rb.velocity = Vector3.up * jumpForce;

            // Modify jump properties to make everything work
            if (isOnGround) 
                isOnGround = false;
            else
                doubleJump = false;
        }

        animator.SetBool("isMoving", Mathf.Round(rb.velocity.x * 100f) / 100f != 0);
        animator.SetFloat("yVelocity",rb.velocity.y);


        
        // Flip sprite's X if needed
        if (horizontalInput != 0)
            flipSprite = horizontalInput < 0;

        sprite.flipX = flipSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player collides with the level or an enemy
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            // TODO: add more checks to allow wall jumping

            // Enable jumping again
            isOnGround = true;
            doubleJump = true;
        }
    }
}
