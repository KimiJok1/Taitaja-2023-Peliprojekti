using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Players properties
    public float speed;
    public float jumpForce;
    
    // Player controls
    [SerializeField] private bool canMove;
    [SerializeField] private float verticalInput;
    [SerializeField] private float horizontalInput;

    // Player assets
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D plrCollider;

    // Jumping properties
    [SerializeField] private bool doubleJump = true;
    [SerializeField] private bool isOnGround = false;

    // Sprite properties
    [SerializeField] private bool flipSprite = false;
    [SerializeField] private float oldPosX = 0;

    void Start()
    {
        // Get every asset for player
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        plrCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Get players input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (!canMove) return;

        // Change player's horizontal velocity based on input
        float dir = horizontalInput < 0 ? -1 : 1;
        rb.velocity = new Vector2(Mathf.Abs(horizontalInput) * speed * dir, rb.velocity.y);

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

        // Set animator properties
        animator.SetBool("isMoving", Mathf.Abs(transform.position.x - oldPosX) > 0.001);
        animator.SetFloat("yVelocity",rb.velocity.y);
        
        // Check if sprite needs to be flipped
        if (horizontalInput != 0)
            flipSprite = horizontalInput < 0;

        // Flip sprite's X if needed
        sprite.flipX = flipSprite;
        plrCollider.offset = new Vector2(flipSprite ? -0.125f : 0.125f, plrCollider.offset.y);

        // Update old position
        oldPosX = transform.position.x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player collides with the level or an enemy
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            bool checkDir = true;
            ContactPoint2D[] allPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(allPoints);

            foreach (var i in allPoints)
                if (i.point.y > transform.position.y) 
                    checkDir = false;

            // Enable jumping again
            if (checkDir)
            {
                isOnGround = checkDir;
                doubleJump = checkDir;
            }
        }

        if (collision.gameObject.CompareTag("Spikes"))
        {
            canMove = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            bool checkDir = true;
            ContactPoint2D[] allPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(allPoints);

            foreach (var i in allPoints)
                if (i.point.y > transform.position.y) 
                    checkDir = false;

            if (checkDir && !isOnGround && !doubleJump)
            {
                isOnGround = checkDir;
                doubleJump = checkDir;
            }
        }
    }
}
