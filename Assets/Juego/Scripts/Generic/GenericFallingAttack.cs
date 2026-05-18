using UnityEngine;

public class GenericFallingAttack : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;

    [Header("Rotación visual")]
    public float rotationZ = -180f;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    void Update()
    {
        transform.position +=
            Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < -5f)
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