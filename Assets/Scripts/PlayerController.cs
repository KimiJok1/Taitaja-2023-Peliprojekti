using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    
    private float verticalInput;
    private float horizontalInput;

    private Rigidbody2D rb;
    private bool isOnGround = false;

    private bool doubleJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && (isOnGround || doubleJump))
        {
            rb.velocity = Vector3.up * jumpForce;
            if (isOnGround) 
                isOnGround = false;
            else
                doubleJump = false;
        }
    }

    private bool checkDir = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            checkDir = true;
            
            /*ContactPoint2D[] allPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(allPoints);

            foreach (var i in allPoints)
                if (i.point.y > transform.position.y) 
                    checkDir = false;*/

            if (checkDir)
            {
                isOnGround = checkDir;
                doubleJump = checkDir;
            }
        }
    }
}
