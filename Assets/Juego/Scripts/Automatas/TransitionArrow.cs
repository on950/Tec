using UnityEngine;

public class TransitionArrow : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 8f;

    private Vector2 direction = Vector2.down;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position +=
            (Vector3)(direction *
            speed *
            Time.deltaTime);

        // Destruir cuando salga de la BattleBox
        if (transform.position.x < -7f ||
            transform.position.x > 7f ||
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
            GameManager.instance.TakeDamage(1);

            Destroy(gameObject);
        }
    }
}