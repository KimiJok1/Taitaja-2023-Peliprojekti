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
    [SerializeField] private AnimationClip death;
    [SerializeField] private BoxCollider2D collider;

    // Jumping properties
    [SerializeField] private bool isOnGround = false;
    [SerializeField] private bool doubleJump = true;
    [SerializeField] private bool flipSprite = false;
    [SerializeField] private float oldPosX = 0;

    void Start()
    {
        // Get player Rigidbody and SpriteRenderer
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Get players input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

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
        if(Mathf.Abs(transform.position.x - oldPosX) > 0.001){
            animator.SetBool("isMoving", true);
        }
        else{
            animator.SetBool("isMoving", false);
        }
        
        animator.SetFloat("yVelocity",rb.velocity.y);
        
        // Flip sprite's X if needed
        if (horizontalInput != 0)
            flipSprite = horizontalInput < 0;

        sprite.flipX = flipSprite;
        collider.offset = new Vector2(flipSprite ? -0.125f : 0.125f, collider.offset.y);

        oldPosX = transform.position.x;
        if (Input.GetKeyDown("f")){
            StartCoroutine("Die");
        }
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

    IEnumerator Die(){
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(death.length);

    }
}
