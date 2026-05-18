using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private bool canTakeDamage = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canTakeDamage)
            return;

        if (collision.CompareTag("Enemy"))
        {
            GameManager.instance.TakeDamage(1);

            Destroy(collision.gameObject);
        }
    }

    public void DisableDamage()
    {
        canTakeDamage = false;
    }

    public void EnableDamage()
    {
        canTakeDamage = true;
    }
}