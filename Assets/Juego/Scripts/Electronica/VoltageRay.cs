using UnityEngine;

public class VoltageRay : MonoBehaviour
{
    public float fallSpeed = 6f;
    public float lifeTime = 4f;

    private Vector3 targetScale;

    void Start()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * 8f
        );

        if (transform.position.y <= -4.5f)
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