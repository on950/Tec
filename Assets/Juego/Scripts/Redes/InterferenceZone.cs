using UnityEngine;

public class InterferenceZone : MonoBehaviour
{
    public float lifeTime = 3f;

    public float growSpeed = 4f;

    Vector3 targetScale;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        targetScale = transform.localScale;

        transform.localScale = Vector3.zero;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.localScale =
        Vector3.Lerp(
        transform.localScale,
        targetScale,
        growSpeed * Time.deltaTime);

        if (lifeTime < 1f)
        {
            Color c = sr.color;

            c.a = lifeTime;

            sr.color = c;
        }

        lifeTime -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.TakeDamage(1);
        }
    }
}