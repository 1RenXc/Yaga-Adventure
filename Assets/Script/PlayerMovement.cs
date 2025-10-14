using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    [SerializeField] private float speed;       // kecepatan gerak kiri-kanan
    [SerializeField] private float jumpForce;   // kekuatan lompat

    private int jumpCount = 0;                  // menghitung jumlah lompatan
    [SerializeField] private int maxJumps = 2;  // jumlah maksimum lompatan (2 = double jump)
    private bool isFacingRight = true;          // arah karakter (default menghadap kanan)

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Gerak kiri-kanan
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        // Set parameter animator
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", grounded);

        // Flip karakter saat bergerak kiri/kanan
        if (horizontalInput > 0.01f && !isFacingRight)
            Flip();
        else if (horizontalInput < -0.01f && isFacingRight)
            Flip();

        // Lompat dengan batas jumlah tertentu
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            jumpCount++;
            grounded = false; // karakter tidak di tanah saat lompat
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset jumlah lompatan saat menyentuh tanah
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            jumpCount = 0;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Fungsi baru untuk cek apakah karakter bisa menyerang
    public bool canAttack()
    {
        // Contoh: karakter bisa menyerang hanya saat berada di tanah
        return grounded;
    }
}
