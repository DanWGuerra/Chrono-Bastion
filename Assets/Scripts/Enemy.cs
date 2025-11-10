using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public float health = 1f;
    public float damageToPlayer = 1f;

    [Header("Audio")]
    public AudioClip hitSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    private Transform target;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float flashDuration = 0.1f;
    private bool isDying = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Tower").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
    }

    void Update()
    {
        if (isDying || target == null) return;

        // Move toward tower
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // Look at tower (top-down, sprite facing downward)
        Vector2 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        // ↑ +90 so that sprites that face “down” visually now look toward the tower
    }

    public void TakeDamage(float amount)
    {
        if (isDying) return;

        health -= amount;
        PlaySound(hitSound);
        FlashRed();

        if (health <= 0)
            Die();
    }

    void Die()
    {
        if (isDying) return;
        isDying = true;

        GameManager.Instance.AddPoints();
        PlaySound(deathSound);

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        moveSpeed = 0;

        float delay = deathSound != null ? deathSound.length : 0f;
        Destroy(gameObject, delay);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDying) return;

        if (other.CompareTag("Tower"))
        {
            GameManager.Instance.LoseTime(damageToPlayer);
            Destroy(gameObject);
        }
    }

    void FlashRed()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.color = Color.red;
        CancelInvoke(nameof(ResetColor));
        Invoke(nameof(ResetColor), flashDuration);
    }

    void ResetColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clip);
        }
    }
}
