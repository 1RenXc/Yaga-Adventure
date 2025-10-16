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
        // Increase timer
        cooldownTimer += Time.deltaTime;

        // Jika klik kiri (tekan), cooldown sudah lewat, dan pemain boleh menyerang
        if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown && playerMovement != null && playerMovement.canAttack())
        {
            Attack(); // Panggil fungsi serangan
        }
    }

    private void Attack()
    {
        if (anim != null) anim.SetTrigger("attack"); // Aktifkan animasi serangan
        cooldownTimer = 0f; // Reset timer cooldown

        if (Magic == null || Magic.Length == 0)
        {
            Debug.LogWarning("PlayerAttack: No Magic projectile assigned.");
            return;
        }

        // Try to find an inactive projectile in the Magic array
        GameObject projectileObj = null;
        for (int i = 0; i < Magic.Length; i++)
        {
            if (Magic[i] == null) continue;
            if (!Magic[i].activeInHierarchy)
            {
                projectileObj = Magic[i];
                break;
            }
        }

        // If none inactive found, try to instantiate a new one from Magic[0] (if it's a prefab)
        if (projectileObj == null)
        {
            // If Magic[0] is a prefab (not in scene) we can instantiate it. Otherwise we'll reuse Magic[0].
            if (Application.isPlaying)
            {
                try
                {
                    projectileObj = Instantiate(Magic[0]);
                }
                catch
                {
                    // Fallback: reuse first element
                    projectileObj = Magic[0];
                }
            }
            else
            {
                projectileObj = Magic[0];
            }
        }

        if (projectileObj != null)
        {
            Vector3 spawnPos = (MagicPoint != null) ? MagicPoint.position : transform.position;
            projectileObj.transform.position = spawnPos;
            projectileObj.SetActive(true);
            Projectile p = projectileObj.GetComponent<Projectile>();
            if (p != null)
                p.SetDirection(Mathf.Sign(transform.localScale.x));
            else
                Debug.LogWarning("PlayerAttack: Selected Magic object has no Projectile component.");
        }
    }
}