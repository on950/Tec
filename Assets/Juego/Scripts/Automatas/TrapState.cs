using UnityEngine;

public class TrapState : MonoBehaviour
{
    [Header("Duración")]
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(
            gameObject,
            lifeTime
        );
    }

    private void OnTriggerEnter2D(
        Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.TakeDamage(1);
        }
    }
}