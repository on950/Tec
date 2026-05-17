using UnityEngine;

public class SignalWave : MonoBehaviour
{
    public float speed = 8f;

    public Vector2 direction = Vector2.left;

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (transform.position.x < -8 ||
            transform.position.x > 8 ||
            transform.position.y < -5 ||
            transform.position.y > 5)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.TakeDamage(1);

            Destroy(gameObject);
        }
    }
}