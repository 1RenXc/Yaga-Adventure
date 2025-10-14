using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown; // Waktu jeda antar serangan
    [SerializeField] private Transform MagicPoint;
    [SerializeField] private GameObject[] Magic;
    private Animator anim;                         // Referensi ke komponen Animator
    private PlayerMovement playerMovement;         // Referensi ke skrip gerakan pemain
    private float cooldownTimer = Mathf.Infinity;  // Timer untuk mengatur cooldown

    private void Awake()
    {
        anim = GetComponent<Animator>();           // Ambil komponen Animator dari GameObject
        playerMovement = GetComponent<PlayerMovement>(); // Ambil skrip PlayerMovement
    }

    private void Update()
    {
        // Jika klik kiri, cooldown sudah lewat, dan pemain boleh menyerang
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
        {
            Attack(); // Panggil fungsi serangan
        }

        cooldownTimer += Time.deltaTime; // Tambahkan waktu setiap frame
    }

    private void Attack()
    {
        anim.SetTrigger("attack"); // Aktifkan animasi serangan
        cooldownTimer = 0;         // Reset timer cooldown

        Magic[0].transform.position = MagicPoint.position;
        Magic[0].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
}