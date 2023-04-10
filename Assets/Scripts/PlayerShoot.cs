using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private float shootCooldown = 0.3f;

    [Header("Targets and Shooting")]
    [SerializeField] private LayerMask canShootLayers;
    [SerializeField] private Transform mainCamera = null;
    [SerializeField] private float range = 100f;
    [SerializeField] private float triggerThreshold = 0.5f;

    [Header("Gun Animation")]
    [SerializeField] private Animator gunAnimator;

    [Header("Muzzle Flash")]
    [SerializeField] private float muzzleFlashTimer = 1f;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private GameObject muzzleFlashPrefab;

    [Header("Bullet Hole Decals")]
    [SerializeField] private GameObject bulletHolePrefab;

    private float lastShootTime = 0f;

    private void Update() {
        float shootAxis = Input.GetAxisRaw("Shoot");
        if (shootAxis > triggerThreshold || shootAxis < -triggerThreshold) {
            if (Time.time > (lastShootTime + shootCooldown)) {
                Shoot();
                lastShootTime = Time.time;
            }
        }
    }

    private void Shoot() {
        RaycastHit hit;

        if (Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hit, range, canShootLayers)) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Structures")) {
                Instantiate(bulletHolePrefab, hit.point + (0.01f * hit.normal), Quaternion.LookRotation(-1 * hit.normal, hit.transform.up));
            } else {
                Health enemyHealth = hit.collider.GetComponent<Health>();
                if (enemyHealth != null) {
                    enemyHealth.TakeDamage(25);
                    if (enemyHealth.IsDead) {
                        Animator enemyAnimator = hit.collider.GetComponent<Animator>();
                        if (enemyAnimator != null) {
                            //enemyAnimator.SetBool("Dead", true);
                        } else {
                            hit.collider.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        if (gunAnimator != null) {
            gunAnimator.SetTrigger("Fire");
        }

        if (muzzleFlashPrefab != null) {
            GameObject flash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            Destroy(flash, muzzleFlashTimer);
        }
    }
}
