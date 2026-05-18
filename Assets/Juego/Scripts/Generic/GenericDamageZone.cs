using UnityEngine;

public class GenericDamageZone : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 1;
    public bool destroyOnHit = false;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.TakeDamage(damage);

            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }
}