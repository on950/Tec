using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 8f;

    public bool blueSoul = false;

    public GameObject ground;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ActivateRedSoul();
    }

    void Update()
    {
        if (blueSoul)
        {
            BlueSoulMovement();
        }
        else
        {
            RedSoulMovement();
        }
    }

    void RedSoulMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = movement.normalized * speed;
    }

    void BlueSoulMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

        if ((Input.GetKeyDown(KeyCode.Space) ||
     Input.GetKeyDown(KeyCode.JoystickButton1)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void ActivateRedSoul()
    {
        blueSoul = false;

        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        if (ground != null)
        {
            ground.SetActive(false);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = Color.red;
        }
    }

    public void ActivateBlueSoul()
    {
        blueSoul = true;

        rb.gravityScale = 3;
        rb.linearVelocity = Vector2.zero;

        if (ground != null)
        {
            ground.SetActive(true);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = Color.blue;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}