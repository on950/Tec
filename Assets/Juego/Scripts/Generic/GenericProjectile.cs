using UnityEngine;

public class GenericProjectile : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;

    private Vector2 direction = Vector2.left;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Voltear sprite según dirección
        if (spriteRenderer != null)
        {
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    void Update()
    {
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);

        if (transform.position.x < -8f ||
            transform.position.x > 8f ||
            transform.position.y < -5f ||
            transform.position.y > 5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}