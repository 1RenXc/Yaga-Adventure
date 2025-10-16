using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;

    private Animator anim;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (hit) return;

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit) return; // already processed
        hit = true;

        if (boxCollider != null) boxCollider.enabled = false;
        if (anim != null) anim.SetTrigger("explode");

        // Stop movement
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Start coroutine to deactivate after animation
        float delay = 0.5f; // default delay
        if (anim != null)
        {
            // Try to get current state's length if available (best-effort)
            try
            {
                // This is a best-effort; may return 0 if no state playing
                AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
                if (info.length > 0) delay = info.length;
            }
            catch { }
        }

        StartCoroutine(DeactivateAfterDelay(delay));
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != Mathf.Sign(_direction) && _direction != 0)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

        // If Rigidbody2D exists, set velocity immediately
        if (rb != null)
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Deactivate();
    }
}
