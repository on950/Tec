using UnityEngine;
using TMPro;

public class StateNode : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 6f;

    [Header("Texto")]
    public TMP_Text stateText;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetStateNumber(int number)
    {
        if (stateText != null)
        {
            stateText.text = "q" + number;
        }
    }

    void Update()
    {
        transform.position +=
            (Vector3)(direction *
            speed *
            Time.deltaTime);

        // Destruir cuando sale de pantalla
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