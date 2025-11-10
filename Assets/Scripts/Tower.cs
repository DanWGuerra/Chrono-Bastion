using UnityEngine;
using System.Linq;

[ExecuteAlways] // So it also updates in the Editor view
[RequireComponent(typeof(LineRenderer))]
public class Tower : MonoBehaviour
{
    public static Tower Instance;

    private Animator animator;

    [Header("Tower Settings")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float bulletSpeed = 10f;
    public float range = 10f;
    public float damage = 1f;

    public AudioSource shootSFX;

    private float nextFireTime;
    private LineRenderer lineRenderer;

    [Header("Visual Settings")]
    [SerializeField] private int circleSegments = 60;
    [SerializeField] private Color circleColor = new Color(0f, 1f, 0f, 0.3f);

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        SetupLineRenderer();
    }

    private void Start()
    {
        Instance = this;
        UpdateRangeCircle();
    }

    void Update()
    {
        GameObject closest = FindClosestEnemy();
        nextFireTime += Time.deltaTime;
        if (closest == null) return;

        


        Vector2 dir = closest.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle +90);
       

        if (nextFireTime >= fireRate)
        {
            nextFireTime = 0;
            Shoot(dir.normalized);
        }

        UpdateRangeCircle();
    }

    GameObject FindClosestEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return null;

        return enemies
            .OrderBy(e => Vector2.Distance(e.transform.position, transform.position))
            .FirstOrDefault(e => Vector2.Distance(e.transform.position, transform.position) <= range);
    }

    void Shoot(Vector2 direction)
    {
        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        animator.Play("Shoot");
        shootSFX.Play();

        // Set bullet direction and rotation (facing upwards by default)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Apply velocity
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;

        // Assign tower’s damage to the bullet
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
            bulletComponent.damage = damage;
    }

    void SetupLineRenderer()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = circleSegments + 1;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = circleColor;
        lineRenderer.endColor = circleColor;
    }

    void UpdateRangeCircle()
    {
        if (lineRenderer == null) return;

        float angleIncrement = 360f / circleSegments;
        for (int i = 0; i <= circleSegments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleIncrement);
            float x = Mathf.Cos(angle) * range;
            float y = Mathf.Sin(angle) * range;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
