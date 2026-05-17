using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 1.5f; // Speed of the player movement   
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 movement; // Store the player's movement input
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement=movement.normalized; // Normalize the movement vector to prevent faster diagonal movement
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);
    }


    private void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement * speed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cooperativa"))
        {
            SceneManager.LoadScene(5);
        }
    }
}