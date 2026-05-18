using UnityEngine;

public class CircuitOverload : MonoBehaviour
{
    public float lifeTime = 2.5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.TakeDamage(1);
        }
    }
}